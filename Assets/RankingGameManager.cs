using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace RankingScene
{

    enum NextGameSate
    {
        Idle = 0,
        Home = 1,
        StageSelect = 2,
    }

    public class RankingGameManager : MonoBehaviour
    {
        // Use this for initialization
        [SerializeField]
        public GameObject ListItemPrefab;
        public GameObject ListContentsObject;
        public GameObject HomeButton;
        public GameObject FadeOutPanel;
        public GameObject ScoreRankingManager;
        public GameObject NameEntryDialog;
        public GameObject InputFieldEnterButton;
        public GameObject YourRankText;
        public GameObject ErrorText;
        public GameObject YourScoreText;

        private GameObject[] listItem;
        private NextGameSate nextState;
        private bool animationEndFlag;
        private ScoreList [] scoreList;
        private bool alreadyGetListFlag;
        private bool exceptionOccured;
        private int myStage;
        private int myHiScore;
        private bool nowRankingIn;

        const int LIST_NUMBER = 50;


        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            listItem = new GameObject[LIST_NUMBER];

            for (int i = 0; i < LIST_NUMBER; i++)
            {
                listItem[i] = GameObject.Instantiate(ListItemPrefab) as GameObject;
                listItem[i].transform.SetParent(ListContentsObject.transform, false);

                listItem[i].transform.SetParent(ListContentsObject.transform, false);
                var text = listItem[i].GetComponentInChildren<Text>();
                text.text = (i + 1).ToString() + " NOW DOWNLOADING...  ";

                if ((i % 2) == 0)
                {
                    listItem[i].GetComponent<Image>().color = new Color(0.5f, 0.7f, 0.5f + i * 0.05f);
                }
                else
                {
                    listItem[i].GetComponent<Image>().color = new Color(0.5f, 0.5f + i * 0.005f, 0.7f);
                }
            }

            Button btn = HomeButton.GetComponent<Button>();
            btn.onClick.AddListener(this.GoHome);

            Button btn1 = InputFieldEnterButton.GetComponent<Button>();
            btn1.onClick.AddListener(EnteredYourName);
            
            FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

            nextState = NextGameSate.Idle;

            int hiscore = 0;// = PlayerPrefs.GetInt("HI_SCORE", 2000);
            int stage = 1;
            int clearedstage;
            for (int i = (int)1; i < (int)LIST_NUMBER + 1; i++)
            {
                clearedstage = PlayerPrefs.GetInt("CLEARED" + i, 0);

                if (clearedstage == (int)1)
                {
                    hiscore += PlayerPrefs.GetInt("HI_SCORE" + i, 0);
                    stage = i;
                }
                else
                {
                    break;
                }
            }
            myHiScore = hiscore;
            myStage = stage;

            exceptionOccured = false;
            try
            {
                ScoreRankingManager.GetComponent<Test>().GetScorePreparation(myHiScore, myStage);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            alreadyGetListFlag = false;

            NameEntryDialog.SetActive(false);

            StartCoroutine("ScrollRanking");

            nowRankingIn = false;

        }

        // Update is called once per frame
        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            if (exceptionOccured)
            {
                return;
            }
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

            if (Input.GetMouseButtonDown(0))    // キャンセル可能
            {
                StopCoroutine("ScrollRanking");
            }

            if (ScoreRankingManager.GetComponent<Test>().HighScoreEntryFlag == true)
            {
                ScoreRankingManager.GetComponent<Test>().HighScoreEntryFlag = false;

                NameEntryDialog.SetActive(true);
                ErrorText.GetComponent<Text>().text = "";
                YourRankText.GetComponent<Text>().text = "YOUR RANKING " + ScoreRankingManager.GetComponent<Test>().YourRanking;
                YourScoreText.GetComponent<Text>().text = "YOUR SCORE " + myHiScore;
            }

            //----------------------------------------------------------------------
            // Go home
            if (animationEndFlag)
            {
                animationEndFlag = false;

                if (nextState == NextGameSate.Idle)
                {

                }
                else if (nextState == NextGameSate.Home)
                {
                    SceneManager.LoadScene("TitleMain");
                }
            }

            if (alreadyGetListFlag == false)
            {
                try
                {
                    bool readdone = ScoreRankingManager.GetComponent<Test>().GetScore(out scoreList);
                    if (readdone && (scoreList != null))
                    {
                        for (int i = 0; i < LIST_NUMBER; i++)
                        {
                            var text = listItem[i].GetComponentInChildren<Text>();
                            //text.text = (i + 1).ToString() + "   ";
                            text.text = (i + 1).ToString() + " "                        // Ranking
                                + (scoreList[i].Score).ToString() + "  "                // Score
                                + "STAGE " + (scoreList[i].Stage).ToString() + "  "     // Stage
                                + scoreList[i].Name;                                    // Name

                            if ((i + 1) == ScoreRankingManager.GetComponent<Test>().YourRanking) {
                                text.color = new Color(1.0f, 0.0f, 0.0f);
                                nowRankingIn = true;
                            }
                        }           
                        alreadyGetListFlag = true;
                    }
                }
                catch
                {
                    exceptionOccured = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void EnteredYourName()
        {
            string str = NameEntryDialog.transform.Find("InputField").GetComponent<InputField>().text;
            if (str.Length > 8)
            {
                NameEntryDialog.transform.Find("InputField").GetComponent<InputField>().text = str.Substring(0, 8);
                ErrorText.GetComponent<Text>().text = TextResources.Text[(int)TextNumber.CHARACTER_LESS_THAN_8, (int)GlobalVariables.Language];
                ErrorText.GetComponent<Text>().font.name = GlobalVariables.LanguageFont;
                return;
            }
            PlayerPrefs.SetString("YOUR_NAME", str);
            ScoreRankingManager.GetComponent<Test>().RankMyScore(myHiScore, myStage);
            alreadyGetListFlag = false; // 再度リスト表示を更新
            NameEntryDialog.SetActive(false);
        }


        /// <summary>
        /// 
        /// </summary>
        void GoHome()
        {
            if (nowRankingIn == true)   // ランク外なのでセーブしない
            {
                try
                {
                    ScoreRankingManager.GetComponent<Test>().SetScore();
                }
                catch
                {
                    exceptionOccured = true;
                    SceneManager.LoadScene("TitleMain");

                    return;
                }
            }

            StartCoroutine("FadeOutScreen");  // ここでホームに戻るとシーンが破壊されて保存されない！！！！！！
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeOutScreen()
        {
            float sec = 0.5f;
            for (float i = 0; i < 1f; i += 1f / (sec * 60))
            {
                // camera.orthographicSize /= 1.1f;
                FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, i);

                yield return new WaitForEndOfFrame();
            }
            if ((exceptionOccured == false) && (nowRankingIn == true))
            {
                bool done;
                int count = 0;
                while (true)
                {
                    count++;
                    if (count >= 60 * 10)   // 10second timeout.
                    {
                        exceptionOccured = true;        // Timeout
                        break;
                    }

                    try
                    {
                        done = ScoreRankingManager.GetComponent<Test>().IsSetDone();
                    }
                    catch
                    {
                        exceptionOccured = true;
                        animationEndFlag = true;
                        SceneManager.LoadScene("TitleMain");

                        yield break;
                    }

                    if (done == false)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    else
                    {
                        break;
                    }
                }

            }

            SceneManager.LoadScene("TitleMain");
            //nextState = NextGameSate.Home;  // Homeに戻る
            animationEndFlag = true;

            yield break;
        }

        /// <summary>
        /// 
        /// </summary>
        IEnumerator ScrollRanking()
        {
            yield return new WaitForSeconds(2.5f);

            float y = (2  /* (listItem[0].GetComponent<VerticalLayoutGroup>().spacing */ + listItem[0].GetComponent<RectTransform>().sizeDelta.y) * (float)(LIST_NUMBER);
            float windowsize = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
            while (true)
            {
                Vector3 pos = ListContentsObject.transform.position;
                pos.y += 0.02f;
                ListContentsObject.transform.position = pos;
                Vector3 pos1 = transform.InverseTransformPoint(ListContentsObject.transform.position);
                if (pos1.y >= (y - windowsize))
                {
                    yield break;
                }
                yield return new WaitForEndOfFrame();

            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum NextGameSate
{
    Idle = 0,
    Home = 1,
    StageSelect = 2,
}

public class StageSelectGameManager : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    public GameObject ListItemPrefab;
    public GameObject ListContentsObject;
    public GameObject HomeButton;
    public GameObject FadeOutPanel;

    private GameObject[] listItem;
    private NextGameSate nextState;
    private bool animationEndFlag;
    private int thresholdLevel; // 選択可能なステージの閾値（この数字以下はOK）

    void Start()
    {
        listItem = new GameObject[(int)Stage.S_MAX];
        Button btn;

        thresholdLevel = 1;

        for (int i = (int)Stage.S_1 - 1; i < (int)Stage.S_MAX - 1; i++)
        {

            listItem[i] = GameObject.Instantiate(ListItemPrefab) as GameObject;
            listItem[i].transform.SetParent(ListContentsObject.transform, false);

            listItem[i].transform.SetParent(ListContentsObject.transform, false);
            var text = listItem[i].GetComponentInChildren<Text>();
            text.text = "STAGE " + (i + 1).ToString() + "   ";

            if ((i % 2) == 0)
            {
                listItem[i].GetComponent<Image>().color = new Color(0.5f, 0.7f, 0.5f + i * 0.05f);
            }
            else
            {
                listItem[i].GetComponent<Image>().color = new Color(0.5f, 0.5f + i * 0.005f, 0.7f);
            }

            btn = listItem[i].GetComponent<Button>();
            //btn.onClick.AddListener(SelectStage);
            int n = i;
            btn.onClick.AddListener(() => SelectStage(n));

            int stageStatus = PlayerPrefs.GetInt("CLEARED" + (i + 1), 0);

            if (i == 0) // ステージ１は最初からアンロックされている
            {
                listItem[i].transform.Find("LockedImage").gameObject.SetActive(true);
                listItem[i].transform.Find("LockedImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("UnLocked");
            }

            if (stageStatus == (int)1)
            {
                listItem[i].transform.Find("LockedImage").gameObject.SetActive(false);
                listItem[i].transform.Find("ClearedTextF").gameObject.SetActive(true);
            }
            else
            {
                if (PlayerPrefs.GetInt("CLEARED" + (i), 0) == 1)
                {
                    thresholdLevel = i + 1;
                    listItem[i].transform.Find("LockedImage").gameObject.SetActive(true);
                    listItem[i].transform.Find("LockedImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("UnLocked");
                    listItem[i].transform.Find("ClearedTextF").gameObject.SetActive(false);
                }
                else
                {
                    listItem[i].transform.Find("LockedImage").gameObject.SetActive(true);
                    listItem[i].transform.Find("ClearedTextF").gameObject.SetActive(false);
                }
            }

        }

        btn = HomeButton.GetComponent<Button>();
        btn.onClick.AddListener(this.GoHome);

        FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

        nextState = NextGameSate.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        if (animationEndFlag)
        {
            animationEndFlag = true;

            if (nextState == NextGameSate.Idle)
            {

            }
            else if (nextState == NextGameSate.Home)
            {
                SceneManager.LoadScene("TitleMain");
            }
            else if (nextState == NextGameSate.StageSelect)
            {
                SceneManager.LoadScene("GameMain");
            }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    void GoHome()
    {
        nextState = NextGameSate.Home;
        StartCoroutine("FadeOutScreen");
    }

    /// <summary>
    /// 
    /// </summary>
    void SelectStage(int n)
    {
        if (n + 1 >= (int)Stage.S_MAX)
        {
            return;
        }
        else if (n + 1 > thresholdLevel)
        {
            listItem[n].transform.Find("LockedImage").GetComponent<Animation>().Stop();
            listItem[n].transform.Find("LockedImage").GetComponent<Animation>().Play();

            return;
        }
        PlayerPrefs.SetInt("CURRENT_STAGE", n + 1);

        nextState = NextGameSate.StageSelect;
        StartCoroutine("FadeOutScreen");
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

        animationEndFlag = true;

        yield break;
    }

}

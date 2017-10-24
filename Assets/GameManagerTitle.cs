using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NendUnityPlugin.AD;
using NendUnityPlugin.Common;

enum CurrentScene
{
    Idle = 0,
    GameMain,
    StageSelect,
    Ranking,
}

public class GameManagerTitle : MonoBehaviour {

    public GameObject RestartButton;
    public GameObject StageSelectButton;
    public GameObject RankingButton;
    public GameObject ShareButton;
    public GameObject LifeText;
    public GameObject Remain;
    public GameObject TimeToRestartText;
    public GameObject PanelMovingParticle;
    public GameObject ScoreText;
    public GameObject FadeOutPanel;
    public GameObject LeftEye;
    public GameObject RightEye;
    public GameObject BackGroundParticle;

    public NendAdInterstitial NendAdInterstitial;//インタースティシャル広告

    private GameObject [] Remains;
    private int oneSecond;
    private Color buttonDefaultColor;
    private Color buttonTextDefaultColor;
    private bool animationEndFlag;
    private CurrentScene currentStage;

    // Use this for initialization
    void Start() {
        RightEye.SetActive(false);
        LeftEye.SetActive(false);

        Button btn = RestartButton.GetComponent<Button>();
        btn.onClick.AddListener(GoStartGame);

        btn = StageSelectButton.GetComponent<Button>();
        btn.onClick.AddListener(GoSelectStage);

        btn = RankingButton.GetComponent<Button>();
        btn.onClick.AddListener(GoRanking);

        btn = ShareButton.GetComponent<Button>();
        btn.onClick.AddListener(GoShare);

        GlobalVariables.Life = PlayerPrefs.GetInt("LIFE", GlobalVariables.LifeMax);
        Remains = new GameObject[GlobalVariables.LifeMax];

        for (int i = 0; i < GlobalVariables.LifeMax; i++)
        {
            Vector3 vec = LifeText.transform.position;

            Remains[i] = Instantiate(Remain, vec + new Vector3(0.4f + i * 0.4f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            Remains[i].SetActive(false);
        }

        string str = PlayerPrefs.GetString("RESTART_TIME", "@");    // ゲーム全くの初めての場合
        if (str == "@")
        {
            GlobalVariables.RestartTime = DateTime.Now.AddMinutes(-GlobalVariables.TimeToStart * GlobalVariables.LifeMax);
        }
        else
        {
            GlobalVariables.RestartTime = DateTime.FromBinary(Convert.ToInt64(str));
        }

        //for (int i = 0; i < GlobalVariables.Life; i++)
        //{
        //    Remains[i].SetActive(true);
        //}
        //if (GlobalVariables.Life == GlobalVariables.LifeMax)
        //{
        //    TimeToRestartText.SetActive(false);
        //}
        //else if (GlobalVariables.Life == 0)
        //{
        //    RestartButton.transform.GetChild(0).GetComponent<Text>().text = "動画を見て再スタート";
        //    StageSelectButton.transform.GetChild(0).GetComponent<Text>().text = "動画を見てステージ選択";
        //}

        //string str = PlayerPrefs.GetString("RESTART_TIME", "@");    // ステージクリア時、復活タイマー0からスタート
        //if (str == "@")
        //{
        //    GlobalVariables.RestartTime = System.DateTime.Now;
        //    restartTime = GlobalVariables.RestartTime.AddMinutes(-GlobalVariables.TimeToStart * GlobalVariables.Life);     // ライフ分時間を過去に遡る
        //}
        //else
        //{
        //    GlobalVariables.RestartTime = System.DateTime.FromBinary(System.Convert.ToInt64(str));
        //    restartTime = GlobalVariables.RestartTime;
        //}

        int score = 0;
        for (int i = (int)Stage.S_1; i < (int)Stage.S_MAX; i++)
        {
            score += PlayerPrefs.GetInt("HI_SCORE" + i, 0);
        }
        ScoreText.GetComponent<Text>().text = "SCORE\n" + score;

        oneSecond = 59;

        string language = Application.systemLanguage.ToString();
        language = "English";
        if (language == "Japanese")
        {
            GlobalVariables.Language = LanguageType.Japanese;
            GlobalVariables.LanguageFont = "AldotheApache";
        }
        else if (language == "English")
        {
            GlobalVariables.Language = LanguageType.English;
            GlobalVariables.LanguageFont = "AldotheApache";
        }
        else
        {
            GlobalVariables.Language = LanguageType.Other;
            GlobalVariables.Language = LanguageType.English;  //他の言語は英語
            GlobalVariables.LanguageFont = "AldotheApache";
        }

        buttonDefaultColor = StageSelectButton.GetComponent<Image>().color;
        buttonTextDefaultColor = StageSelectButton.transform.GetChild(0).GetComponent<Text>().color;

        FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

        animationEndFlag = false;
        currentStage = CurrentScene.Idle;

        NendAdInterstitial.Instance.Load("a6eca9dd074372c898dd1df549301f277c53f2b9", "3172");

        NendAdInterstitial.Instance.Show();
    }

    // Update is called once per frame
    void Update() {
        //-------------------------------------------------------------------------
        // Stage遷移
        //-------------------------------------------------------------------------
        if (animationEndFlag)
        {
            switch (currentStage)
            {
                case CurrentScene.GameMain:
                    //SceneManager.LoadScene("OpeningAnimation");
                    int cleared;
                    int stage = (int)Stage.S_1;

                    for (int i = (int)Stage.S_1; i < (int)Stage.S_MAX; i++)
                    {
                        cleared = PlayerPrefs.GetInt("CLEARED" + i, 0);

                        if (cleared == (int)1)
                        {
                            stage = i;
                            break;
                        }
                    }
                    ///////////■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    ///////////■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    ///////////■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    if (true) // ((stage % 50) == 0)
                    ///////////■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    ///////////■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    ///////////■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    {
                        SceneManager.LoadScene("OpeningExplanation");
                        return;
                    }

                    SceneManager.LoadScene("GameMain");
                    break;

                case CurrentScene.StageSelect:
                    SceneManager.LoadScene("StageSelect");
                    break;

                case CurrentScene.Ranking:
                    SceneManager.LoadScene("Ranking");
                    break;

            }
        }

        //-------------------------------------------------------------------------
        // Lifeの処理
        //-------------------------------------------------------------------------
        if ((++oneSecond % 6) == 0)
        {
            oneSecond = 0;
            LifeControl();

            if (GlobalVariables.Life > 0)
            {
                StageSelectButton.GetComponent<Button>().enabled = true;
                RestartButton.GetComponent<Button>().enabled = true;
                StageSelectButton.GetComponent<Image>().color = buttonDefaultColor;
                RestartButton.GetComponent<Image>().color = buttonDefaultColor;
                StageSelectButton.transform.GetChild(0).GetComponent<Text>().color = buttonTextDefaultColor;
                RestartButton.transform.GetChild(0).GetComponent<Text>().color = buttonTextDefaultColor;
            }
            else
            {
                StageSelectButton.GetComponent<Button>().enabled = false;
                RestartButton.GetComponent<Button>().enabled = false;
                StageSelectButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
                RestartButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
                StageSelectButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f); ;
                RestartButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f); ;
            }

        }

    }


    /// <summary>
    /// 
    /// </summary>
    void LifeControl()
    {
        int timeToStart = GlobalVariables.TimeToStart;   // minutes.
        DateTime now = DateTime.Now;
        TimeSpan ts = now - GlobalVariables.RestartTime;    //DateTime の差が TimeSpan として返る
        if (ts.TotalSeconds > GlobalVariables.TimeToStart * 60 * GlobalVariables.LifeMax)    // Lifeが満タンの時
        {
            GlobalVariables.RestartTime = now.AddSeconds(-GlobalVariables.TimeToStart * 60 * GlobalVariables.LifeMax);
        }

        int newlife = 0;
        int remainTime = DateTime.Now.Subtract(GlobalVariables.RestartTime).Minutes;
        if (timeToStart * 60 * 1 <= ts.TotalSeconds)
        {
            newlife = 1;
            RestartButton.transform.GetChild(0).GetComponent<Text>().text = TextResources.Text[(int)TextNumber.START, (int)GlobalVariables.Language];  // ]"START";
        }
        if (timeToStart * 60 * 2 <= ts.TotalSeconds)
        {
            newlife = 2;
        }
        if (timeToStart * 60 * 3 <= ts.TotalSeconds)
        {
            newlife = 3;
            TimeToRestartText.SetActive(false);
        }
        string str = TextResources.Text[(int)TextNumber.TIME_TO_RESTART, (int)GlobalVariables.Language] + (timeToStart - 1 - ((int)ts.TotalMinutes % timeToStart)).ToString("00") + ":" + (60 - 1 - ((int)ts.TotalSeconds % 60)).ToString("00");
        TimeToRestartText.GetComponent<Text>().text = str;
        TimeToRestartText.GetComponent<Text>().font = Resources.Load(GlobalVariables.LanguageFont) as Font;

        DisplayLife(newlife);

        GlobalVariables.Life = newlife;
        PlayerPrefs.SetInt("LIFE", GlobalVariables.Life);
        PlayerPrefs.SetString("RESTART_TIME", GlobalVariables.RestartTime.ToBinary().ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    void DisplayLife(int life)
    {
        int offset = 1;

        //if (gameStatus != GameStatus.GameEnd2)
        //{
        //    offset = 2;
        //}

        for (int i = 0; i < GlobalVariables.LifeMax; i++)
        {
            if (i <= life - offset)    // 現在の火花の数の1少ない数を表示
            {
                Remains[i].SetActive(true);
            }
            else
            {
                Remains[i].SetActive(false);
            }
        }

        if (life > GlobalVariables.Life)
        {
            if (life - offset >= 0)
            {
                PanelMovingParticle.SetActive(false);
                PanelMovingParticle.SetActive(true);
                PanelMovingParticle.transform.position = Remains[life - offset].transform.position;
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    void GoStartGame()
    {
        PlayerPrefs.SetInt("CURRENT_STAGE", 0);     // クリアしたステージから
        currentStage = CurrentScene.GameMain;
        StartCoroutine("FadeOutScreen");
        //RightEye.SetActive(true);
        LeftEye.SetActive(true);

        BackGroundParticle.GetComponent<ParticleSystem>().startSpeed = 2.5f;
        BackGroundParticle.GetComponent<ParticleSystem>().startSize = 2.0f;
        BackGroundParticle.GetComponent<ParticleSystem>().startColor = new Color(1f, 0.3f, 0.0f);

    }

    /// <summary>
    /// 
    /// </summary>
    void GoSelectStage()
    {
        currentStage = CurrentScene.StageSelect;
        StartCoroutine("FadeOutScreen");
    }

    /// <summary>
    /// 
    /// </summary>
    void GoRanking()
    {
        currentStage = CurrentScene.Ranking;
        StartCoroutine("FadeOutScreen");
    }

    /// <summary>
    /// 
    /// </summary>
    void GoShare()
    {
        PlayerPrefs.DeleteAll();    // 最初から 
        GlobalVariables.RestartTime = GlobalVariables.RestartTime.AddMinutes(-1);   //ライフ1つ増やす

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutScreen()
    {
        float sec = 0.8f;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
   
    private GameObject [] Remains;
    private int oneSecond;


    // Use this for initialization
    void Start() {
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

    }

    // Update is called once per frame
    void Update() {
        //-------------------------------------------------------------------------
        // Lifeの処理
        //-------------------------------------------------------------------------
        if ((++oneSecond % 6) == 0)
        {
            oneSecond = 0;
            LifeControl();

            //    int timeToStart = GlobalVariables.TimeToStart;   // minutes.
            //    DateTime now = DateTime.Now;
            //    TimeSpan ts = now - GlobalVariables.RestartTime;    //DateTime の差が TimeSpan として返る
            //    if (ts.TotalSeconds > GlobalVariables.TimeToStart * 60 * GlobalVariables.LifeMax)    // Lifeが満タンの時
            //    {
            //        GlobalVariables.RestartTime = now.AddSeconds(-GlobalVariables.TimeToStart * 60 * GlobalVariables.LifeMax);
            //        GlobalVariables.Life = 3;
            //    }

            //    int newlife = 0;
            //    int remainTime = DateTime.Now.Subtract(GlobalVariables.RestartTime).Minutes;
            //    if (timeToStart * 60 * 1 <= ts.TotalSeconds)
            //    {
            //        newlife = 1;
            //        RestartButton.transform.GetChild(0).GetComponent<Text>().text = "再スタート";
            //    }
            //    if (timeToStart * 60 * 2 <= ts.TotalSeconds)
            //    {
            //        newlife = 2;
            //    }
            //    if (timeToStart * 60 * 3 <= ts.TotalSeconds)
            //    {
            //        newlife = 3;
            //        TimeToRestartText.SetActive(false);
            //    }
            //    string str = "ライフ復活まであと " + (timeToStart - 1 - ((int)ts.TotalMinutes % timeToStart)).ToString("00") + ":" + (60 - 1 - ((int)ts.TotalSeconds % 60)).ToString("00");
            //    TimeToRestartText.GetComponent<Text>().text = str;

            //    for (int i = 0; i < GlobalVariables.LifeMax; i++)
            //    {
            //        if (i < newlife)    // 現在の火花の数の1少ない数を表示
            //        {
            //            Remains[i].SetActive(true);
            //        }
            //        else
            //        {
            //            Remains[i].SetActive(false);
            //        }
            //    }

            //    if (newlife > GlobalVariables.Life)
            //    {
            //        PanelMovingParticle.transform.position = Remains[newlife - 1].transform.position;
            //        PanelMovingParticle.SetActive(false);
            //        PanelMovingParticle.SetActive(true);
            //    }

            //    GlobalVariables.Life = newlife;
            //    PlayerPrefs.SetInt("LIFE", GlobalVariables.Life);
            //}
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
            RestartButton.transform.GetChild(0).GetComponent<Text>().text = "再スタート";
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
        string str = "ライフ復活まであと " + (timeToStart - 1 - ((int)ts.TotalMinutes % timeToStart)).ToString("00") + ":" + (60 - 1 - ((int)ts.TotalSeconds % 60)).ToString("00");
        TimeToRestartText.GetComponent<Text>().text = str;

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
        if (GlobalVariables.Life == 0)
        {
            return;
            GlobalVariables.Life = 1; // 動画を見てゲームする
        }
        PlayerPrefs.SetInt("CURRENT_STAGE", 0);     // クリアしたステージから
        SceneManager.LoadScene("OpeningAnimation");
    }

    /// <summary>
    /// 
    /// </summary>
    void GoSelectStage()
    {
        if (GlobalVariables.Life == 0)
        {
            return;
            GlobalVariables.Life = 1; // 動画を見てゲームする
        }
        SceneManager.LoadScene("StageSelect");
    }
    /// <summary>
    /// 
    /// </summary>
    void GoRanking()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    void GoShare()
    {
        PlayerPrefs.DeleteAll();    // 最初から 
        GlobalVariables.RestartTime = GlobalVariables.RestartTime.AddMinutes(-1);   //ライフ1つ増やす

    }
}


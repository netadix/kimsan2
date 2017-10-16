using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSSA;
using System.Linq;
using System;

/// <summary>
/// 
/// </summary>
public class ScoreList {
    public int Stage;
    public int Score;
    public string Name;
}

/// <summary>
/// 
/// </summary>
public class Test : MonoBehaviour {
    public bool HighScoreEntryFlag { set; get; }
    public int YourRanking { get; set; }
    private bool getTextFromServerFlag;
    private bool setTextToServerFlag;

    private string serializedTextFromServer;
    private string serializedTextToServer;
    private bool completeGetTextFromServer;
    private bool completeSetTextToServer;
    private ScoreList [] scoreList;
    private int myScore;
    private int myStage;
    private bool exceptionOccurredFlag;

    const int MaxListLength = 50;

    /// <summary>
    /// スコアリストを取得する準備
    /// 
    /// </summary>
    /// <param name="score">自分のスコア</param>
    public void GetScorePreparation(int score, int stage)
    {
        getTextFromServerFlag = true;
        myScore = score;
        myStage = stage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scorelist">取得したスコアを格納するリスト</param>
    /// <returns></returns>
    public bool GetScore(out ScoreList [] scorelist)
    {
      
        if (getTextFromServerFlag)
        {
            scorelist = null;

            return false;       // 現在スコアをサーバーから取得中
        }
        else
        {
            scorelist = scoreList;

            return true;
        }
    }

    /// <summary>
    /// スコアリストをサーバーに保存する
    /// </summary>
    public void SetScore()
    {   
        setTextToServerFlag = true;
    }


    /// <summary>
    /// スコアリストをサーバーに保存した？
    /// ret : true 保存した
    /// </summary>
    public bool IsSetDone()
    {
        return completeSetTextToServer;
    }


    /// <summary>
    ///   Stage, Score, Name が1～50まで入っている
    /// 1 50,208790,name1<CR>
    /// 2 42,205600,name2<CR>
    /// 3 31,200120,name3<CR>
    ///     :
    ///     :
    /// 49 12,4500,name49<CR>
    /// 50 8,2400,name50<CR>
    ///    
    /// </summary>
    void GetScoreList(string str)
    {
        char [] delimiterChars = { ',', '\n' };
        int count = 0;
        int index;

        scoreList = new ScoreList[MaxListLength];  // リストの最後が""　Nullで配列サイズが151あるため＋１

        string[] stArrayData = str.Split(delimiterChars);

        foreach (string x in stArrayData)
        {
            if (count >= MaxListLength * 3)
            {
                break;
            }

            index = count / 3;

            switch (count % 3)
            {
                case 0:
                    scoreList[index] = new ScoreList();
                    scoreList[index].Stage = Int32.Parse(x);
                    break;

                case 1:
                    scoreList[index].Score = Int32.Parse(x);
                    break;

                case 2:
                    scoreList[index].Name = x;
                    break;

                default:
                    break;
            }
            count++;
        }

    }

    /// <summary>
    /// これは時間かかるし、ランキング表示を抜ける時に呼ぶ方がいいかも
    /// </summary>
    /// <param name="list"></param>
    void SetScoreList(ScoreList [] score)
    {
        int index;
        string str = "";

        for (int i = 0; i < MaxListLength * 3; i++)
        {
            index = i / 3;

            switch (i % 3)
            {
                case 0:
                    str += (score[index].Stage).ToString() + ",";
                    break;

                case 1:
                    str += (score[index].Score).ToString() + ",";
                    break;

                case 2:
                    str += score[index].Name + "\n";
                    break;

                default:
                    break;
            }
        }
        SetTextToServer(str);
    }

    /// <summary>
    /// 
    /// </summary>
    public void InitializeScore()
    {
        string str = "";

        for (int i = 0; i < MaxListLength; i++)
        {
            str += (i + 1).ToString() + "," + ((MaxListLength - i) * 100).ToString() + ",noname" + (i + 1).ToString() + "\n";
        }
        SetTextToServer(str);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="score"></param>
    public void RankMyScore(int score, int stage)
    {
        int i, j;
        int temporaryscore = MaxListLength - 1;
        string yourname = PlayerPrefs.GetString("YOUR_NAME", "YOURNAME");

        for (i = 0; i < MaxListLength; i++)
        {
            if ((scoreList[i].Name == yourname) || (scoreList[i].Name == "YOURNAME"))       // 既に自分のスコアがランクインしていたら消すように
            {
                temporaryscore = i;
                break;
            }
        }

        for (i = 0; i < MaxListLength; i++)
        {
            if (scoreList[i].Score <= score)
            {

                for (j = temporaryscore - 1; j >= i; j--)
                {
                    scoreList[j + 1].Score = scoreList[j].Score;
                    scoreList[j + 1].Name = scoreList[j].Name;
                    scoreList[j + 1].Stage = scoreList[j].Stage;
                }
                scoreList[i].Score = score;
                scoreList[i].Name = yourname;
                scoreList[i].Stage = stage;
                YourRanking = i + 1;

                if (yourname == "YOURNAME")
                {
                    HighScoreEntryFlag = true;
                    //　名前の入力がなく新規エントリーなのでここで名前の入力をする
                }
                break;
            }
        }
    }

    // Use this for initialization
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {

        // https://docs.google.com/spreadsheets/d/1RL7BwH5R4wQvxyqt6By2OmrVzPl8M3T-TBMtXfn-m4M/edit#gid=1808913702
        // googleのスプレッドシート netadixxxxx@google.com のスプレッドシートの「Kimsan」というタブシートを使用する

        // SetTextToServer("alsdkfjalskdjfoaiejowiajeoaijeflajljfasdf");

        completeGetTextFromServer = false;
        completeSetTextToServer = false;

        getTextFromServerFlag = false;
        setTextToServerFlag = false;
        myScore = 0;

        exceptionOccurredFlag = false;

        HighScoreEntryFlag = false;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        string str;

        if (exceptionOccurredFlag)
        {
            return;
        }

        if (completeSetTextToServer)
        {
            setTextToServerFlag = false;  // 保存出来たのでフラグを落とす
        }

        try
        {
            if ((getTextFromServerFlag) && (setTextToServerFlag == false))    // このフラグを立てて読み込む
            {
                GetTextFromServer();
                getTextFromServerFlag = false;
            }

            bool completeflag = NotifyGetTextFromServer(out str);
            if (completeflag)   // サーバーからデータが取得できた
            {
                GetScoreList(str);
                RankMyScore(myScore, myStage);
                //getTextFromServerFlag = false;
            }

            if ((getTextFromServerFlag == false) && (setTextToServerFlag))
            {
                setTextToServerFlag = false;
                SetScoreList(scoreList);
            }

        }
        catch(Exception e)
        {
            exceptionOccurredFlag = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    private void SetTextToServer(string str)
    {
        serializedTextToServer = str;
        StartCoroutine(ChatLogSetIterator());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    public void GetTextFromServer()
    {
        StartCoroutine(ChatLogGetIterator());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool NotifyGetTextFromServer(out string str)
    {
        bool temp;

        str = serializedTextFromServer;

        temp = completeGetTextFromServer;

        if (completeGetTextFromServer)
        {
            completeGetTextFromServer = false;  // 1回読んだのでフラグを落とす
        }

        return temp;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChatLogGetIterator()
    {
        SpreadSheetQuery query;
        try
        {
            query = new SpreadSheetQuery("Kimsan");
        }
        catch (Exception e)
        {
            exceptionOccurredFlag = true;

            yield break;
        }

        query.Limit(1);     // 1つだけ取得

        yield return query.FindAsync();

        foreach (var so in query.Result)
        {
            serializedTextFromServer = so["name"].ToString();
        }

        completeGetTextFromServer = true;

        yield break;
        }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChatLogSetIterator()
    {
        SpreadSheetQuery query;
        try
        {
            query = new SpreadSheetQuery("Kimsan");
        }
        catch (Exception e)
        {
            exceptionOccurredFlag = true;
            completeSetTextToServer = true;

            Debug.Log(e);

            yield break;
        }

        query.Where("rank", "=", "1");
        yield return query.FindAsync();

        var so = query.Result.FirstOrDefault();
        if (so != null)
        {
            so["name"] = serializedTextToServer;
            yield return so.SaveAsync();
        }
        completeSetTextToServer = true;

        yield break;
    }
}



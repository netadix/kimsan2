//#define DRAG_CONTROL
#define PUSH_CONTROL


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

// 0 Nothing (Empty)
// 1 Straight Vertical
// 2 Straight Horizontal
// 3 Curve Left to Up
// 4 Curve Up to Right
// 5 Curve Right to Down
// 6 Curve Down to Left
// 7 Curve (double) Left to Up / Right to Down
// 8 Curve (double) Down to Left / Up to Right
public enum PanelType
{
    Nothing = 0,                                // 何もない（唯一のパネル)
    StraightVertical = 1,
    StraightHorizontal,
    CurveLeftUp,
    CurveUpRight,
    CurveRightDown,
    CurveDownLeft,
    CurveLeftUp_RightDown,
    CurveDownLeft_UpRight,
    CurveLeftUp_RightDown_LeftUpOnlyEnabled,    // 一回右下通ったので左上のみ通れる
    CurveLeftUp_RightDown_RightDownOnlyEnabled, // 一回左上通ったので右下のみ通れる
    CurveDownLeft_UpRight_DownLeftOnlyEnabled,  // 一回右上通ったので左下のみ通れる
    CurveDownLeft_UpRight_UpRightOnlyEnabled,   // 一回左下通ったので右上のみ通れる
    Disabled,                                   // もはや一回通ったので通れないor最初から通れない
    Fixed                                       // パネル固定（動かせない）
}

public enum FuseDirection
{
    Up = 0,
    Right,
    Down,
    Left
}

public enum MoveDirection
{
    None = 0,
    Right = 1,
    Up,
    Left,
    Down
}

public enum GameStatus
{
    Start = 0,
    PreStart,
    Title,
    Play,
    ChallengeTime,
    GameOver,
    GameOver2,
    Ending,
    Cleared,
    Cleared1,
    ClearedNormalStage,
    ClearedRocket,
    ClearedRocket1,
    GameEnd,
    GameEnd2,
    Restart


}


//--------------------------------------------------------------------------
// [x, y]  x = 4, y = 4の場合
// [y]
// ↑
// 3
// 2
// 1
// 0
// 0 1 2 3 →[x]

public class PanelMatrix
{
    public long Size;
    public PanelType[,] Matrix;
    public GameObject[,] PanelObject;

    public PanelMatrix(long size)
    {
        Size = size;
        Initialize();
    }

    void Initialize()
    {
        Matrix = new PanelType[Size, Size];
        PanelObject = new GameObject[Size, Size];
    }

}

public class StageParameter
{
    public int HiScore;
    public int ClearLevel;      // Normal : 0 / Excellent : 1
    public float RemainTime;
    public int Cleared;         // Cleared : 1 / Not Cleared : 0

    public StageParameter()
    {
        HiScore = 0;
        ClearLevel = 0;
        RemainTime = 0f;
        Cleared = 0;
    }
}


/// <summary>
/// ///////////////////////////////////////////////////////////////////////
/// </summary>
public partial class PanelSystem : MonoBehaviour
{

    public GameObject NoPanel;
    public GameObject PlainPanel;
    public GameObject FixedPanel;
    public GameObject FireRope;
    public GameObject PanelSubSystem;
    public GameObject InformationText;
    public GameObject MainCamera;
    public GameObject FireCracker;
    public GameObject Explosion;
    public GameObject ExplosionM;
    public GameObject SpeedUpButton;
    public GameObject Missile;
    public GameObject StageClearParticle;
    public GameObject Remain;
    public GameObject TimeText;
    public Sprite BurntSprite;
    public GameObject StageText;
    public GameObject PanelMovingParticle;
    public GameObject RestartButton;
    public GameObject HomeButton;
    public GameObject TimeToRestartText;
    public GameObject ScoreDialogImage;
    public GameObject ScoreText;
    public GameObject NextArrowImage;
    public GameObject LifeText;
    public GameObject FadeOutPanel;
    public GameObject GameOverDialogImage;
    public GameObject BackGround;

    //public DateTime restartTime;   // ライフ復活までの時間

    //----------------------------------------------------
    private float panelWidth;
    private float panelHeight;
    private int panelSize;
    private PanelType[] stage;
    private GameObject[] Remains;

    private long currentX;
    private long currentY;

    private PanelMatrix Panel;                      // 現在の面のパネル
    private PanelMatrix NextPanel;                  // 次の面のパネル
    private GameObject[] stageToStageFireRope;      // 現在の面のロープ
    private GameObject[] nextStageToStageFireRope;  // 次の面のロープ
    private GameObject backGround;
    private GameObject nextBackGround;

    private MoveDirection PanelDirection;
    private float moveDistance;
    private GameObject MovingPanel;

    private long swapX1, swapX2;
    private long swapY1, swapY2;

    private Vector3 tapPosition;
    private int pushButtonCount;
    GameObject tapObject;

    private int tappedPanelX;
    private int tappedPanelY;

    private FuseDirection CurrentFusePanelDirection;

    //----------------------------------------------------
    private int stageCount;
    private bool okFlag;

    private GameStatus gameStatus;
    private int generalCounter;

    private GameObject explosionPrefab;         // 爆発のスタンダートアセットがプレハブになっているため毎回インスタンス化
    private float timeRemain;
    private bool timeTextAnimationFlag;
    private const int rocketStagePeriod = 5;    // ロケットを飛ばすステージ間隔
    private bool restartFlag;       // 再スタートボタンが押された？
    private int steps;              // 移動したパネルの数

    private int score;              // 現在のスコア
    private int excellentScore;     // 各ステージのEXCELLENTの条件スコアしきい値を格納
    private StageParameter[] stageParameter;
    private int hiScore;            // 過去の全体のハイスコア
    private int clearedStage;       // クリアしたステージ数（これ以下のステージはクリアしている)

    private bool stageRopeBurntFlag;
    private int oneSecond;

    //---------------------------------------------------------------------------------------
    partial void StageData(int stageNum);


    //---------------------------------------------------------------------------------------
    // set your itunes app id like 00000000
    private const string itunesAppId = "375380948";  // 評価してもらうためにAppStoreのID
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    GameObject CreateDoublePanel(PanelType type)
    {
        GameObject obj;
        GameObject child;
        float x, y, r, x1, y1;
        Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
        Vector2 pos;
        float theta;

        obj = Instantiate(PlainPanel, transform.position, transform.rotation) as GameObject;

        r = panelWidth / 2f;
        x = 0;

        for (int i = 0; i < 16; i++)
        {
            // theta = Mathf.PI / 2f / 16f * i;
            theta = Mathf.PI / 2f / 17f * i + Mathf.PI / 2f / 20f;
            y = Mathf.Sin(theta) * r;
            x = Mathf.Cos(theta) * r;
            x1 = x - panelWidth / 2f;
            y1 = y - panelHeight / 2f;
            pos = new Vector2(x1, y1);   // Down to Left Curve
            child = Instantiate(FireRope, pos, transform.rotation) as GameObject;
            child.transform.parent = obj.transform;
            child.transform.position = pos;
            child.transform.name = "Dot" + i;
        }

        for (int i = 0; i < 16; i++)
        {
            // theta = Mathf.PI / 2f / 16f * i + Mathf.PI / 2f / 16f / 2f;
            theta = Mathf.PI / 2f / 17f * i + Mathf.PI / 2f / 20f;
            y = Mathf.Sin(theta) * r;
            x = Mathf.Cos(theta) * r;
            x1 = x - panelWidth / 2f;
            y1 = y - panelHeight / 2f;
            x1 = -x1;
            y1 = -y1;
            pos = new Vector2(x1, y1);   // Up to Right Curve
            child = Instantiate(FireRope, pos, transform.rotation) as GameObject;
            child.transform.parent = obj.transform;
            child.transform.position = pos;
            child.transform.name = "Dotx" + i;
        }

        if (type == PanelType.CurveDownLeft_UpRight)
        {
            rot = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (type == PanelType.CurveLeftUp_RightDown)
        {
            rot = Quaternion.Euler(0f, 0f, 90f);
        }
        obj.transform.rotation = rot;

        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    GameObject CreateCurvePanel(PanelType type)
    {
        GameObject obj;
        GameObject child;
        float x, y, r, x1, y1;
        Quaternion rot = Quaternion.Euler(0f, 0f, 0f);

        obj = Instantiate(PlainPanel, transform.position, transform.rotation) as GameObject;

        r = panelWidth / 2f;

        for (int i = 0; i < 16; i++)
        {
            //float theta = Mathf.PI / 2f / 16f * i + Mathf.PI / 2f / 16f / 2f;
            float theta = Mathf.PI / 2f / 17f * i + Mathf.PI / 2f / 20f;
            y = Mathf.Sin(theta) * r;
            x = Mathf.Cos(theta) * r;

            //y = Mathf.Sqrt(-(x * x) + (r * r));
            //x += panelWidth / 2f / 16f;
            x1 = x - panelWidth / 2f;
            y1 = y - panelHeight / 2f;
            Vector2 pos = new Vector2(x1, y1);   // Left to Down Curve
            child = Instantiate(FireRope, pos, transform.rotation) as GameObject;
            child.transform.parent = obj.transform;
            child.transform.position = pos;
            child.transform.name = "Dot" + i;
        }

        if (type == PanelType.CurveDownLeft)
        {
            rot = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (type == PanelType.CurveLeftUp)
        {
            rot = Quaternion.Euler(0f, 0f, 270f);
        }
        else if (type == PanelType.CurveRightDown)
        {
            rot = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (type == PanelType.CurveUpRight)
        {
            rot = Quaternion.Euler(0f, 0f, 180f);
        }
        obj.transform.rotation = rot;

        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    GameObject CreateLinePanel(PanelType type)
    {
        GameObject obj;
        GameObject child;
        Quaternion rot = Quaternion.Euler(0f, 0f, 0f);

        obj = Instantiate(PlainPanel, transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        for (int i = 0; i < 16; i++)
        {
            //Vector2 pos = new Vector2((float)i * panelWidth / 16f - panelWidth / 2f + panelWidth / 32f, 0f);   // Horizontal Line
            Vector2 pos = new Vector2((float)i * panelWidth / 17f - panelWidth / 2f + panelWidth / 20f, 0f);   // Horizontal Line
            child = Instantiate(FireRope, pos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            child.transform.position = pos;
            child.transform.parent = obj.transform;
            child.transform.name = "Dot" + i;

        }
        if (type == PanelType.StraightHorizontal)
        {
            rot = Quaternion.Euler(0f, 0f, 0f);
        }
        else// Vertical
        {
            rot = Quaternion.Euler(0f, 0f, 90f);
        }
        obj.transform.rotation = rot;

        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    GameObject CreatePlainPanel()
    {
        GameObject obj;

        obj = Instantiate(PlainPanel, transform.position, transform.rotation) as GameObject;

        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    GameObject CreateFixedPanel()
    {
        GameObject obj;

        obj = Instantiate(FixedPanel, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<SpriteRenderer>().color = new Color(0.32f, 0.28f, 0.28f);  // 動かせないパネル

        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    GameObject CreateNoPanel()
    {
        GameObject obj;

        obj = Instantiate(NoPanel, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<SpriteRenderer>().enabled = false;

        return obj;
    }

    /// <summary>
    /// /////////////////////////////////////////////////////////
    // 左下を原点として上方向(Y方向)に指定の大きさ(panelSize)のパネルを並べていく
    /// </summary>
    /// <param name="stageNum"></param>
    void CreateStage(out PanelMatrix retpanel, bool currentFlag = true)
    {
        PanelMatrix panel = new PanelMatrix(panelSize);
        float offset = panelWidth / 2f;

        retpanel = panel;

        int count = 0;
        for (int i = 0; i < panel.Size; i++)
        {
            for (int j = 0; j < panel.Size; j++)
            {
                panel.Matrix[i, j] = stage[count++];

                PanelType panelType = panel.Matrix[i, j];
                switch (panelType)
                {
                    case PanelType.StraightVertical:
                        panel.PanelObject[i, j] = CreateLinePanel(panelType);
                        break;

                    case PanelType.StraightHorizontal:
                        panel.PanelObject[i, j] = CreateLinePanel(panelType);
                        break;

                    case PanelType.CurveLeftUp:
                    case PanelType.CurveUpRight:
                    case PanelType.CurveRightDown:
                    case PanelType.CurveDownLeft:
                        panel.PanelObject[i, j] = CreateCurvePanel(panelType);
                        break;

                    case PanelType.CurveLeftUp_RightDown:
                    case PanelType.CurveDownLeft_UpRight:
                        panel.PanelObject[i, j] = CreateDoublePanel(panelType);
                        break;

                    case PanelType.Nothing:
                        panel.PanelObject[i, j] = CreateNoPanel();
                        break;

                    case PanelType.Disabled:
                        panel.PanelObject[i, j] = CreatePlainPanel();
                        break;

                    case PanelType.Fixed:
                        panel.PanelObject[i, j] = CreateFixedPanel();
                        break;
                }

                if (currentFlag)
                {
                    panel.PanelObject[i, j].transform.position = new Vector2(i * panelWidth, j * panelHeight);
                }       
                else
                {
                    panel.PanelObject[i, j].transform.position = new Vector2(stageToStageFireRope[stageToStageFireRope.Length - 1].transform.position.x + i * panelWidth + offset, stageToStageFireRope[stageToStageFireRope.Length - 1].transform.position.y - ((panelSize - 1) * panelHeight) + j * panelHeight);
                }
            }
        }
        if (currentFlag)
        {
            Vector2 pos = new Vector2(panel.PanelObject[0, 0].transform.position.x + ((panelWidth * panel.Size)) / 2, panel.PanelObject[0, 0].transform.position.y + ((panelHeight * panel.Size) / 2));
            backGround = Instantiate(BackGround, pos, transform.rotation) as GameObject;
           // backGround.transform.position = new Vector2(panel.PanelObject[0, 0].transform.position.x + ((panelWidth * panel.Size)) / 2, panel.PanelObject[0, 0].transform.position.y + ((panelHeight * panel.Size) / 2));
        }
        else
        {
            Vector2 pos = new Vector2(panel.PanelObject[0, 0].transform.position.x + ((panelWidth * panel.Size)) / 2, panel.PanelObject[0, 0].transform.position.y + ((panelHeight * panel.Size) / 2));
            nextBackGround = Instantiate(BackGround, pos, transform.rotation) as GameObject;
            //nextBackGround.transform.position = new Vector2(panel.PanelObject[0, 0].transform.position.x + ((panelWidth * panel.Size)) / 2, panel.PanelObject[0, 0].transform.position.y + ((panelHeight * panel.Size) / 2));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="retpanel"></param>
    /// <param name="currentFlag"></param>
    void CreateRandomStage(out PanelMatrix retpanel, bool currentFlag = true)
    {
        PanelMatrix panel = new PanelMatrix(panelSize);
        float offset = panelWidth / 2f;

        retpanel = panel;

        int count = 0;
        for (int i = 0; i < panel.Size; i++)
        {
            for (int j = 0; j < panel.Size; j++)
            {
                panel.Matrix[i, j] = stage[count++];

                PanelType panelType = (PanelType)((int)(UnityEngine.Random.value * 8f) + 1);
                switch (panelType)
                {
                    case PanelType.StraightVertical:
                        panel.PanelObject[i, j] = CreateLinePanel(panelType);
                        break;

                    case PanelType.StraightHorizontal:
                        panel.PanelObject[i, j] = CreateLinePanel(panelType);
                        break;

                    case PanelType.CurveLeftUp:
                    case PanelType.CurveUpRight:
                    case PanelType.CurveRightDown:
                    case PanelType.CurveDownLeft:
                        panel.PanelObject[i, j] = CreateCurvePanel(panelType);
                        break;

                    case PanelType.CurveLeftUp_RightDown:
                    case PanelType.CurveDownLeft_UpRight:
                        panel.PanelObject[i, j] = CreateDoublePanel(panelType);
                        break;

                    case PanelType.Nothing:
                        panel.PanelObject[i, j] = CreateNoPanel();
                        break;

                    case PanelType.Disabled:
                        panel.PanelObject[i, j] = CreatePlainPanel();
                        break;

                    case PanelType.Fixed:
                        panel.PanelObject[i, j] = CreateFixedPanel();
                        break;
                }

                if (currentFlag)
                {
                    panel.PanelObject[i, j].transform.position = new Vector2(i * panelWidth, j * panelHeight);
                }
                else
                {
                    panel.PanelObject[i, j].transform.position = new Vector2(stageToStageFireRope[stageToStageFireRope.Length - 1].transform.position.x + i * panelWidth + offset, stageToStageFireRope[stageToStageFireRope.Length - 1].transform.position.y - ((panelSize - 1) * panelHeight) + j * panelHeight);
                }
            }
        }
    }

    enum RopeForStage
    {
        NextStage = 0,
        NextNextStage,
        Rocket,
        NextStageRocket,
    }
    /// <summary>
    /// ///////////////////////////////////////////
    /// </summary>
    void CreateStageToStageRope(out GameObject[] obj, RopeForStage stg, int nextPanelSize = 0)
    {
        Vector2 pos = Vector3.zero;
        float offset = 0; // panelWidth / 2;      // TBD
        int fireRopeCount1st = (16 * 1) + 16 * (6 - panelSize);
        int fireRopeCount1stRound = 16;
        int fireRopeCountVertical = 16 * (nextPanelSize - 2);
        int fireRopeCount2ndRound = 16;
        int fireRopeCount2nd = 16 * 2;
        int fireRopeToRocket = 16 * 5 + 16 * (6 - panelSize);

        int count = 0;
        float theta;
        float x, y;
        float r = panelWidth / 2f;
        int totalRope;

        if (stg == RopeForStage.NextStage)
        {
            pos.y = Panel.PanelObject[Panel.Size - 1, 0].transform.position.y;
            pos.x = Panel.PanelObject[Panel.Size - 1, 0].transform.position.x + panelWidth / 2 + offset;
        }
        else if (stg == RopeForStage.NextNextStage)
        {
            pos.y = NextPanel.PanelObject[NextPanel.Size - 1, 0].transform.position.y;
            pos.x = NextPanel.PanelObject[NextPanel.Size - 1, 0].transform.position.x + panelWidth / 2 + offset;
        }

        if ((stg == RopeForStage.Rocket) || (stg == RopeForStage.NextStageRocket))        // ロケット用のロープ
        {
            totalRope = fireRopeToRocket;

            obj = new GameObject[totalRope];

            if (stg == RopeForStage.NextStageRocket)
            {
                pos.y = NextPanel.PanelObject[Panel.Size - 1, 0].transform.position.y;
                pos.x = NextPanel.PanelObject[Panel.Size - 1, 0].transform.position.x + panelWidth / 2 + offset;
            }
            else
            {
                pos.y = Panel.PanelObject[Panel.Size - 1, 0].transform.position.y;
                pos.x = Panel.PanelObject[Panel.Size - 1, 0].transform.position.x + panelWidth / 2 + offset;
            }

            for (int i = 0; i < totalRope; i++)
            {
                obj[count] = Instantiate(FireRope, pos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                pos.x += panelWidth / 16f;
                pos.y += 0;
                count++;
            }

            return;
        }

        totalRope = fireRopeCount1st
                + fireRopeCount1stRound
                + fireRopeCountVertical
                + fireRopeCount2ndRound
                + fireRopeCount2nd;

        obj = new GameObject[totalRope];

        for (int i = 0; i < fireRopeCount1st; i++)
        {
            obj[count] = Instantiate(FireRope, pos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            pos.x += panelWidth / 16f;
            pos.y += 0;
            count++;
        }


        pos.y += panelWidth / 2;
        pos.x += 0;

        for (int i = 0; i < fireRopeCount1stRound; i++)
        {
            theta = (Mathf.PI * 1.5f) + Mathf.PI / 2f / 16f * i;
            y = Mathf.Sin(theta) * r;
            x = Mathf.Cos(theta) * r;

            Vector2 pos1;
            pos1.x = pos.x + x;
            pos1.y = pos.y + y;

            obj[count] = Instantiate(FireRope, pos1, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            count++;
        }

        pos.y += 0;
        pos.x += panelWidth / 2;

        for (int i = 0; i < fireRopeCountVertical; i++)
        {
            obj[count] = Instantiate(FireRope, pos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            pos.x += 0;
            pos.y += panelWidth / 16f;
            count++;
        }

        pos.y += 0;
        pos.x += panelWidth / 2;

        for (int i = 0; i < fireRopeCount2ndRound; i++)
        {
            theta = (Mathf.PI / 2) + Mathf.PI / 2f / 16f * (fireRopeCount1stRound - i);
            y = Mathf.Sin(theta) * r;
            x = Mathf.Cos(theta) * r;

            Vector2 pos1;
            pos1.x = pos.x + x;
            pos1.y = pos.y + y;

            obj[count] = Instantiate(FireRope, pos1, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            count++;
        }

        pos.y += panelHeight / 2; ;
        pos.x += 0;

        for (int i = 0; i < fireRopeCount2nd; i++)
        {
            obj[count] = Instantiate(FireRope, pos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            pos.x += panelWidth / 16f;
            pos.y += 0;
            count++;
        }

    }

    /// <summary>
    /// ///////////
    /// </summary>
    void Start()
    {
        stageParameter = new StageParameter[(int)Stage.S_MAX];

        okFlag = false;

        hiScore = 0;
        clearedStage = 0;
        for (int i = (int)Stage.S_1; i < (int)Stage.S_MAX; i++)
        {
            stageParameter[i] = new StageParameter();
            stageParameter[i].HiScore = PlayerPrefs.GetInt("HI_SCORE" + i, 0);
            stageParameter[i].ClearLevel = PlayerPrefs.GetInt("CLEAR_LEVEL" + i, 0);
            stageParameter[i].Cleared = PlayerPrefs.GetInt("CLEARED" + i, 0);
            stageParameter[i].RemainTime = PlayerPrefs.GetFloat("REMAIN_TIME" + i, 0f);
            hiScore += stageParameter[i].HiScore;
            score = hiScore;

            if (stageParameter[i].Cleared == (int)1)
            {
                clearedStage = i;
            }

        }
        DisplayScoreText(score);

        PanelDirection = MoveDirection.None;
        panelWidth = PlainPanel.GetComponent<SpriteRenderer>().bounds.size.x;  // 0.64 = 64 pixel
        panelHeight = PlainPanel.GetComponent<SpriteRenderer>().bounds.size.y;

        pushButtonCount = 0;
        int mode = PlayerPrefs.GetInt("CURRENT_STAGE", -1);
        if (mode <= 0)
        {
            stageCount = clearedStage + 1; // クリアした直後のステージから（ここに来ることはない？）
        }
        else
        {
            stageCount = mode;      // Stage select
        }

        gameStatus = GameStatus.PreStart;
        generalCounter = 0;
        InformationText.GetComponent<Text>().text = "";
        DisplayStageText(stageCount);

        //backGround = Instantiate(BackGround, transform.position, transform.rotation) as GameObject;

        StageData(stageCount);
        CreateStage(out Panel);
        StageData(stageCount + 1);        // 次のステージのデータ
        CreateStageToStageRope(out stageToStageFireRope, RopeForStage.NextStage, panelSize);
        StageData(stageCount);           // ステージをセットし直す

        if ((stageCount % rocketStagePeriod) == 0)  // ロケット発射ステージ （ステージセレクトでここに来た）
        {
            CreateStageToStageRope(out nextStageToStageFireRope, RopeForStage.Rocket, 0);    // ロケットへのロープ
            foreach (GameObject obj in stageToStageFireRope)
            {
                Destroy(obj);
            }
            stageToStageFireRope = nextStageToStageFireRope;
            NextPanel = Panel;
        }

        PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], FuseDirection.Left, Panel.Matrix[currentX, currentY]);

        Button btn = SpeedUpButton.GetComponent<Button>();
        btn.onClick.AddListener(SpeedUp);

        restartFlag = false;

        Remains = new GameObject[GlobalVariables.LifeMax];

        for (int i = 0; i < GlobalVariables.LifeMax; i++)
        {
            Vector3 vec = LifeText.transform.position;

            Remains[i] = Instantiate(Remain, vec + new Vector3(0.4f + i * 0.4f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f)) as GameObject;

            Remains[i].SetActive(false);
        }

        //for (int i = 0; i < GlobalVariables.Life; i++)
        //{
        //    Remains[i].SetActive(true);
        //}

        timeTextAnimationFlag = false;

        steps = 0;

        stageRopeBurntFlag = false;

        GameOverDialogImage.SetActive(false);
        ScoreDialogImage.SetActive(false);
        TimeToRestartText.SetActive(false);
        StageClearParticle.SetActive(false);
        PanelMovingParticle.SetActive(false);
        InformationText.SetActive(true);

        NextArrowImage.SetActive(true);
        NextArrowImage.transform.position = Panel.PanelObject[panelSize - 1, 0].transform.position + new Vector3(panelWidth * 1.25f, panelHeight * 0.5f, 0f);
        NextArrowImage.GetComponent<Animation>().Play();

        StartPanelProcedure();

        oneSecond = 59;

    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="stage"></param>
    void GotoStage(int stage)
    {
        PanelSubSystem.GetComponent<PanelObject>().RemoveFireCrackerFromParent();

        for (int i = 0; i < Panel.Size; i++)
        {
            for (int j = 0; j < Panel.Size; j++)
            {
                Destroy(Panel.PanelObject[i, j]);

            }
        }
        Destroy(backGround);

        for (int k = 0; k < stageToStageFireRope.Length; k++)
        {
            Destroy(stageToStageFireRope[k]);
        }

        Panel = NextPanel;      // 現在のステージと次のステージを入れ替え
        stageToStageFireRope = nextStageToStageFireRope;
        backGround = nextBackGround;
        
        stageCount = stage;
        StageData(stageCount);

    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="stage"></param>
    void StartStage()
    {
        PanelSubSystem.GetComponent<PanelObject>().RemoveFireCrackerFromParent();

        for (int i = 0; i < Panel.Size; i++)
        {
            for (int j = 0; j < Panel.Size; j++)
            {
                Destroy(Panel.PanelObject[i, j]);

            }
        }

        Destroy(backGround);

        for (int k = 0; k < stageToStageFireRope.Length; k++)
        {
            Destroy(stageToStageFireRope[k]);
        }

        StageData(stageCount);
        CreateStage(out Panel);

        if ((stageCount % rocketStagePeriod) == 0)  // ロケット発射
        {
            CreateStageToStageRope(out stageToStageFireRope, RopeForStage.Rocket, 0);    // ロケットへのロープ
            nextStageToStageFireRope = stageToStageFireRope;
            NextPanel = Panel;
        }
        else
        {
            StageData(stageCount + 1);        // 次のステージのデータ
            CreateStageToStageRope(out stageToStageFireRope, RopeForStage.NextStage, panelSize);
            StageData(stageCount);           // ステージをセットし直す
        }
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //-------------------------------------------------------------------------
        // Lifeの処理
        //-------------------------------------------------------------------------
        if ((++oneSecond % 6) == 0)
        {
            oneSecond = 0;

            LifeControl();
        }

        //-------------------------------------------------------------------------
        // 状態遷移
        //-------------------------------------------------------------------------
        switch (gameStatus)
        {
            //-------------------------------------------------------------------------
            case GameStatus.Cleared:
                if (okFlag)
                {
                    okFlag = false;

                    InformationText.GetComponent<Text>().text = "";

                    CalculateScore();
                    SaveGameData();

                    DisplayScoreText(score);
                    ScoreDialogImage.SetActive(false);

                    gameStatus = GameStatus.Cleared1;
                    StartCoroutine("AnimationStageRopeBurning");
                }
                break;

            case GameStatus.Cleared1:
                generalCounter++;
                if (generalCounter >= 1)
                {
                    generalCounter = 0;
                    if (stageCount % rocketStagePeriod == 0)        // 5ステージ毎にロケット発射
                    {
                        gameStatus = GameStatus.ClearedRocket;
                    }
                    else
                    {
                        gameStatus = GameStatus.ClearedNormalStage;
                        DisplayTimeText(timeRemain);
                        DisplayStageText(stageCount);
                    }

                }
                break;

            case GameStatus.ClearedRocket:
                generalCounter++;
                //if (generalCounter == 100)
                if (stageRopeBurntFlag == true)
                {
                    stageRopeBurntFlag = false;
                    Missile.transform.Find("RocketFire").gameObject.SetActive(true);
                    FireCracker.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    FireCracker.SetActive(false);
                }
                if (generalCounter >= 200)
                {
                    generalCounter = 0;
                    gameStatus = GameStatus.ClearedRocket1;

                }
                break;

            case GameStatus.ClearedRocket1:
                generalCounter++;
                Missile.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, (float)generalCounter / 12, 0);
                if (generalCounter >= 150)
                {
                    //GotoStage(++stageCount);
                    stageCount++;
                    StartStage();

                    gameStatus = GameStatus.PreStart;
                    generalCounter = 0;
                    pushButtonCount = 0;
                    PanelSubSystem.GetComponent<PanelObject>().StartFlag = false;
                    StageClearParticle.SetActive(false);
                }
                break;

            case GameStatus.ClearedNormalStage:
                generalCounter++;
                //if (generalCounter >= 150)
                if (stageRopeBurntFlag == true)
                {
                    stageRopeBurntFlag = false;
                    GotoStage(++stageCount);
                    gameStatus = GameStatus.PreStart;
                    generalCounter = 0;
                    pushButtonCount = 0;
                    PanelSubSystem.GetComponent<PanelObject>().StartFlag = false;
                    StageClearParticle.SetActive(false);
                }
                break;

            case GameStatus.ChallengeTime:      // 火花が消えていくが完全に消えるまではまだ生きている
                const float deltaChallengeTime = 120;
                FireCracker.transform.localScale = new Vector3(0.2f - 0.2f / deltaChallengeTime * generalCounter, 0.2f - 0.2f / deltaChallengeTime * generalCounter, 1f);
                bool state = Playing();
                if (state)
                {
                    break;
                }
                generalCounter++;
                if (generalCounter >= deltaChallengeTime)
                {
                    StageEndCommonProcedure();
                    generalCounter = 0;
                    //GlobalVariables.Life--;

                    bool judge = CalculateLife();
                    if (judge)
                    {
                        gameStatus = GameStatus.GameEnd;
                    }
                    else
                    {
                        GameOver();
                        gameStatus = GameStatus.GameOver;
                    }

                    //if (GlobalVariables.Life - 1 <= 0)
                    //{
                    //    gameStatus = GameStatus.GameEnd;
                    //}
                    //else
                    //{
                    //    GameOver();
                    //    gameStatus = GameStatus.GameOver;
                    //}
                }
                break;

            case GameStatus.GameOver:
                const float deltaGameOverAnimationTime = 20;
                generalCounter++;
                if (generalCounter >= deltaGameOverAnimationTime)
                {
                    gameStatus = GameStatus.GameOver2;
                    generalCounter = 0;
                    GameOver2();
                }
                break;

            case GameStatus.GameOver2:
                generalCounter++;
                if (generalCounter >= 60)
                {
                    generalCounter = 0;
                    StartStage();
                    gameStatus = GameStatus.PreStart;
                    InformationText.GetComponent<Text>().text = "";
                    //MainCamera.GetComponent<Animator>().Play("CameraBlurOff");
                }
                break;

            case GameStatus.GameEnd:
                generalCounter++;
                if (generalCounter >= 60)
                {
                    GameEnd();
                    gameStatus = GameStatus.GameEnd2;
                    generalCounter = 0;

                    GameOverDialogImage.GetComponent<GameOverDialog>().StartDialogAnimation();
                    TimeToRestartText.SetActive(true);
                    //GameOverDialogImage.SetActive(true);
                    //RestartButton.SetActive(true);
                    //GlobalVariables.RestartTime = DateTime.Now;
                    RestartButton.transform.GetChild(0).GetComponent<Text>().text = TextResources.Text[(int)TextNumber.WATCH_MOVIE_TO_START, (int)GlobalVariables.Language]; // "動画を観てすぐに始める";
                    RestartButton.transform.GetChild(0).GetComponent<Text>().font.name = GlobalVariables.LanguageFont;
                }
                break;

            case GameStatus.GameEnd2:
                generalCounter++;

                if (restartFlag == true)    // 再スタートボタンが押された
                {

                    GlobalVariables.RestartTime = GlobalVariables.RestartTime.AddMinutes(-GlobalVariables.TimeToStart);   //ライフ1つ増やす(TBD)

                    restartFlag = false;

                    GameOverDialogImage.SetActive(false);
                    //RestartButton.SetActive(false);
                    TimeToRestartText.SetActive(false);

                    generalCounter = 0;
                    StartStage();
                    gameStatus = GameStatus.PreStart;
                    InformationText.GetComponent<Text>().text = "";
                }
                break;

            case GameStatus.PreStart:
                generalCounter++;

                FireCracker.SetActive(true);
                FireCracker.transform.localScale = new Vector3(0.2f, 0.2f, 1f); // これ効果なかった(アニメーターが何故か走っていて優先される
                PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], FuseDirection.Left, Panel.Matrix[currentX, currentY]);

                NextArrowImage.SetActive(true);
                NextArrowImage.transform.position = Panel.PanelObject[panelSize - 1, 0].transform.position + new Vector3(panelWidth * 1.25f, panelHeight * 0.5f, 0f);

                if (generalCounter == 1)
                {
                    //FireCracker.GetComponent<Animator>().SetBool("FireCrackerAppearFlag", true);
                    PanelSubSystem.GetComponent<PanelObject>().SpeedUpFlag = false; // スピードダウン
                    DisplayStageText(stageCount);
                    DisplayTimeText(timeRemain);

                    InformationText.GetComponent<Text>().text = "STAGE " + stageCount;
                    InformationText.GetComponent<Animation>().Stop();
                    InformationText.GetComponent<Animation>().Play("PopUpTextAnimation");
                }
                else if (generalCounter == (30 + 90))
                {
                    InformationText.GetComponent<Text>().text = "START";
                    InformationText.GetComponent<Animation>().Stop();
                    InformationText.GetComponent<Animation>().Play("PopUpTextAnimation");
                }
                else if (generalCounter >= (30 + 250))  // ここでユーザーにパネル構成を考えさせる時間を与える
                {
                    InformationText.GetComponent<Text>().text = "";

                    generalCounter = 0;
                    gameStatus = GameStatus.Start;
                }
                if (Input.GetMouseButtonDown(0))    // キャンセル可能
                {
                    generalCounter = 0;
                    gameStatus = GameStatus.Start;
                }
                break;

            case GameStatus.Start:
                generalCounter++;
                if (generalCounter >= 1)
                {
                    steps = 0;

                    //Remains[GlobalVariables.Life - 1].SetActive(false);
                    generalCounter = 0;
                    if (explosionPrefab != null)
                    {
                        Destroy(explosionPrefab);
                    }
                    //Animator anime = MainCamera.GetComponent<Animator>();
                    //anime.Play("GlowAnimation");
                    gameStatus = GameStatus.Play;
                    //FireCracker.GetComponent<Animator>().SetBool("FireCrackerAppearFlag", false);
                    Panel.PanelObject[currentX, currentY].GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.8f, 0.8f);  // 燃え中のパネル
                    pushButtonCount = 0;    // 強制的にプッシュをリリースする
                    PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], FuseDirection.Left, Panel.Matrix[currentX, currentY]);
                    PanelSubSystem.GetComponent<PanelObject>().StartFlag = true;

                    Missile.GetComponent<Rigidbody>().useGravity = false;
                    Missile.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    Missile.transform.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                    Missile.transform.position = new Vector3(105.1f, 1.2f, 0f);
                    Missile.transform.rotation = Quaternion.Euler(0, 0, 0);
                    GameObject.Find("Nodong").transform.Find("RocketFire").gameObject.SetActive(false);
                }

                StartPanelProcedure();
                break;

            case GameStatus.Play:
                generalCounter = 0;
                Playing();
                break;

            case GameStatus.Restart:
                break;

            default:
                // Nothing to do.
                break;
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
        string str = TextResources.Text[(int)TextNumber.TIME_TO_RESTART, (int)GlobalVariables.Language] + (timeToStart - 1 - ((int)ts.TotalMinutes % timeToStart)).ToString("00") + ":" + (60 - 1 - ((int)ts.TotalSeconds % 60)).ToString("00");
        TimeToRestartText.GetComponent<Text>().text = str;
        TimeToRestartText.GetComponent<Text>().font.name = GlobalVariables.LanguageFont;

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
    void SaveGameData()
    {
        // ゲームデータをセーブ
        PlayerPrefs.SetInt("HI_SCORE" + stageCount, stageParameter[stageCount].HiScore);
        PlayerPrefs.SetInt("CLEAR_LEVEL" + stageCount, stageParameter[stageCount].ClearLevel);
        PlayerPrefs.SetInt("CLEARED" + stageCount, stageParameter[stageCount].Cleared);
        PlayerPrefs.SetFloat("REMAIN_TIME" + stageCount, stageParameter[stageCount].RemainTime);
        PlayerPrefs.SetInt("CURRENT_STAGE", stageCount);
        PlayerPrefs.SetInt("LIFE", GlobalVariables.Life);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 
    /// </summary>
    void CalculateScore()
    {
        int currentScore = ScoreDialogImage.GetComponent<ScoreDialog>().score;

        if (stageParameter[stageCount].HiScore<currentScore)
        {
            stageParameter[stageCount].HiScore = currentScore;
        }

        stageParameter[stageCount].ClearLevel = ScoreDialogImage.GetComponent<ScoreDialog>().clearLevel;
        stageParameter[stageCount].Cleared = 1;
        stageParameter[stageCount].RemainTime = ScoreDialogImage.GetComponent<ScoreDialog>().remainTime;
        if (clearedStage < stageCount)
        {
            clearedStage = stageCount;
        }

        currentScore = 0;
        for (int i = (int) Stage.S_1; i <= clearedStage; i++)
        {
            currentScore += stageParameter[i].HiScore;
        }
        if (currentScore > hiScore)
        {
            hiScore = currentScore;
        }
        score = hiScore;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stage"></param>
    void DisplayStageText(int stage)
    {
        StageText.GetComponent<Text>().text = "STAGE " + stage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="score"></param>
    void DisplayScoreText(int score)
    {
        ScoreText.GetComponent<Text>().text = "SCORE\n" + score;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    void DisplayTimeText(float time)
    {
        TimeText.GetComponent<Text>().text = "TIME\n" + time.ToString("0.000");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool Playing()
    {
        Vector3 clickPosition;
        int i = 0;
        int j = 0;
        float delta = (panelHeight + panelWidth) / 2f / 4f;    // 1/60秒ごとのパネル移動距離
        MoveDirection tapDirection = MoveDirection.None;

        DisplayTimeText(timeRemain);

        timeRemain -= 1f / 60f;
        if (timeRemain <= 0f)
        {
            StageEndCommonProcedure();
            //GlobalVariables.Life--;
            bool judge = CalculateLife();
            if (judge)
            {
                gameStatus = GameStatus.GameEnd;
            }
            else
            {
                GameOver("TIME OUT");
                gameStatus = GameStatus.GameOver;
            }

            //if (GlobalVariables.Life <= 0)
            //{
            //    gameStatus = GameStatus.GameEnd;
            //}
            //else
            //{
            //    GameOver("TIME OUT");
            //    gameStatus = GameStatus.GameOver;
            //}
            timeRemain = 0f;
            DisplayTimeText(timeRemain);

            return true;
        }

        if ((timeRemain <= 5f) && (!timeTextAnimationFlag))   // タイマーが5以下になったらタイマーをハートビートさせる
        {
            TimeText.GetComponent<Animation>().Play();
            timeTextAnimationFlag = true;
        }

        if (Panel.PanelObject[currentX, currentY].name.Substring(0, 2) != "No")   // "NoPanel"
        {
            Panel.PanelObject[currentX, currentY].GetComponent<Animation>().Stop();
            Panel.PanelObject[currentX, currentY].GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.8f, 0.8f);  // 燃え中のパネル
        }

        if ((PanelSubSystem.GetComponent<PanelObject>().GetFuseBurningCompletion() == true) && (gameStatus == GameStatus.Play))
        {
            if (JudgeFuseConnection())
            {
                return true;    // ゲームオーバーかステージクリア
            }
        }

        if ((PanelDirection != MoveDirection.None) && (MovingPanel != null))    // パネル移動アニメーション中か？
        {
            moveDistance -= delta;
            if (moveDistance + delta > 0f)
            {
                Vector2 pos;

                switch (PanelDirection)
                {
                    case MoveDirection.Down:
                        pos = MovingPanel.transform.position;
                        pos.y -= delta;
                        MovingPanel.transform.position = pos;
                        break;

                    case MoveDirection.Up:
                        pos = MovingPanel.transform.position;
                        pos.y += delta;
                        MovingPanel.transform.position = pos;
                        break;

                    case MoveDirection.Right:
                        pos = MovingPanel.transform.position;
                        pos.x += delta;
                        MovingPanel.transform.position = pos;
                        break;

                    case MoveDirection.Left:
                        pos = MovingPanel.transform.position;
                        pos.x -= delta;
                        MovingPanel.transform.position = pos;
                        break;
                }
                PanelMovingParticle.transform.position = MovingPanel.transform.position;

                return false;
            }
            else
            {
                if (tapObject.name.Substring(0, 2) != "No" && MovingPanel.name.Substring(0, 5) == "Plain")   // "NoPanel"を掴んで移動したら無効    Y.Ida added 2017.10.08
                {
                    PanelMovingParticle.SetActive(false);
                    PanelMovingParticle.SetActive(true);
                    PanelMovingParticle.transform.position = tapObject.transform.position;
                }
                // パネルオブジェクトの入れ替え
                GameObject temp_mat;
                temp_mat = Panel.PanelObject[swapX1, swapY1];
                Panel.PanelObject[swapX1, swapY1] = Panel.PanelObject[swapX2, swapY2];
                Panel.PanelObject[swapX2, swapY2] = temp_mat;

                switch (PanelDirection)
                {
                    case MoveDirection.Down:
                        tappedPanelY--; // drag用
                        break;

                    case MoveDirection.Up:
                        tappedPanelY++; // drag用
                        break;

                    case MoveDirection.Right:
                        tappedPanelX++; // drag用
                        break;

                    case MoveDirection.Left:
                        tappedPanelX--; // drag用
                        break;
                }

                PanelDirection = MoveDirection.None;    // パネル移動中フラグを落とす

                if (gameStatus == GameStatus.ChallengeTime)
                {
                    if (JudgeFuseConnection())
                    {
                        return true;    // ゲームオーバーかステージクリア
                    }
                }
            }
        }

#if !PUSH_CONTROL
        bool push = Input.GetMouseButton(0);
        if (push)    // GetMouseButtonDown(0)) <- 押された瞬間なのでアカン
        {
            if (pushButtonCount == 0)
            {
                // ここでの注意点は座標の引数にVector2を渡すのではなく、Vector3を渡すことである。
                // Vector3でマウスがクリックした位置座標を取得する
                clickPosition = Input.mousePosition;
                // Z軸修正
                clickPosition.z = 10f;
                // スクリーン座標をワールド座標に変換する
                tapPosition = Camera.main.ScreenToWorldPoint(clickPosition);

                float x;
                float y;
                for (i = 0; i < Panel.Size; i++)
                {
                    for (j = 0; j < Panel.Size; j++)
                    {
                        x = Panel.PanelObject[i, j].transform.position.x;
                        y = Panel.PanelObject[i, j].transform.position.y;

                        if ((x - panelWidth / 2 < tapPosition.x) && (x + panelWidth / 2 > tapPosition.x))
                        {
                            if ((y - panelHeight / 2 < tapPosition.y) && (y + panelHeight / 2 > tapPosition.y))
                            {
                                // HitしたObjectがあるかどうか
                                tapObject = Panel.PanelObject[i, j];
                                pushButtonCount++;
                                tappedPanelX = i;
                                tappedPanelY = j;
                                if (tapObject.name.Substring(0, 5) == "Plain")   // "PlainPanel"
                                {
                                    tapObject.GetComponent<Animation>().Stop();
                                }
                                tapObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.8f, 0.0f);  // 掴んだパネル
 
                                return;
                            }
                        }
                    }
                }
            }
            else  // リリースではなくドラッグでパネルを移動させる
            {
#if DRAG_CONTROL
            if (PanelDirection == MoveDirection.None)    // パネル移動アニメーション中じゃなかったら
                {
                    clickPosition = Input.mousePosition;
                    clickPosition.z = 10f;
                    Vector3 dragPosition = Camera.main.ScreenToWorldPoint(clickPosition);
                    float deltax = dragPosition.x - tapPosition.x;
                    float deltay = dragPosition.y - tapPosition.y;

                    if ((Mathf.Abs(deltax) > panelWidth) || (Mathf.Abs(deltay) > panelHeight))
                    {
                        if (tapObject.name.Substring(0, 5) == "Plain")   // "PlainPanel"
                        {
                            tapObject.GetComponent<Animation>().Stop();
                            tapObject.GetComponent<Animation>().Play("PanelColorTappedAnimation");
                        }
                        Debug.Log("  tapped:" + tapPosition + "  drag:" + dragPosition);
                        Debug.Log("dX:" + deltax + "  dY:" + deltay);

                        if ((Mathf.Abs(deltax) == 0) && (Mathf.Abs(deltay) == 0))
                        {
                            tapDirection = MoveDirection.None;
                        }
                        else if (Mathf.Abs(deltax) > Mathf.Abs(deltay))
                        {
                            if (deltax > 0)
                            {
                                tapDirection = MoveDirection.Right;
                                tapPosition.x += panelWidth;
                            }
                            else
                            {
                                tapDirection = MoveDirection.Left;
                                tapPosition.x -= panelWidth;
                            }
                        }
                        else
                        {
                            tapPosition.y = dragPosition.y;
                            if (deltay > 0)
                            {
                                tapDirection = MoveDirection.Up;
                                tapPosition.y += panelHeight;
                            }
                            else
                            {
                                tapDirection = MoveDirection.Down;
                                tapPosition.y -= panelHeight;
                            }
                        }
                        i = tappedPanelX;
                        j = tappedPanelY;
                        if (tapObject.name.Substring(0, 5) == "Plain")   // "PlainPanel"
                        { 
                            tapObject.GetComponent<Animation>().Stop();
                        }
                        tapObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.8f, 0.0f);  // 掴んだパネル
                        goto FOUND_OBJECT;
                    }
                }
#endif
            }
        }
        else
        {
            if (pushButtonCount != 0)   // Release
            {
                //tapObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);  // 掴んだパネル
                if (tapObject.name.Substring(0, 5) == "Plain")   // "PlainPanel"
                {
                    tapObject.GetComponent<Animation>().Stop();
                    tapObject.GetComponent<Animation>().Play("PanelColorTappedAnimation");
                }
                pushButtonCount = 0;
#if !DRAG_CONTROL 
                clickPosition = Input.mousePosition;
                clickPosition.z = 10f;
                Vector3 releasePosition = Camera.main.ScreenToWorldPoint(clickPosition);
                float deltax = releasePosition.x - tapPosition.x;
                float deltay = releasePosition.y - tapPosition.y;

                if ((Mathf.Abs(deltax) == 0) && (Mathf.Abs(deltay) == 0))
                {
                    tapDirection = MoveDirection.None;
                }
                else if (Mathf.Abs(deltax) > Mathf.Abs(deltay))
                {
                    if (deltax > 0)
                    {
                        tapDirection = MoveDirection.Right;
                    }
                    else
                    {
                        tapDirection = MoveDirection.Left;
                    }
                }
                else
                {
                    if (deltay > 0)
                    {
                        tapDirection = MoveDirection.Up;
                    }
                    else
                    {
                        tapDirection = MoveDirection.Down;
                    }
                }
                i = tappedPanelX;
                j = tappedPanelY;
                goto FOUND_OBJECT;
#endif
            }
        }
#else   // PUSH_CONTROL
        bool push = Input.GetMouseButton(0);
        if (push)   
        {
            if (pushButtonCount == 0)
            {
                // ここでの注意点は座標の引数にVector2を渡すのではなく、Vector3を渡すことである。
                // Vector3でマウスがクリックした位置座標を取得する
                clickPosition = Input.mousePosition;
                // Z軸修正
                clickPosition.z = 10f;
                // スクリーン座標をワールド座標に変換する
                tapPosition = Camera.main.ScreenToWorldPoint(clickPosition);

                float x;
                float y;
                for (i = 0; i < Panel.Size; i++)
                {
                    for (j = 0; j < Panel.Size; j++)
                    {
                        x = Panel.PanelObject[i, j].transform.position.x;
                        y = Panel.PanelObject[i, j].transform.position.y;

                        if ((x - panelWidth / 2 < tapPosition.x) && (x + panelWidth / 2 > tapPosition.x))
                        {
                            if ((y - panelHeight / 2 < tapPosition.y) && (y + panelHeight / 2 > tapPosition.y))
                            {
                                // HitしたObjectがあるかどうか
                                tapObject = Panel.PanelObject[i, j];
                                pushButtonCount++;
                                tappedPanelX = i;
                                tappedPanelY = j;

                                if (tapObject.name.Substring(0, 5) == "Fixed")   // 動かせないパネル
                                {
                                    pushButtonCount = 0;    // 入力キャンセル

                                    return false;
                                }
                                else if (tapObject.name.Substring(0, 5) == "Plain")   // "PlainPanel"
                                {
                                    tapObject.GetComponent<Animation>().Stop();
                                }
                                if (tapObject.name.Substring(0, 2) == "No")   // NoPanel    Y.Ida added 2017.10.08
                                {
                                    pushButtonCount = 0;    // 入力キャンセル

                                    return false;
                                }
                                tapObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.8f, 0.0f);  // 掴んだパネル
 
                                return false;
                            }
                        }
                    }
                }
            }
        }
        if (true)
        {
            if (!push && (pushButtonCount == -1)) // パネル移動してから一度指が離された
            {
                pushButtonCount = 0;     
            }
            if (pushButtonCount > 0)   // パネルをタップした
            {
                //tapObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);  // 掴んだパネル
                if (tapObject.name.Substring(0, 5) == "Plain")   // "PlainPanel"
                {
                    tapObject.GetComponent<Animation>().Stop();
                    tapObject.GetComponent<Animation>().Play("PanelColorTappedAnimation");
                }

                clickPosition = Input.mousePosition;
                clickPosition.z = 10f;
                Vector3 releasePosition = Camera.main.ScreenToWorldPoint(clickPosition);
                float deltax = releasePosition.x - tapPosition.x;
                float deltay = releasePosition.y - tapPosition.y;

                if ((Mathf.Abs(deltax) < panelWidth / 8f) && (Mathf.Abs(deltay) < panelHeight / 8f))
                {
                    tapDirection = MoveDirection.None;
                    if (!push)
                    {
                        pushButtonCount = 0;    // 一度パネルから指を離された（パネル移動をキャンセルした）
                    }
                    return false;
                }
                else if (Mathf.Abs(deltax) > Mathf.Abs(deltay))
                {
                    if (deltax > 0)
                    {
                        tapDirection = MoveDirection.Right;
                    }
                    else
                    {
                        tapDirection = MoveDirection.Left;
                    }
                }
                else
                {
                    if (deltay > 0)
                    {
                        tapDirection = MoveDirection.Up;
                    }
                    else
                    {
                        tapDirection = MoveDirection.Down;
                    }
                }
                i = tappedPanelX;
                j = tappedPanelY;
                pushButtonCount = -1;   // パネル移動した
                
                goto FOUND_OBJECT;
            }
        }
#endif
        return false;

        FOUND_OBJECT:
        PanelType temp;

        //Debug.Log("x:" + i + " y:" + j);

        if ((i != 0) && (tapDirection == MoveDirection.Left))
        {
            if (Panel.PanelObject[i - 1, j].name.Substring(0, 2) == "No")   // "NoPanel"
            {
                PanelDirection = MoveDirection.Left;
                MovingPanel = Panel.PanelObject[i, j];
                Vector2 pos = Panel.PanelObject[i - 1, j].transform.position;
                pos.x += panelWidth;
                Panel.PanelObject[i - 1, j].transform.position = pos;
                moveDistance = panelWidth;

                temp = Panel.Matrix[i - 1, j];
                Panel.Matrix[i - 1, j] = Panel.Matrix[i, j];
                Panel.Matrix[i, j] = temp;

                swapX1 = i;
                swapX2 = i - 1;
                swapY1 = j;
                swapY2 = j;

                if ((i == currentX) && (j == currentY))
                {
                    currentX--;
                }
                steps++;

                //DebugStatus();

                return false;
            }
        }
        if ((j != 0) && (tapDirection == MoveDirection.Down))
        {
            if (Panel.PanelObject[i, j - 1].name.Substring(0, 2) == "No")   // "NoPanel"
            {
                //Panel.PanelObject[i, j].GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -4f), ForceMode2D.Impulse);
                PanelDirection = MoveDirection.Down;
                MovingPanel = Panel.PanelObject[i, j];
                Vector2 pos = Panel.PanelObject[i, j - 1].transform.position;
                pos.y += panelHeight;
                Panel.PanelObject[i, j - 1].transform.position = pos;
                moveDistance = panelHeight;

                temp = Panel.Matrix[i, j - 1];
                Panel.Matrix[i, j - 1] = Panel.Matrix[i, j];
                Panel.Matrix[i, j] = temp;

                swapX1 = i;
                swapX2 = i;
                swapY1 = j;
                swapY2 = j - 1;

                if ((i == currentX) && (j == currentY))
                {
                    currentY--;
                }
                steps++;

                //DebugStatus();

                return false;

            }
        }

        if ((i != Panel.Size - 1) && (tapDirection == MoveDirection.Right))
        {
            if (Panel.PanelObject[i + 1, j].name.Substring(0, 2) == "No")   // "NoPanel"
            {
                PanelDirection = MoveDirection.Right;
                MovingPanel = Panel.PanelObject[i, j];
                Vector2 pos = Panel.PanelObject[i + 1, j].transform.position;
                pos.x -= panelWidth;
                Panel.PanelObject[i + 1, j].transform.position = pos;
                moveDistance = panelWidth;

                temp = Panel.Matrix[i + 1, j];
                Panel.Matrix[i + 1, j] = Panel.Matrix[i, j];
                Panel.Matrix[i, j] = temp;

                swapX1 = i;
                swapX2 = i + 1;
                swapY1 = j;
                swapY2 = j;

                if ((i == currentX) && (j == currentY))
                {
                    currentX++;
                }
                steps++;

                //DebugStatus();

                return false;

            }
        }
        if ((j != Panel.Size - 1) && (tapDirection == MoveDirection.Up))
        {
            if (Panel.PanelObject[i, j + 1].name.Substring(0, 2) == "No")   // "NoPanel"
            {
                PanelDirection = MoveDirection.Up;
                MovingPanel = Panel.PanelObject[i, j];
                Vector2 pos = Panel.PanelObject[i, j + 1].transform.position;
                pos.y -= panelHeight;
                Panel.PanelObject[i, j + 1].transform.position = pos;
                moveDistance = panelHeight;

                temp = Panel.Matrix[i, j + 1];
                Panel.Matrix[i, j + 1] = Panel.Matrix[i, j];
                Panel.Matrix[i, j] = temp;

                swapX1 = i;
                swapX2 = i;
                swapY1 = j;
                swapY2 = j + 1;

                if ((i == currentX) && (j == currentY))
                {
                    currentY++;
                }

                steps++;

                //DebugStatus();

                return false;

            }
        }

        PanelDirection = MoveDirection.None;

        return false;
    }

    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //　現在の燃えているパネルのタイプを返却する
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    public PanelType GetCurrentPanelType()
    {
        PanelType currentType = Panel.Matrix[currentX, currentY];

        return currentType;
    }

    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //　現在の燃えているパネルが最終的に燃えていく方向を返却する
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    public FuseDirection GetCurrentFuseDirection()
    {
        return CurrentFusePanelDirection;
    }

    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //　導火線がパネルの中で燃え尽きた時に導火線が繋がっているかどうか判断する
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    public bool JudgeFuseConnection()
    {
        PanelType currentType = Panel.Matrix[currentX, currentY];
        FuseDirection nextStartDirection = FuseDirection.Up;    // Just in case.
        bool challengeTimeFlag;
        long tempx, tempy;

        if ((PanelDirection != MoveDirection.None) && (MovingPanel != null))    // パネル移動アニメーション中か？
        {
            return false;
        }

        tempx = currentX; tempy = currentY;

        // Panel.PanelObject[currentX, currentY].GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f);  // 燃え終わり灰色
        if (Panel.PanelObject[currentX, currentY].name.Substring(0, 2) != "No")   // "NoPanel"
        {
            Panel.PanelObject[currentX, currentY].GetComponent<Animation>().Stop();
            Panel.PanelObject[currentX, currentY].GetComponent<Animation>().Play("PanelColorBurntAnimation");
        }

        // 移動可能方向
        //    case PanelType.StraightVertical:    // Down, Up  
        //    case PanelType.StraightHorizontal;  // Right, Left
        //    case PanelType.CurveLeftUp:         // Left, Up
        //    case PanelType.CurveUpRight:        // Up, RIght
        //    case PanelType.CurveRightDown:      // Right, Down
        //    case PanelType.CurveDownLeft:       // Down, Left
        //    case PanelType.CurveLeftUp_RightDown:   // Down, Up, RIght, Left
        //    case PanelType.CurveDownLeft_UpRight:   // Down, Up, RIght, Left
        challengeTimeFlag = false;

        switch (CurrentFusePanelDirection)
        {
            case FuseDirection.Down:
                nextStartDirection = FuseDirection.Up;  //　次のパネルの燃え始めの方向
                if (currentY > 0)
                {
                    currentType = Panel.Matrix[currentX, currentY - 1];

                    if (Panel.Matrix[currentX, currentY - 1] == PanelType.StraightVertical)
                    {
                        currentY--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Down; // 燃えていく方向
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY - 1] == PanelType.CurveLeftUp)
                    {
                        currentY--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Left;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY - 1] == PanelType.CurveUpRight)
                    {
                        currentY--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Right;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY - 1] == PanelType.CurveLeftUp_RightDown)
                    {
                        currentY--;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveLeftUp_RightDown_RightDownOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Left;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY - 1] == PanelType.CurveDownLeft_UpRight)
                    {
                        currentY--;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveDownLeft_UpRight_DownLeftOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Right;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY - 1] == PanelType.CurveLeftUp_RightDown_LeftUpOnlyEnabled)
                    {
                        currentY--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Left;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY - 1] == PanelType.CurveDownLeft_UpRight_UpRightOnlyEnabled)
                    {
                        currentY--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Right;
                        break;
                    }
                    // gameover
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                else
                {
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                break;

            case FuseDirection.Up:
                nextStartDirection = FuseDirection.Down;
                if (currentY < Panel.Size - 1)
                {
                    currentType = Panel.Matrix[currentX, currentY + 1];

                    if (Panel.Matrix[currentX, currentY + 1] == PanelType.StraightVertical)
                    {
                        currentY++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Up;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY + 1] == PanelType.CurveRightDown)
                    {
                        currentY++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Right;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY + 1] == PanelType.CurveDownLeft)
                    {
                        currentY++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Left;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY + 1] == PanelType.CurveLeftUp_RightDown)
                    {
                        currentY++;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveLeftUp_RightDown_LeftUpOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Right;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY + 1] == PanelType.CurveDownLeft_UpRight)
                    {
                        currentY++;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveDownLeft_UpRight_UpRightOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Left;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY + 1] == PanelType.CurveLeftUp_RightDown_RightDownOnlyEnabled)
                    {
                        currentY++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Right;
                        break;
                    }
                    if (Panel.Matrix[currentX, currentY + 1] == PanelType.CurveDownLeft_UpRight_DownLeftOnlyEnabled)
                    {
                        currentY++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Left;
                        break;
                    }
                    // gameover
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                else
                {
                    // gameover
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                break;

            case FuseDirection.Left:
                nextStartDirection = FuseDirection.Right;
                if (currentX > 0)
                {
                    currentType = Panel.Matrix[currentX - 1, currentY];

                    if (Panel.Matrix[currentX - 1, currentY] == PanelType.StraightHorizontal)
                    {
                        currentX--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Left;
                        break;
                    }
                    if (Panel.Matrix[currentX - 1, currentY] == PanelType.CurveUpRight)
                    {
                        currentX--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Up;
                        break;
                    }
                    if (Panel.Matrix[currentX - 1, currentY] == PanelType.CurveRightDown)
                    {
                        currentX--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Down;
                        break;
                    }
                    if (Panel.Matrix[currentX - 1, currentY] == PanelType.CurveLeftUp_RightDown)
                    {
                        currentX--;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveLeftUp_RightDown_LeftUpOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Down;
                        break;
                    }
                    if (Panel.Matrix[currentX - 1, currentY] == PanelType.CurveDownLeft_UpRight)
                    {
                        currentX--;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveDownLeft_UpRight_DownLeftOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Up;
                        break;
                    }
                    if (Panel.Matrix[currentX - 1, currentY] == PanelType.CurveLeftUp_RightDown_RightDownOnlyEnabled)
                    {
                        currentX--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Down;
                        break;
                    }
                    if (Panel.Matrix[currentX - 1, currentY] == PanelType.CurveDownLeft_UpRight_UpRightOnlyEnabled)
                    {
                        currentX--;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Up;
                        break;
                    }
                    // gameover
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                else
                {
                    // gameover
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                break;

            case FuseDirection.Right:
                if ((currentX == Panel.Size - 1) && (currentY == 0))
                {
                    // game clear!!!!!!!!!
                    // ロケットが右下にあると仮定したら
                    StageClear();
                    //GotoStage();
                    return true;
                }

                nextStartDirection = FuseDirection.Left;
                if (currentX < Panel.Size - 1)
                {
                    currentType = Panel.Matrix[currentX + 1, currentY];

                    if (Panel.Matrix[currentX + 1, currentY] == PanelType.StraightHorizontal)
                    {
                        currentX++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Right;
                        break;
                    }
                    if (Panel.Matrix[currentX + 1, currentY] == PanelType.CurveLeftUp)
                   {
                        currentX++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Up;
                        break;
                    }
                    if (Panel.Matrix[currentX + 1, currentY] == PanelType.CurveDownLeft)
                    {
                        currentX++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Down;
                        break;
                    }
                    if (Panel.Matrix[currentX + 1, currentY] == PanelType.CurveLeftUp_RightDown)
                    {
                        currentX++;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveLeftUp_RightDown_RightDownOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Up;
                        break;
                    }
                    if (Panel.Matrix[currentX + 1, currentY] == PanelType.CurveDownLeft_UpRight)
                    {
                        currentX++;
                        Panel.Matrix[currentX, currentY] = PanelType.CurveDownLeft_UpRight_UpRightOnlyEnabled;
                        CurrentFusePanelDirection = FuseDirection.Down;
                        break;
                    }
                    if (Panel.Matrix[currentX + 1, currentY] == PanelType.CurveLeftUp_RightDown_LeftUpOnlyEnabled)
                    {
                        currentX++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Up;
                        break;
                    }
                    if (Panel.Matrix[currentX + 1, currentY] == PanelType.CurveDownLeft_UpRight_DownLeftOnlyEnabled)
                    {
                        currentX++;
                        Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                        CurrentFusePanelDirection = FuseDirection.Down;
                        break;
                    }
                    // gameover
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                else
                {
                    // gameover
                    ChallengeTime();
                    challengeTimeFlag = true;
                }
                break;
        }
        if (gameStatus == GameStatus.Play)
        {
            PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], nextStartDirection, currentType);
        }
        else if ((gameStatus == GameStatus.ChallengeTime) && (challengeTimeFlag == false))
        {
            PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], nextStartDirection, currentType);
            FireCracker.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
            gameStatus = GameStatus.Play;
        }

        if (Panel.PanelObject[currentX, currentY].name.Substring(0, 2) != "No")   // "NoPanel" チャレンジタイムに入る時に1フレーム分パネルの色がちらつくための防止
        {
            if (gameStatus == GameStatus.ChallengeTime)
            {
                Panel.PanelObject[tempx, tempy].GetComponent<Animation>().Stop();
            }
        }

        //DebugStatus1(nextStartDirection);

        //-------------------------------------------------------------------------------debug
        for (int iii = 0; iii < Panel.Size; iii++)
        {
            for (int jjj = 0; jjj < Panel.Size; jjj++)
            {
                if ((Panel.Matrix[iii, jjj] == PanelType.CurveLeftUp_RightDown_LeftUpOnlyEnabled)
                 || (Panel.Matrix[iii, jjj] == PanelType.CurveLeftUp_RightDown_RightDownOnlyEnabled)
                 || (Panel.Matrix[iii, jjj] == PanelType.CurveDownLeft_UpRight_UpRightOnlyEnabled)
                 || (Panel.Matrix[iii, jjj] == PanelType.CurveDownLeft_UpRight_DownLeftOnlyEnabled))
                {
                    // Panel.PanelObject[iii, jjj].GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.5f, 0.5f);  // 片側だけ燃えた
                }
            }
        }
        //Panel.PanelObject[currentX, currentY].GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.8f, 0.8f);  // 燃え中のパネル
        //-------------------------------------------------------------------------------debug
        return false;
   }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// </summary>
    void StartPanelProcedure()
    {
        switch (Panel.Matrix[currentX, currentY])
        {
            case PanelType.StraightHorizontal:
            case PanelType.CurveDownLeft:
            case PanelType.CurveRightDown:
            case PanelType.CurveUpRight:
            case PanelType.StraightVertical:
            case PanelType.CurveLeftUp:
            default:
                Panel.Matrix[currentX, currentY] = PanelType.Disabled;
                break;

            case PanelType.CurveLeftUp_RightDown:
                Panel.Matrix[currentX, currentY] = PanelType.CurveLeftUp_RightDown_RightDownOnlyEnabled;
                break;

            case PanelType.CurveDownLeft_UpRight:
                Panel.Matrix[currentX, currentY] = PanelType.CurveDownLeft_UpRight_UpRightOnlyEnabled;
                break;
        }
    }
    
    /// <summary>
    /// ///////////////////////////////////////////////////////
    /// </summary>
    void ChallengeTime()
    {
        gameStatus = GameStatus.ChallengeTime;
        //PanelSubSystem.GetComponent<PanelObject>().StartFlag = false;
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="str"></param>
    void GameOver(string str = "MISS")
    {
        //       Debug.Log("x:" + currentX + "y:" + currentY);
        InformationText.GetComponent<Text>().text = str;

        InformationText.GetComponent<Animation>().Stop();
        InformationText.GetComponent<Animation>().Play("PopUpTextAnimation");

        //Animator anime = MainCamera.GetComponent<Animator>();
        //anime.Play("CameraBlur");

        PanelSubSystem.GetComponent<PanelObject>().StartFlag = false;       // サブシステムを停止する

    }

    /// <summary>
    /// ////////////////////////////////////////////////////////
    /// </summary>
    void GameOver2()
    {
        for (int i = 0; i < Panel.Size; i++)
        {
            for (int j = 0; j < Panel.Size; j++)
            {
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().useGravity = true;
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().AddForce(new Vector3((float)0f, ((float)j - Panel.Size / 2f) * 40f, 0));
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f));
            }
        }

        NextArrowImage.SetActive(false);
    }

    /// <summary>
    /// １機死んだ時
    ///  ret = true : ゲームオーバー
    ///  ret = false : ミス（1機失う）
    /// </summary>
    bool CalculateLife()
    {
        DateTime now = DateTime.Now;
        TimeSpan ts = now - GlobalVariables.RestartTime;    //DateTime の差が TimeSpan として返る
        if (ts.TotalSeconds > GlobalVariables.TimeToStart * 60 * GlobalVariables.LifeMax)    // Lifeが満タンの時
        {
            GlobalVariables.RestartTime = DateTime.Now.AddSeconds(-GlobalVariables.TimeToStart * 60 * GlobalVariables.LifeMax);
        }

        GlobalVariables.RestartTime = GlobalVariables.RestartTime.AddMinutes(GlobalVariables.TimeToStart);
        PlayerPrefs.SetString("RESTART_TIME", GlobalVariables.RestartTime.ToBinary().ToString());

        ts = now - GlobalVariables.RestartTime; // 1機失った後の時間
        if (ts.TotalSeconds < GlobalVariables.TimeToStart * 60)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    void DebugStatus()
    {
        Debug.Log("CurrentX:" + currentX + "y:" + currentY);

    }
    void DebugStatus1(FuseDirection nextStartDirection)
    {
        Debug.Log("CurrentX:" + currentX + "y:" + currentY);
        Debug.Log("CurrentFusePanelDirection:" + CurrentFusePanelDirection + "  nextStartDirection:" + nextStartDirection);

    }

    /// <summary>
    /// //////////////////////////////////////////////////////
    /// </summary>
    void StageEndCommonProcedure()
    {
        TimeText.GetComponent<Animation>().Stop();
        TimeText.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f);
        timeTextAnimationFlag = false;

        FireCracker.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);     // 0にするとAssertion Failureになる なんで？？ゼロ割？
        PanelSubSystem.GetComponent<PanelObject>().StartFlag = false;

    }

    /// <summary>
    /// ///////////////////////////////////////////////////////
    /// </summary>
    void GameEnd()
    {
        //       Debug.Log("x:" + currentX + "y:" + currentY);
        InformationText.GetComponent<Text>().text = "GAME OVER";
        //Animator anime = MainCamera.GetComponent<Animator>();
        //anime.Play("CameraBlur");

        Animator anime = MainCamera.GetComponent<Animator>();
        anime.Rebind();
        anime.Play("CameraShocked");

        InformationText.GetComponent<Animation>().Play("TextAnimation");

        explosionPrefab = Instantiate(Explosion, new Vector3(panelWidth * 1f, panelHeight * 1f, 0f), Quaternion.Euler(0f, 0f, 0f)) as GameObject;

        for (int i = 0; i < Panel.Size; i++)
        {
            for (int j = 0; j < Panel.Size; j++)
            {
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().useGravity = true;
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().AddForce(new Vector3(((float)i - Panel.Size / 2f) * 40f, ((float)j - Panel.Size / 2f) * 40f, 0));
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f));

            }
        }

        NextArrowImage.SetActive(false);

        Missile.GetComponent<Rigidbody>().useGravity = true;
        Missile.GetComponent<Rigidbody>().AddForce(new Vector3(200f, 120f, 0f));
        Missile.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f));

    }

    /// <summary>
    /// 
    /// </summary>
    void StageClear()
    {
        TimeText.GetComponent<Animation>().Stop();
        TimeText.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f);
        timeTextAnimationFlag = false;

        explosionPrefab = Instantiate(ExplosionM, FireCracker.transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;

        StartCoroutine("AnimateSpinPanel");

        //        FireCracker.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        ScoreDialogImage.GetComponent<ScoreDialog>().StartDialogAnimation(timeRemain, steps, excellentScore, stageParameter[stageCount].HiScore);   // スコアダイアログを表示

        InformationText.GetComponent<Text>().text = "STAGE " + stageCount + " CLEAR";
        InformationText.GetComponent<Animation>().Stop();
        InformationText.GetComponent<Animation>().Play("TextAnimation");

        gameStatus = GameStatus.Cleared;

        if ((stageCount % rocketStagePeriod) == rocketStagePeriod - 1)  // ロケット発射1ステージ前
        {
            StageData(stageCount + 1);         // 次のステージのデータ
            CreateStage(out NextPanel, false);
            CreateStageToStageRope(out nextStageToStageFireRope, RopeForStage.NextStageRocket, 0);    // ロケットへのロープ
        }
        else if ((stageCount % rocketStagePeriod) == 0)  // ロケット発射
        {
            Missile.transform.position = new Vector3(
                 nextStageToStageFireRope[nextStageToStageFireRope.Length - 1].transform.position.x + (Missile.GetComponent<SpriteRenderer>().bounds.size.x / 2f), 
                 nextStageToStageFireRope[nextStageToStageFireRope.Length - 1].transform.position.y + (Missile.GetComponent<SpriteRenderer>().bounds.size.y / 2f),
                0f);
        }
        else if ((stageCount % rocketStagePeriod) >= 1) 
        {
            StageData(stageCount + 1);         // 次のステージのデータ
            CreateStage(out NextPanel, false);
            StageData(stageCount + 2);        // 次の次のステージのデータ
            CreateStageToStageRope(out nextStageToStageFireRope, RopeForStage.NextNextStage, panelSize);
        }
        else
        {
            // Never come here.
        }

        StageData(stageCount + 1);         // 次のステージのデータをセット

        PanelSubSystem.GetComponent<PanelObject>().RemoveFireCrackerFromParent();
        PanelSubSystem.GetComponent<PanelObject>().StartFlag = false;

        FireCracker.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        NextArrowImage.SetActive(false);
        //       StartCoroutine("AnimationStageRopeBurning");
    }
    // StartBurning(Direction, PanelType);
    // これをパネルのプレハブ(PlainPanel)のクラスメソッドに実装する。
    // これはパネルが燃え移るタイミングでシステムから呼ばれる。
    // パネルオブジェクトは自分のパネルタイプと燃える方向が知らされる。
    // パネルオブジェクトはアニメーションで3秒くらいで自分のタイミングで燃えるアニメーションを表示して、
    // パネルの燃え終わりのタイミングをパネルオブジェクトから通知する。
    // JudgeFuseConnection()


    /// <summary>
    ///
    /// </summary>
    void SpeedUp()
    {
        bool button = PanelSubSystem.GetComponent<PanelObject>().SpeedUpFlag;

        PanelSubSystem.GetComponent<PanelObject>().SpeedUpFlag  = !button;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Restart()
    {
        restartFlag = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void GoNext()
    {
        okFlag = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void GoHome(bool ScoreSave = true)
    {
        if (ScoreSave)  // ステージクリアした時
        {
            CalculateScore();
        }
        else // ゲームオーバーの時
        {   
            //PlayerPrefs.SetString("RESTART_TIME", GlobalVariables.RestartTime.ToBinary().ToString());
        }
        SaveGameData();
        StartCoroutine("FadeOutScreen");

        //SceneManager.LoadScene("TitleMain");
    }

    /// <summary>
    /// 
    /// </summary>
    public void Share()
    {
        UniStoreOpener.OpenStore();
    }


    /// <summary>
    /// //////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimationStageRopeBurning()
    {
        Vector3 deltax = new Vector3(panelWidth * 11f / stageToStageFireRope.Length, 0f, 0f);

        FireCracker.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        FireCracker.transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < stageToStageFireRope.Length; i++)
        {
            stageToStageFireRope[i].transform.GetComponent<SpriteRenderer>().sprite = BurntSprite;
            stageToStageFireRope[i].transform.position += new Vector3(UnityEngine.Random.value * 0.010f - 0.005f, UnityEngine.Random.value * 0.010f - 0.005f, 0f);

            FireCracker.transform.position = stageToStageFireRope[i].transform.position;

            if ((NextPanel.PanelObject[0, 0].transform.position.x <= 0f) && ((stageCount % rocketStagePeriod) != 0))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            else if (((stageCount % rocketStagePeriod) == 0) && (Missile.transform.position.x <= panelWidth * 3f))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            for (int k = 0; k < Panel.Size; k++)
            {
                for (int j = 0; j < Panel.Size; j++)
                {
                    Panel.PanelObject[k, j].transform.position -= deltax;

                }
            }
            if (Panel != NextPanel) // Stage 5, 10, 15...以外
            {
                for (int k = 0; k < NextPanel.Size; k++)
                {
                    for (int j = 0; j < NextPanel.Size; j++)
                    {
                        NextPanel.PanelObject[k, j].transform.position -= deltax;

                    }
                }
            }

            for (int k = 0; k < stageToStageFireRope.Length; k++)
            {
                stageToStageFireRope[k].transform.position -= deltax;
            }

            if (stageToStageFireRope != nextStageToStageFireRope)   // Stage 5, 10, 15...以外
            {
                for (int k = 0; k < nextStageToStageFireRope.Length; k++)
                {
                    nextStageToStageFireRope[k].transform.position -= deltax;
                }

            }

            if (stageCount % rocketStagePeriod == 0)
            {
                Missile.transform.position -= deltax;
            }

            backGround.transform.position -= deltax;
            if ((nextBackGround != backGround) && (nextBackGround != null))  // Stage 5, 10, 15...以外
            {
                nextBackGround.transform.position -= deltax;
            }

            if (Input.GetMouseButton(0))    // キャンセル
            {
                continue;
            }

            yield return new WaitForEndOfFrame();

        }
        stageRopeBurntFlag = true;
        FireCracker.transform.GetChild(0).gameObject.SetActive(true);

        yield break;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimateSpinPanel()
    {
        for (int i = 0; i < Panel.Size; i++)
        {
            for (int j = 0; j < Panel.Size; j++)
            {
                if ((Panel.Matrix[i, j] != PanelType.Disabled) && (Panel.Matrix[i, j] != PanelType.Fixed) && (Panel.Matrix[i, j] != PanelType.Nothing))
                {
                    Panel.PanelObject[i, j].GetComponent<Animation>().Play("PanelSpinAnimation");
                }
                yield return new WaitForEndOfFrame();
            }
        }

        yield break;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="sec"></param>
    /// <returns></returns>
    public IEnumerator FadeOutScreen()
    {
        float sec = 0.5f;
        for (float i = 0; i < 1f; i += 1f / (sec * 60))
        {
            // camera.orthographicSize /= 1.1f;
            FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, i);

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("TitleMain");

        yield break;
    }

}


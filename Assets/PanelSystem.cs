//#define DRAG_CONTROL
#define PUSH_CONTROL


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

public enum Stage
{
    S_1 = 1,
    S_2,
    S_3,
    S_4,
    S_5,
    S_6,
    S_7,
    S_8,
    S_9,
    S_10,
    S_11,
    S_12,
    S_13,
    S_14,
    S_15,
    S_16,
    S_17,
    S_18,
    S_19,
    S_20,
    S_21,
    S_22,
    S_23,
    S_24,
    S_25,
    S_26,
    S_27,
    S_28,
    S_29,
    S_30,
    S_31,
    S_32,
    S_33,
    S_34,
    S_35,
    S_36,
    S_37,
    S_38,
    S_39,
    S_40,
    S_41,
    S_42,
    S_43,
    S_44,
    S_45,
    S_46,
    S_47,
    S_48,
    S_49,

}

//--------------------------------------------------------------------------
//--------------------------------------------------------------------------
//--------------------------------------------------------------------------
//--------------------------------------------------------------------------
public class PanelSystem : MonoBehaviour
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
    public GameObject SpeedUpButton;
    public GameObject Missile;
    public GameObject StageClearParticle;
    public GameObject Remain;
    public GameObject TimeText;
    public Sprite BurntSprite;
    public GameObject StageText;
    public GameObject PanelMovingParticle;
    public GameObject RestartButton;
    public GameObject TimeToRestartText;
    public GameObject ScoreDialogImage;

    private float panelWidth;
    private float panelHeight;
    private int panelSize;
    private PanelType[] stage;
    private GameObject[] Remains;
    private GameObject[] stageToStageFireRope;
    private GameObject[] nextStageToStageFireRope;

    private long currentX;
    private long currentY;
    private PanelMatrix Panel;
    private PanelMatrix NextPanel;
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

    private int stageCount;

    private GameStatus gameStatus;
    private int generalCounter;

    private GameObject explosionPrefab;
    private int life;
    private float timeRemain;
    private bool timeTextAnimationFlag;
    private const int rocketStagePeriod = 5;    // ロケットを飛ばすステージ間隔
    private bool restartFlag;
    private DateTime restartTime;
    private int steps;
    private int score;

    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
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
    /// //////////////////////////////////////////////////////////////
    /// </summary>
    void StageData(int stageNum)
    {
        switch ((Stage)stageNum)
        {
            case Stage.S_1:     // Stage1
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,

                     // X 3列目
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,
                    PanelType.Nothing,

                     // X 4列目
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_2:     // Stage2
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,

                     // X 3列目
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
                    PanelType.Nothing,

                     // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_3:     // Stage3
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,

                     // X 3列目
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_4:     // Stage4
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft,

                     // X 3列目
                    PanelType.CurveLeftUp,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveRightDown,

                     // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft,

                };
                break;

            case Stage.S_5:     // Stage5
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft,

                     // X 2列目
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,
                    PanelType.CurveUpRight,
                    PanelType.StraightHorizontal,

                     // X 3列目
                    PanelType.StraightHorizontal,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp_RightDown,

                     // X 4列目
                    PanelType.StraightHorizontal,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft,
                    PanelType.StraightVertical,

                };
                break;

            case Stage.S_6:     // Stage 6
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 20f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft,

                     // X 2列目
                    PanelType.CurveLeftUp,
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.CurveRightDown,

                     // X 3列目
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.CurveRightDown,
                    PanelType.StraightHorizontal,

                     // X 4列目
                    PanelType.StraightHorizontal,
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                };
                break;

            case Stage.S_7:     // Stage 7
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 60f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,

                     // X 3列目
                    PanelType.StraightHorizontal,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,

                     // X 4列目
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft_UpRight,

                };
                break;

            case Stage.S_8:     // Stage 8
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.StraightVertical,
                    PanelType.CurveRightDown,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.CurveUpRight,
                    PanelType.CurveLeftUp,
                    PanelType.StraightVertical,
                    PanelType.CurveRightDown,

                     // X 3列目
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveRightDown,
                    PanelType.CurveDownLeft_UpRight,

                     // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                };
                break;

            case Stage.S_9:     // Stage 9
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.CurveLeftUp,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                     // X 2列目
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,

                     // X 3列目
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                     // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,

                };
                break;

            case Stage.S_10:     // Stage 10
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 60f;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft,

                     // X 2列目
                    PanelType.Nothing,
                    PanelType.CurveRightDown,
                    PanelType.StraightVertical,
                    PanelType.CurveRightDown,

                     // X 3列目
                    PanelType.CurveLeftUp,
                    PanelType.CurveLeftUp,
                    PanelType.CurveRightDown,
                    PanelType.CurveDownLeft,

                     // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp,
                    PanelType.StraightVertical,

                };
                break;

            case Stage.S_11:     // Stage 11
                currentX = 0;
                currentY = 4;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage2 [5, 5]
                panelSize = 5;
                timeRemain = 30f;

                stage = new PanelType[]
                {
                    PanelType.CurveDownLeft,
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,

                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft_UpRight,

                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,

                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,

                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
               };
                break;

            case Stage.S_12:     // Stage 12
                currentX = 0;
                currentY = 4;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage2 [5, 5]
                panelSize = 5;
                timeRemain = 45f;

                stage = new PanelType[]
                {
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.CurveRightDown,
                    PanelType.StraightHorizontal,

                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,

                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,

                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft,
                    PanelType.StraightHorizontal,

                    PanelType.CurveLeftUp,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft,
               };
                break;

            case Stage.S_13:     // Stage 13
                currentX = 0;
                currentY = 4;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage2 [5, 5]
                panelSize = 5;
                timeRemain = 45f;

                stage = new PanelType[]
                {
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                    PanelType.StraightHorizontal,
                    PanelType.StraightVertical,
                    PanelType.StraightHorizontal,
                    PanelType.StraightVertical,
                    PanelType.StraightHorizontal,

                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,

                    PanelType.StraightHorizontal,
                    PanelType.StraightVertical,
                    PanelType.StraightHorizontal,
                    PanelType.StraightVertical,
                    PanelType.StraightHorizontal,

                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
               };
                break;

            case Stage.S_14:     // Stage 14
                currentX = 0;
                currentY = 4;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage2 [5, 5]
                panelSize = 5;
                timeRemain = 45f;

                stage = new PanelType[]
                {
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft,
                    PanelType.CurveDownLeft,
                    PanelType.StraightVertical,
                    PanelType.StraightHorizontal,

                    PanelType.CurveDownLeft,
                    PanelType.CurveRightDown,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,

                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.CurveLeftUp_RightDown,

                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveLeftUp,
                    PanelType.CurveRightDown,
                    PanelType.StraightVertical,

                    PanelType.StraightHorizontal,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.StraightVertical,
               };
                break;

            case Stage.S_15:     // Stage 15
                currentX = 0;
                currentY = 4;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage2 [5, 5]
                panelSize = 5;
                timeRemain = 45f;

                stage = new PanelType[]
                {
                    PanelType.Disabled,
                    PanelType.Fixed,
                    PanelType.Fixed,
                    PanelType.Fixed,
                    PanelType.StraightHorizontal,

                    PanelType.CurveDownLeft,
                    PanelType.Disabled,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,

                    PanelType.StraightHorizontal,
                    PanelType.Disabled,
                    PanelType.Nothing,
                    PanelType.Disabled,
                    PanelType.Fixed,

                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveLeftUp,
                    PanelType.Fixed,
                    PanelType.StraightVertical,

                    PanelType.StraightHorizontal,
                    PanelType.CurveDownLeft,
                    PanelType.Fixed,
                    PanelType.Fixed,
                    PanelType.StraightVertical,
               };
                break;

            case Stage.S_21:     // Stage 21
                currentX = 0;
                currentY = 5;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage3 [6, 6]
                panelSize = 6;
                timeRemain = 20f;

                stage = new PanelType[]
                {
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,

                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft,

                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.CurveRightDown,

                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft
                };
                break;

/*
            case Stage.S_7:     // Stage3
                currentX = 0;
                currentY = 5;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage3 [6, 6]
                panelSize = 6;
                TimeRemain = 60f;

                stage = new PanelType[]
                {
                    PanelType.Nothing,
                    PanelType.CurveDownLeft,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,

                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft_UpRight,

                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,

                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,

                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,

                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing
                };
                break;

            case Stage.S_8:     // Stage4
                currentX = 0;
                currentY = 6;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage3 [7, 7]
                panelSize = 7;
                TimeRemain = 60f;

                stage = new PanelType[]
                {
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,
                    PanelType.StraightHorizontal,

                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft_UpRight,

                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,

                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,

                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,

                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,

                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.StraightVertical,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing
                };
                break;

            case Stage.S_10:     // Stage6
                currentX = 0;
                currentY = 6;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage3 [7, 7]
                panelSize = 7;
                TimeRemain = 60f;

                stage = new PanelType[]
                {
                    PanelType.CurveDownLeft,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                    PanelType.CurveDownLeft,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft,

                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,

                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,

                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveUpRight,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,

                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightHorizontal,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,
                };
                break;
*/

            default:
                break;
        }
    }

    /// <summary>
    /// /////////////////////////////////////////////////////////
    // 左下を原点として上方向(Y方向)に指定の大きさ(panelSize)のパネルを並べていく
    /// </summary>
    /// <param name="stageNum"></param>
    void CreateStage(out PanelMatrix retpanel, bool currentFlag = true)
    {
        PanelMatrix panel  = new PanelMatrix(panelSize);
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
    void CreateStageToStageRope(out GameObject [] obj, RopeForStage stg, int nextPanelSize = 0)
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

        PanelDirection = MoveDirection.None;
        panelWidth = PlainPanel.GetComponent<SpriteRenderer>().bounds.size.x;  // 0.64 = 64 pixel
        panelHeight = PlainPanel.GetComponent<SpriteRenderer>().bounds.size.y;

        pushButtonCount = 0;
        stageCount = 1;
        gameStatus = GameStatus.Start;
        generalCounter = 0;
        InformationText.GetComponent<Text>().text = "";
        StageText.GetComponent<Text>().text = "STAGE : " + stageCount;

        StageData(stageCount);  
        CreateStage(out Panel);
        StageData(stageCount + 1);        // 次のステージのデータ
        CreateStageToStageRope(out stageToStageFireRope, RopeForStage.NextStage, panelSize);
        StageData(stageCount);           // ステージをセットし直す

        PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], FuseDirection.Left, Panel.Matrix[currentX, currentY]);

        Button btn = SpeedUpButton.GetComponent<Button>();
        btn.onClick.AddListener(SpeedUp);

        Button btn1 = RestartButton.GetComponent<Button>();
        btn1.onClick.AddListener(Restart);

        restartFlag = false;
        RestartButton.SetActive(false);

        life = 3;
        Remains = new GameObject [life];

        for (int i = 0; i < life; i++)
        {
            Remains[i] = Instantiate(Remain, new Vector3(0f + i * 0.4f, 5f, 0f), Quaternion.Euler(0f, 0f, 0f)) as GameObject;

        }
        timeTextAnimationFlag = false;

        steps = 0;
        score = 0;

        ScoreDialogImage.SetActive(false);
        TimeToRestartText.SetActive(false);
        StageClearParticle.SetActive(false);
        PanelMovingParticle.SetActive(false);
        InformationText.SetActive(false);

        StartPanelProcedure();
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

        for (int k = 0; k < stageToStageFireRope.Length; k++)
        {
            Destroy(stageToStageFireRope[k]);
        }

        Panel = NextPanel;      // 現在のステージと次のステージを入れ替え
        stageToStageFireRope = nextStageToStageFireRope;
        stageCount = stage;
        StageData(stageCount);

        //CreateStage(out NextPanel, false);

        // PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], FuseDirection.Left, Panel.Matrix[currentX, currentY]);

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
        // 状態遷移
        //-------------------------------------------------------------------------
        switch (gameStatus)
        {
            //-------------------------------------------------------------------------
            case GameStatus.Cleared:
                if (ScoreDialogImage.GetComponent<ScoreDialog>().OkFlag)
                {
                    ScoreDialogImage.GetComponent<ScoreDialog>().OkFlag = false;
                    score += ScoreDialogImage.GetComponent<ScoreDialog>().score;

                    InformationText.GetComponent<Text>().text = "";

                    gameStatus = GameStatus.Cleared1;
                    StartCoroutine("AnimationStageRopeBurning");
                }
                break;

            case GameStatus.Cleared1:
                generalCounter++;
                if (generalCounter >= 60)
                {
                    generalCounter = 0;
                    if (stageCount % rocketStagePeriod == 0)        // 5ステージ毎にロケット発射
                    {
                        gameStatus = GameStatus.ClearedRocket;
                    }
                    else
                    {
                        gameStatus = GameStatus.ClearedNormalStage;
                        TimeText.GetComponent<Text>().text = "TIME : " + timeRemain.ToString("0.000");
                        StageText.GetComponent<Text>().text = "STAGE : " + stageCount;
                    }

                }
                break;

            case GameStatus.ClearedRocket:
                generalCounter++;
                if (generalCounter == 100)
                {
                    Missile.transform.Find("RocketFire").gameObject.SetActive(true);
                    FireCracker.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
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
                if (generalCounter >= 150)
                {
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
                Playing();
                generalCounter++;
                if (generalCounter >= deltaChallengeTime)
                {
                    StageEndCommonProcedure();
                    generalCounter = 0;
                    life--;
                    if (life <= 0)
                    {
                        gameStatus = GameStatus.GameEnd;
                    }
                    else
                    {
                        GameOver();
                        gameStatus = GameStatus.GameOver;
                    }
                }
                break;

            case GameStatus.GameOver:
                const float deltaGameOverAnimationTime = 60;
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
                if (generalCounter >= 30)
                {
                    generalCounter = 0;
                    StartStage();
                    gameStatus = GameStatus.PreStart;
                    InformationText.GetComponent<Text>().text = "";
                    MainCamera.GetComponent<Animator>().Play("CameraBlurOff");
                }
                break;

            case GameStatus.GameEnd:
                generalCounter++;
                if (generalCounter >= 60)
                {
                    GameEnd();
                    gameStatus = GameStatus.GameEnd2;
                    generalCounter = 0;
                    RestartButton.SetActive(true);
                    TimeToRestartText.SetActive(true);
                    restartTime = DateTime.Now;
                    RestartButton.transform.GetChild(0).GetComponent<Text>().text = "動画を観てすぐに始める";
                }
                break;

            case GameStatus.GameEnd2:
                int TimeToStart = 10;   // minutes.
                generalCounter++;

                int newlife = 0;
                int remainTime = DateTime.Now.Subtract(restartTime).Minutes;
                TimeSpan ts = DateTime.Now - restartTime;   //DateTime の差が TimeSpan として返る
                {
                    if (TimeToStart * 60 * 1 <= ts.TotalSeconds)
                    {
                        newlife = 1;
                        RestartButton.transform.GetChild(0).GetComponent<Text>().text = "再スタート";
                    }
                    if (TimeToStart * 60 * 2 <= ts.TotalSeconds)
                    {
                        newlife = 2;
                    }
                    if (TimeToStart * 60 * 3 <= ts.TotalSeconds)
                    {
                        newlife = 3;
                        TimeToRestartText.SetActive(false);
                    }
                    string str = "ライフ復活まであと " + (TimeToStart - 1 - ((int)ts.TotalMinutes % TimeToStart)).ToString("00") + ":" + (60 - 1 - ((int)ts.TotalSeconds % 60)).ToString("00");
                    TimeToRestartText.GetComponent<Text>().text = str;

                    for (int i = 0; i < newlife; i++)
                    {
                        Remains[i].SetActive(true);
                    }
                    if (newlife > life)
                    {
                        PanelMovingParticle.SetActive(false);
                        PanelMovingParticle.SetActive(true);
                        PanelMovingParticle.transform.position = Remains[newlife - 1].transform.position;
                    }
                    life = newlife;
                }

                //if (lapsedTime <= 0)
                //{
                //    RestartButton.transform.GetChild(0).GetComponent<Text>().text = "再スタート";
                //    if (life <= 2)
                //    {
                //        life++;
                //        Remains[life - 1].SetActive(true);
                //        restartTime = DateTime.Now;
                //        PanelMovingParticle.SetActive(false);
                //        PanelMovingParticle.SetActive(true);
                //        PanelMovingParticle.transform.position = Remains[life - 1].transform.position;
                //    }
                //    if (life == 3)
                //    {
                //        TimeToRestartText.SetActive(false);
                //    }
                //}

                if (restartFlag == true)    // 再スタートボタンが押された
                {
                    if (life == 0) life = 1;        // とりあえずデバッグ用

                    restartFlag = false;
                    RestartButton.SetActive(false);
                    TimeToRestartText.SetActive(false);

                    generalCounter = 0;
                    StartStage();
                    gameStatus = GameStatus.PreStart;
                    InformationText.GetComponent<Text>().text = "";


                    //life = 3;
                    //Remains[0].SetActive(true);
                    //Remains[1].SetActive(true);
                    //Remains[2].SetActive(true);
                }
                break;

            case GameStatus.PreStart:
                generalCounter++;
                if (generalCounter >= 30)
                {
                    //FireCracker.GetComponent<Animator>().SetBool("FireCrackerAppearFlag", true);
                    generalCounter = 0;
                    gameStatus = GameStatus.Start;
                    PanelSubSystem.GetComponent<PanelObject>().SpeedUpFlag = false; // スピードダウン
                    StageText.GetComponent<Text>().text = "STAGE : " + stageCount;
                }
                break;

            case GameStatus.Start:
                generalCounter++;
                if (generalCounter >= 1)
                {
                    steps = 0;

                    Remains[life - 1].SetActive(false);
                    generalCounter = 0;
                    if (explosionPrefab != null)
                    {
                        Destroy(explosionPrefab);
                    }
                    Animator anime = MainCamera.GetComponent<Animator>();
                    anime.Play("GlowAnimation");
                    gameStatus = GameStatus.Play;
                                                                                    //FireCracker.GetComponent<Animator>().SetBool("FireCrackerAppearFlag", false);
                    Panel.PanelObject[currentX, currentY].GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.8f, 0.8f);  // 燃え中のパネル
                    pushButtonCount = 0;    // 強制的にプッシュをリリースする
                    PanelSubSystem.GetComponent<PanelObject>().StartBurning(Panel.PanelObject[currentX, currentY], FuseDirection.Left, Panel.Matrix[currentX, currentY]);
                    PanelSubSystem.GetComponent<PanelObject>().StartFlag = true;
                    FireCracker.transform.localScale = new Vector3(0.2f, 0.2f, 1f); // これ効果なかった(アニメーターが何故か走っていて優先される

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

    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    void Playing()
    {
        Vector3 clickPosition;
        int i = 0;
        int j = 0;
        float delta = (panelHeight + panelWidth) / 2f / 4f;    // 1/60秒ごとのパネル移動距離
        MoveDirection tapDirection = MoveDirection.None;

        TimeText.GetComponent<Text>().text = "TIME : " + timeRemain.ToString("0.000");

        timeRemain -= 1f / 60f;
        if (timeRemain <= 0f)
        {
            StageEndCommonProcedure();
            life--;
            if (life <= 0)
            {
                gameStatus = GameStatus.GameEnd;
            }
            else
            {
                GameOver("TIME OUT");
                gameStatus = GameStatus.GameOver;
            }
            timeRemain = 0f;

            return;
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
            JudgeFuseConnection();
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

                return;
            }
            else
            {
                PanelMovingParticle.SetActive(false);
                PanelMovingParticle.SetActive(true);
                PanelMovingParticle.transform.position = tapObject.transform.position;
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
                    JudgeFuseConnection();
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

                                    return;
                                }
                                else if (tapObject.name.Substring(0, 5) == "Plain")   // "PlainPanel"
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
                    return;
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
        return;

        FOUND_OBJECT:
        PanelType temp;

        Debug.Log("x:" + i + " y:" + j);

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

                DebugStatus();

                return;
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

                DebugStatus();

                return;

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

                DebugStatus();

                return;

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

                DebugStatus();

                return;

            }
        }

        PanelDirection = MoveDirection.None;
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
    public void JudgeFuseConnection()
    {
        PanelType currentType = Panel.Matrix[currentX, currentY];
        FuseDirection nextStartDirection = FuseDirection.Up;    // Just in case.
        bool challengeTimeFlag;
        long tempx, tempy;

        if ((PanelDirection != MoveDirection.None) && (MovingPanel != null))    // パネル移動アニメーション中か？
        {
            return;
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
                    return;
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

        DebugStatus1(nextStartDirection);

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
        Animator anime = MainCamera.GetComponent<Animator>();
        anime.Play("CameraBlur");

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
    }

    void DebugStatus()
    {
        return;
        Debug.Log("CurrentX:" + currentX + "y:" + currentY);

    }
    void DebugStatus1(FuseDirection nextStartDirection)
    {
        return;
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
        Animator anime = MainCamera.GetComponent<Animator>();
        anime.Play("CameraBlur");
        InformationText.GetComponent<Animation>().Play();

        explosionPrefab = Instantiate(Explosion, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f)) as GameObject;

        for (int i = 0; i < Panel.Size; i++)
        {
            for (int j = 0; j < Panel.Size; j++)
            {
                //Panel.PanelObject[i, j].GetComponent<Rigidbody2D>().gravityScale = 3f;
                //Panel.PanelObject[i, j].GetComponent<Rigidbody2D>().AddForce(new Vector2(((float)i - Panel.Size / 2f) * 40f, ((float)j - Panel.Size / 2f) * 40f));
                //float turn = Input.GetAxis("Horizontal");
                //Panel.PanelObject[i, j].GetComponent<Rigidbody2D>().AddRelativeForce.AddRelativeTorque(float x, float y, Inpulse ForceMode mode = ForceMode.Force);
                //float turn = Input.GetAxis("Vertical");
                //Panel.PanelObject[i, j].GetComponent<Rigidbody2D>().AddTorque(transform.up * torque * turn);
                //float turn = Input.GetAxis("Horizontal");
                //Panel.PanelObject[i, j].GetComponent<Rigidbody2D>().AddTorque(transform.up * torque * turn);
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().useGravity = true;
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().AddForce(new Vector3(((float)i - Panel.Size / 2f) * 40f, ((float)j - Panel.Size / 2f) * 40f, 0));
                Panel.PanelObject[i, j].GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f));

            }
        }
        Missile.GetComponent<Rigidbody>().useGravity = true;
        Missile.GetComponent<Rigidbody>().AddForce(new Vector3(200f, 120f, 0f));
        Missile.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f, UnityEngine.Random.value * 100f));


    }


    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    void StageClear()
    {
        TimeText.GetComponent<Animation>().Stop();
        TimeText.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f);
        timeTextAnimationFlag = false;

        //        FireCracker.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        ScoreDialogImage.GetComponent<ScoreDialog>().StartDialogAnimation(timeRemain, steps);   // スコアダイアログを表示

        InformationText.SetActive(true);
        InformationText.GetComponent<Text>().text = "STAGE " + stageCount + " CLEAR";
        InformationText.GetComponent<Animation>().Play();

        // StageClearParticle.SetActive(true);

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
    void Restart()
    {
        restartFlag = true;
    }

    /// <summary>
    /// //////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimationStageRopeBurning()
    {
        Vector3 deltax = new Vector3(panelWidth * 11f / stageToStageFireRope.Length, 0f, 0f);

        FireCracker.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

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

            yield return new WaitForEndOfFrame();

        }
        yield break;

    }

}


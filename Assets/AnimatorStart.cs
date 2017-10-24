using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimatorStart : MonoBehaviour {
    public GameObject City1;
    public GameObject City2;
    public GameObject City3;
    public GameObject City4;
    public GameObject City5;
    public GameObject TargetMark;
    public GameObject InfoText;
    public GameObject MapObject;
    public GameObject TargetText;
    public GameObject TimeText;
    public new Camera camera;
    public GameObject SkipButton;
    public GameObject FadeOutPanel;
    public GameObject AlertText;

    private int animationCounter;
    private LineRenderer LineDrawer;
    private bool gameStartFlag;
    private int stageCount;

    private bool alertFlag;

    // Use this for initialization
    void Start () {
        // gameObject.GetComponent<Animator>().Play("TargetCityAnimation");
        animationCounter = -1;
        LineDrawer = gameObject.GetComponent<LineRenderer>();
        gameStartFlag = false;
        stageCount = 1;

        alertFlag = false;

        TargetText.GetComponent<Text>().text = "";

        Button btn = SkipButton.GetComponent<Button>();
        btn.onClick.AddListener(SkipAnimation);

        FadeOutPanel.GetComponent<Image>().color = new Color(0.0f, 0f, 0f, 1f);

        City1.GetComponent<Animation>().Play();

        StartCoroutine("DisplayAlertText");

    }

    // Update is called once per frame
    void Update () {

        if (alertFlag)
        {
            alertFlag = false;
            gameObject.GetComponent<Animation>().Play("OpeningAnimation");

            StartCoroutine("DisplayText");
            StartCoroutine("DisplayTimeText");
        }

        if (animationCounter >= 0)
        {
            animationCounter++;
        }
        
        if (gameStartFlag)
        {
            gameStartFlag = true;
        }


        switch (animationCounter)
        {
            case 20:
                StartCoroutine("DrawLineAndTargetText");
                break;

            case 120:
                break;

            default:
                break;
        }
	}

    /// <summary>
    /// 
    /// </summary>
    void SkipAnimation()
    {
        gameStartFlag = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="pos3"></param>
    /// <param name="alpha"></param>
    void DrawLine(Vector3 pos1, Vector3 pos2, Vector3 pos3, float alpha = 1f)
    {
        //const float zdepth = -3.0f;     // カメラに近付けないとスプライトの裏に隠れてしまう

        LineDrawer.positionCount = 3;
        //LineDrawer.SetWidth(.positionCount = Size;
        LineDrawer.SetPosition(0, pos1);
        LineDrawer.SetPosition(1, pos2);
        LineDrawer.SetPosition(2, pos3);

        LineDrawer.GetComponent<LineRenderer>().material.SetColor("_Color", new Color(1.0f, 0f, 0f, alpha));
    }


    /// <summary>
    /// 
    /// </summary>
    void OpeningAnimationEnd()
    {
        TargetMark.GetComponent<Animation>().Play("TargetMark");
        animationCounter = 0;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayAlertText()
    {
        bool[] pattern = { true, false, true, false, true, false };
        
        foreach(bool onoff in pattern)
        {
            AlertText.SetActive(onoff);
            yield return new WaitForSeconds(0.5f);

            AlertText.SetActive(onoff);

            //yield return new WaitForSeconds(0.5f);
            //AlertText.SetActive(false);
            //yield return new WaitForSeconds(0.5f);
            //AlertText.SetActive(true);
            //yield return new WaitForSeconds(0.5f);
            //AlertText.SetActive(false);
            //yield return new WaitForSeconds(0.5f);
            //AlertText.SetActive(true);
            //yield return new WaitForSeconds(0.5f);
            //AlertText.SetActive(false);
            //yield return new WaitForSeconds(0.5f);

            if (gameStartFlag == true)
            {

                for (float i = 0; i < 1f; i += 0.02f)
                {
                    //camera.orthographicSize /= 1.1f;
                    FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, i);

                    yield return new WaitForEndOfFrame();
                }
                gameStartFlag = false;
                SceneManager.LoadScene("GameMain");

                yield break;
            }
        }
        alertFlag = true;

        for (float i = 1; i > 0f; i -= 0.04f)
        {
            FadeOutPanel.GetComponent<Image>().color = new Color(0.1f, 0f, 0f, i);
            yield return new WaitForEndOfFrame();
        }
        yield break;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayText()
    {
        int counter = 0;

        while (true)
        {
            counter++;

            InfoText.GetComponent<Text>().text = "[TARGET LOCATION]" +
            "\nLongitude : " + Mathf.Abs(MapObject.transform.position.x) +
            "\nLatitude : " + Mathf.Abs(MapObject.transform.position.y) +
            "\nAltitude : " + Mathf.Abs(gameObject.transform.localScale.x - 0.00001f);

            InfoText.GetComponent<Text>().text += "\n\nSEARCHING ...";
            if (animationCounter < 0)
            {
                for (int i = 0; i < counter / 10; i++)
                {
                    InfoText.GetComponent<Text>().text += ".";
                }
            }
            if (animationCounter > 0)
            {
                InfoText.GetComponent<Text>().text += "\nTARGET FOUND.";
            }
            if (animationCounter > 90)
            {
                InfoText.GetComponent<Text>().text += "\nARMED AND READY.";
            }
            if (animationCounter > 100)
            {
                InfoText.GetComponent<Text>().text += "\n" + MapObject.transform.position.x / 5.3f + "DEG.";
            }
            if (animationCounter > 105)
            {
                InfoText.GetComponent<Text>().text += "\n" + MapObject.transform.position.y / 5.3f + "DEG.";
            }
            if (animationCounter > 100)
            {
                InfoText.GetComponent<Text>().text += "\n" + 5.3f + "DEG.";
            }
            if (animationCounter > 110)
            {
                InfoText.GetComponent<Text>().text += "\n" + MapObject.transform.position.x / 33.3f + "";
            }
            if (animationCounter > 112)
            {
                InfoText.GetComponent<Text>().text += "\n" + MapObject.transform.position.y / 33.3f + "";
            }
            if (animationCounter > 120)
            {
                InfoText.GetComponent<Text>().text += "\n" + (MapObject.transform.position.z + 5000f) / 44.3f + "";
            }
            if (animationCounter > 122)
            {
                InfoText.GetComponent<Text>().text += "\n" + MapObject.transform.position.x / 44.3f + "KHZ";
            }
            if (animationCounter > 132)
            {
                InfoText.GetComponent<Text>().text += "\n" + MapObject.transform.position.y / 44.3f + "";
            }
            if (animationCounter > 135)
            {
                InfoText.GetComponent<Text>().text += "\n" + (MapObject.transform.position.z + 100000f) / 5.3f + "";
            }
            if (animationCounter > 140)
            {
                InfoText.GetComponent<Text>().text += "\nLAUNCHER ANGLE ADJUSTED.";
            }
            if (animationCounter > 180)
            {
                InfoText.GetComponent<Text>().text += "\n\nDone.";
            }

            if ((counter % 10) < 5)
            {
                InfoText.GetComponent<Text>().text += "_";
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DrawLineAndTargetText()
    {
        int counter = 0;
        Vector3 pos1 = new Vector3(0f, 0f, -0.1f);
        Vector3 pos2 = new Vector3(-1f, -3f, -0.1f);
        Vector3 pos3 = new Vector3(2.5f, -3f, -0.1f);

        LineDrawer.GetComponent<LineRenderer>().material.SetColor("_Color", new Color(1.0f, 0f, 0f, 0.8f));

        // ターゲットからの引き出し線を描く
        //DrawLine(new Vector3(0, 0, -3f), new Vector3(-1f, -1.5f, -3f), new Vector3(1.5f, -1.5f, -3f));
        for (int i = 0; i < 8; i++)
        {
            LineDrawer.positionCount = i + 1;
            LineDrawer.SetPosition(i, pos1 + ((pos2 - pos1) / 8f * i));

            yield return new WaitForEndOfFrame();
        }
        for (int i = 8; i < 16; i++)
        {
            LineDrawer.positionCount = i + 1;
            LineDrawer.SetPosition(i, pos2 + ((pos3 - pos2) / 8f * (i - 8)));

            yield return new WaitForEndOfFrame();
        }

        // ターゲットにターゲット名を一文字ずつ書く
        float interval = 0.05f;
        TargetText.GetComponent<Text>().text = "";

        string str;


        switch (stageCount)
        {
            case 1:
                str = "TARGET :\nSOUTH ISLAND";
                break;

            case 11:
                str = "TARGET :\nTRICOLOR";
                break;

            case 21:
                str = "TARGET :\nSOUTH KOREA";
                break;

            case 31:
                str = "TARGET :\nJAPON";
                break;

            case 41:
                str = "TARGET :\nTHE UNITED COUNTRY";
                break;

            default:
                str = "     ";
                break;
        }

        string temp = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '\n')     // TARGET表示で一旦インターバル
            {
                counter = 0;
                for (int j = 0; j < 90; j++)
                {
                    counter++;
                    if ((counter % 10) < 5)
                    {
                        TargetText.GetComponent<Text>().text = temp;
                    }
                    else
                    {
                        TargetText.GetComponent<Text>().text = temp + "_";
                    }

                    yield return new WaitForEndOfFrame();
                }
                interval = 0.15f;
            }
            temp += str[i];
            TargetText.GetComponent<Text>().text = temp + "_";

            yield return new WaitForSeconds(interval);
        }

        counter = 0;
        while(true)
        {
            counter++;
            if ((counter % 10) < 5)
            {
                TargetText.GetComponent<Text>().text = str;
            }
            else
            {
                TargetText.GetComponent<Text>().text = str + "_";
            }

            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayTimeText()
    {
        int counter = (int)(UnityEngine.Random.value * 10000f + 10000f);

        // ここでキー入力を待つ
        int sec = 12;
        for (int j = 0; j < 60 * sec; j++)
        {
            counter++;
            TimeText.GetComponent<Text>().text = "TIME LAPSED : " + counter;

            if (gameStartFlag == true)
            {

                for (float i = 0; i < 1f; i += 0.02f)
                {
                    //camera.orthographicSize /= 1.1f;
                    FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, i);

                    yield return new WaitForEndOfFrame();
                }
                gameStartFlag = false;
                SceneManager.LoadScene("GameMain");

                yield break;
            }

            yield return new WaitForEndOfFrame();
        }

        // Auto start
        float seconds = 0.5f;
        for (float i = 0; i < 1f; i += 1f / (seconds * 60))
        {
            // camera.orthographicSize /= 1.1f;
            FadeOutPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, i);
            camera.orthographicSize *= 0.96f;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("GameMain");

        yield break;
    }
}


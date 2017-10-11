using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreDialog : MonoBehaviour {

    public GameObject OkButton;
    public GameObject ShareButton;
    public GameObject HomeButton;
    public GameObject ScoreText;
    public GameObject StepText;
    public GameObject RemainTimeText;
    public GameObject ExcellentText;
    public GameObject StageClearParticle;
    public GameObject HiScoreText;
    public GameObject PanelSystemObject;

    private int excellentScore;
    private int currentHiScore;

    public int score { get; set; }
    public float remainTime { get; set; }
    public int clearLevel { get; set; }
    public int steps { get; set; }

    // Use this for initialization
    void Start () {
        //remainTime = 0;
        //steps = 0;

        Button btn = OkButton.GetComponent<Button>();
        btn.onClick.AddListener(GoNext);
        Button btn1 = ShareButton.GetComponent<Button>();
        btn1.onClick.AddListener(Share);
        Button btn2 = HomeButton.GetComponent<Button>();
        btn2.onClick.AddListener(GoHome);

        btn1.transform.GetChild(0).GetComponent<Text>().text = TextResources.Text[(int)TextNumber.EVALUATE_THIS_APP, (int)GlobalVariables.Language];
    }

    // Update is called once per frame
    void Update () {
		

	}

    /// <summary>
    /// 
    /// </summary>
    void GoHome()
    {
        PanelSystemObject.GetComponent<PanelSystem>().GoHome();
    }

    /// <summary>
    /// 
    /// </summary>
    void GoNext()
    {
        PanelSystemObject.GetComponent<PanelSystem>().GoNext();
    }

    /// <summary>
    /// 
    /// </summary>
    void Share()
    {
        PanelSystemObject.GetComponent<PanelSystem>().Share();
    }

    /// <summary>
    /// 
    /// </summary>
    public void StartDialogAnimation(float remaintime, int step, int excellentscore, int currenthiscore)
    {
        score = 0;

        HomeButton.SetActive(false);
        OkButton.SetActive(false);
        ShareButton.SetActive(false);
        gameObject.SetActive(true);

        steps = step;
        remainTime = remaintime;
        excellentScore = excellentscore;
        currentHiScore = currenthiscore;

        StartCoroutine("AnimationOkDialog");

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimationOkDialog()
    {
        gameObject.transform.localScale = new Vector3(0f, 0f, 0f);

        yield return new WaitForSeconds(1.6f);

        ScoreText.GetComponent<Text>().text = "SCORE : 0";
        RemainTimeText.GetComponent<Text>().text = "TIME : " + remainTime.ToString("0.000");
        StepText.GetComponent<Text>().text = "STEPS : " + steps;
        ExcellentText.SetActive(false);
        HiScoreText.SetActive(false);

        for (float i = 5f; i >= 1f; i -= 0.33f)
        {
            gameObject.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForEndOfFrame();
        }
        for (float i = 1f; i <= 1.1f; i += 0.02f)
        {
            gameObject.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForEndOfFrame();
        }
        for (float i = 1.1f; i >= 1f; i -= 0.02f)
        {
            gameObject.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForEndOfFrame();
        }

        const int SCORE_RATIO = 10;
        for (float i = remainTime; i >= 0f; i -= 0.2f)
        {
            //if (steps > MAX_STEP)
            //{
            //    steps = MAX_STEP;
            //}
            // score += (MAX_STEP - steps) * 10;
            score += (SCORE_RATIO * 10 / (steps + SCORE_RATIO)) * 10;
            ScoreText.GetComponent<Text>().text = "SCORE : " + score;
            RemainTimeText.GetComponent<Text>().text = "TIME : " + i.ToString("0.000");

            bool push = Input.GetMouseButton(0);
            if (!push)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        ScoreText.GetComponent<Text>().text = "SCORE : " + score;
        RemainTimeText.GetComponent<Text>().text = "TIME : " + 0f.ToString("0.000");

        OkButton.SetActive(true);
        ShareButton.SetActive(true);
        HomeButton.SetActive(true);

        if ((score > excellentScore) || (score > currentHiScore))  
        {
            if (score > excellentScore)
            {
                StageClearParticle.SetActive(false);
                StageClearParticle.SetActive(true);
                ExcellentText.SetActive(true);

            }
            if (score > currentHiScore)
            {
                HiScoreText.SetActive(true);
            }

            // StartCoroutine("AnimationExcellentText");
            gameObject.GetComponent<Animation>().Play("ScoreDialogAnimation");
            clearLevel = 1;
        }
        else
        {
            clearLevel = 0;
        }


        yield break;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimationExcellentText()
    {
        for (float i = 1f; i <= 1.6f; i += 0.05f)
        {

            ExcellentText.transform.localScale = new Vector3(i, i, i);
            ExcellentText.transform.rotation = Quaternion.Euler(0f, 0f, (i - 1) * 10f);

            yield return new WaitForEndOfFrame();
        }
        for (float i = 1.6f; i >= 1.0f; i -= 0.05f)
        {

            ExcellentText.transform.localScale = new Vector3(i, i, i);
            ExcellentText.transform.rotation = Quaternion.Euler(0f, 0f, (i - 1) * 10f);

            yield return new WaitForEndOfFrame();
        }

        yield break;

    }

}


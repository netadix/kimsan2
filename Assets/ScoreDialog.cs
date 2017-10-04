using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDialog : MonoBehaviour {

    public GameObject OkButton;
    public GameObject ShareButton;
    public GameObject DialogImage;
    public GameObject ScoreText;
    public GameObject StepText;
    public GameObject RemainTimeText;

    private float remainTime;
    private int steps;

    public int score { get; set; }
    public bool OkFlag { get; set; }

    // Use this for initialization
    void Start () {
        OkFlag = false;
        //remainTime = 0;
        //steps = 0;

        Button btn = OkButton.GetComponent<Button>();
        btn.onClick.AddListener(OkToNextStage);
    }

    // Update is called once per frame
    void Update () {
		

	}

    /// <summary>
    ///
    /// </summary>
    void OkToNextStage()
    {
        OkFlag = true;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void StartDialogAnimation(float remaintime, int step)
    {
        score = 0;

        OkButton.SetActive(false);
        ShareButton.SetActive(false);
        gameObject.SetActive(true);

        steps = step;
        remainTime = remaintime;

        Debug.Log("step " + steps);
        Debug.Log("remainTime " + remainTime);

        StartCoroutine("AnimationOkDialog");

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimationOkDialog()
    {
        DialogImage.transform.localScale = new Vector3(0f, 0f, 0f);

        yield return new WaitForSeconds(1.6f);

        ScoreText.GetComponent<Text>().text = "SCORE : 0" + score;
        RemainTimeText.GetComponent<Text>().text = "TIME : " + remainTime.ToString("0.000");
        StepText.GetComponent<Text>().text = "STEPS : " + steps;

        for (float i = 10f; i >= 1f; i -= 0.25f) {
        
            DialogImage.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForEndOfFrame();
        }
        for (float i = 1f; i <= 1.1f; i += 0.02f)
        {

            DialogImage.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForEndOfFrame();
        }
        for (float i = 1.1f; i >= 1f; i -= 0.02f)
        {

            DialogImage.transform.localScale = new Vector3(i, i, i);

            yield return new WaitForEndOfFrame();
        }

        const int MAX_STEP = 50;
        for (float i = remainTime; i >= 0f; i -= 0.1f)
        {
            if (steps > MAX_STEP)
            {
                steps = MAX_STEP;
            }
            score += MAX_STEP - steps;
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


        yield break;

    }
}


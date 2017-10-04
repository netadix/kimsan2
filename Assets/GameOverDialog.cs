using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDialog : MonoBehaviour
{

    public GameObject OkButton;
    public GameObject ShareButton;
    public GameObject HomeButton;
    public GameObject PanelSystemObject;

    public int score { get; set; }
    public float remainTime { get; set; }
    public int clearLevel { get; set; }
    public int steps { get; set; }
    public bool OkFlag { get; set; }

    // Use this for initialization
    void Start()
    {
        OkFlag = false;

        Button btn = OkButton.GetComponent<Button>();
        btn.onClick.AddListener(Restart);
        Button btn1 = ShareButton.GetComponent<Button>();
        btn1.onClick.AddListener(Share);
        Button btn2 = HomeButton.GetComponent<Button>();
        btn2.onClick.AddListener(GoHome);
    }

    // Update is called once per frame
    void Update()
    {


    }

    /// <summary>
    /// 
    /// </summary>
    void GoHome()
    {
        bool ScoreSaved = false;
        PanelSystemObject.GetComponent<PanelSystem>().GoHome(ScoreSaved);
    }

    /// <summary>
    /// 
    /// </summary>
    void Restart()
    {
        PanelSystemObject.GetComponent<PanelSystem>().Restart();
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
    public void StartDialogAnimation()
    {
        gameObject.SetActive(true);

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

        yield break;

    }


}


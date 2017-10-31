using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningExplanation : MonoBehaviour {

    public new Camera camera;
    public GameObject SkipButton;
    public GameObject FadeOutPanel;
    public GameObject ExplanationText;
    public GameObject KimsanObject;

    private int animationCounter;
    private Stage stageCount;
    private string[] explanation;
    private bool animationEndFlag;

    // Use this for initialization
    void Start()
    {
        animationEndFlag = false;

        stageCount = Stage.S_49;

        switch (stageCount)
        {
            case Stage.S_49:
                explanation = new string[]
                {
                    "世界の面前でこの私と国家の存在を否定し、わが共和国をなきものにするという歴代で最も暴悪な宣戦布告をしてきた以上\nわれわれもそれに見合う史上最高の超強硬対応措置断行を慎重に考慮した。\n\n",
                    "幾度も実験を重ねて完成した、本物のチャーハンの味を見せてやろう。\n\n",
                    "ロケットの発射準備を始めろ。\n",
                };
                break;

            case Stage.S_99:
                explanation = new string[]
                {
                    "ハハハ。\n手始めに南の都市を火の海にし、同胞たちを無慈悲に粉砕した。\n\n",
                    "次に歴代最も暴悪な宣戦布告であり、史上最高の超強硬対応措置の断行を慎重に考慮する。",
                };
                break;

            case Stage.S_149:
                explanation = new string[]
                {
                    "「わが共和国が、世界が見たことがないような火力と怒りに直面するだろう」とは面白い。\n",
                    "愚かな国の無慈悲に粉砕する。容赦なき火の洗礼。無慈悲な報復。史上最高の超強硬的軍事打撃行動。",
                    "容赦なき火の洗礼。無慈悲な報復。史上最高の超強硬的軍事打撃行動。",
                };
                break;
        }


        animationCounter = -1;


        Button btn = SkipButton.GetComponent<Button>();
        btn.onClick.AddListener(SkipAnimation);

        FadeOutPanel.GetComponent<Image>().color = new Color(0.0f, 0f, 0f, 1f);

        StartCoroutine("TextAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        if (animationEndFlag)
        {
            SkipAnimation();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SkipAnimation()
    {
        SceneManager.LoadScene("OpeningAnimation");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator TextAnimation()
    {
        int count;
        string sentence = "";
        bool key = false;

        foreach (string str in explanation)
        {
            count = 0;
            while (str.Length > count)
            {
                sentence += str[count];
                ExplanationText.GetComponent<Text>().text = sentence;
                count++;

                bool push = Input.GetMouseButton(0);

                if (push == true)
                {
                    continue;
                }
                else
                {
                    yield return new WaitForSeconds(0.03f);
                }
            }
            yield return new WaitForEndOfFrame();   // 1フレーム待つ

            key = true;
            for (int i = 0; i < 90; i++)
            {
                if (Input.GetMouseButton(0) == true)    // コルーチンの中ではGetMouseButtonDown()が使えないため
                {
                    if (key == false)
                    {
                        key = true;
                        break;
                    }
                    key = true;
                }
                else
                {
                    key = false;
                }

                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();   // 1フレーム待つ
        }
        yield return new WaitForSeconds(1.5f);

        animationEndFlag = true;

        yield break;
    }
}


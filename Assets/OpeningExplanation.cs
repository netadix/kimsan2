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
    private int stageCount;
    private  string[] explanation;

    // Use this for initialization
    void Start()
    {
        explanation = new string[]
        {
            "世界の面前でこの私と国家の存在を否定し、\nわが共和国をなきものにするという歴代で最も暴悪な宣戦布告をしてきた以上\nわれわれもそれに見合う史上最高の超強硬対応措置断行を慎重に考慮した。\n",
            "幾度も実験を重ねて完成した、本物のチャーハンの味を見せてやろう。\n",
            "ロケットを打ち上げるのだ。\n",

            "手始めに南の都市を火の海にし、\n同胞たちを無慈悲に粉砕した。\n",
            "次に歴代最も暴悪な宣戦布告であり、史上最高の超強硬対応措置の断行を慎重に考慮する。",

            "「わが共和国が、世界が見たことがないような火力と怒りに直面するだろう」とは面白い。\n",
            "愚かな国の無慈悲に粉砕する。容赦なき火の洗礼。無慈悲な報復。史上最高の超強硬的軍事打撃行動。",
            "容赦なき火の洗礼。無慈悲な報復。史上最高の超強硬的軍事打撃行動。",
        };


        animationCounter = -1;
        stageCount = 1;


        Button btn = SkipButton.GetComponent<Button>();
        btn.onClick.AddListener(SkipAnimation);

        FadeOutPanel.GetComponent<Image>().color = new Color(0.0f, 0f, 0f, 1f);


    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    void SkipAnimation()
    {

    }
}


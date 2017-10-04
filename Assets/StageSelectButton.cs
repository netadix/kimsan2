using UnityEngine;
using System;

public class MyButton : MonoBehaviour
{
    public void ButtonClick()
    {
        int stageCount = 0;

        string str = gameObject.transform.Find("text").transform.name.Substring(5, 3);
        Int32.TryParse(str, out stageCount);
        PlayerPrefs.SetInt("CURRENT_STAGE", stageCount);

    }
}
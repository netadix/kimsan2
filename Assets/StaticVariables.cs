using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//public enum PanelType
//{
//    Nothing = 0,
//    StraightVertical = 1,
//    StraightHorizontal,
//    CurveLeftUp,
//    CurveUpRight,
//    CurveRightDown,
//    CurveDownLeft,
//    CurveLeftUp_RightDown,
//    CurveDownLeft_UpRight
//}

//public enum FuseDirection
//{
//    Up = 0,
//    Right,
//    Down,
//    Left
//}

/// <summary>
/// 
/// </summary>
static class GlobalVariables
{
    static public DateTime RestartTime;
    static public int TimeToStart = 10;//10; // minutes
    static public int Life;
    static readonly public int LifeMax = 3;

    static public LanguageType Language;
    static public string LanguageFont;
    static public int FireSpeed = 10;
} 

/// <summary>
/// 
/// </summary>
enum LanguageType
{
    Japanese = 0,
    English = 1,
    Other = 3
}

enum TextNumber
{
    HOME = 0,
    STAGE,
    STAGE_SELECT,
    START,
    RANKING,
    FIRE,
    SKIP,
    TIME,
    SPEED_UP,

    //-------------------------------------------------
    TIME_TO_RESTART,
    WATCH_MOVIE_TO_START,
    CHARACTER_LESS_THAN_8,
    EVALUATE_THIS_APP,


}
/// <summary>
/// 
/// </summary>
static public class TextResources
{
    static public string[,] Text =
    {
        { "HOME", "HOME" , ""},
        { "STAGE", "STAGE" , ""},
        { "STAGE SELECT", "STAGE SELECT" , ""},
        { "START", "START" , ""},
        { "RANKING", "RANKING" , ""},
        { "FIRE", "FIRE" , ""},
        { "SKIP", "SKIP" , ""},
        { "TIME", "TIME" , ""},
        { "SPEED UP", "SPEED UP" , ""},
        //-------------------------------------------------
        // この方法でローカライズするのはしんどいので次回からはツール使う（今回は力技で対応）
        { "ライフ復活まであと ", "TIME TO RESTART AFTER ", "" },
        { "動画を観てすぐにスタート", "WATCH MOVIE TO START", "" },
        { "8文字以内で入力せよ！", "NAME MUST BE LESS THAN 8 CHARACTER!", "" },
        { "このゲームを評価", "EVALUATE THIS GAME", "" },
    };
}


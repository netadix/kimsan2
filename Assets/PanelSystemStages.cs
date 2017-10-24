//#define DRAG_CONTROL
#define PUSH_CONTROL


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    S_50,
    S_51,
    S_52,
    S_53,
    S_54,
    S_55,
    S_56,
    S_57,
    S_58,
    S_59,
    S_60,
    S_61,
    S_62,
    S_63,
    S_64,
    S_65,
    S_66,
    S_67,
    S_68,
    S_69,
    S_70,
    S_71,
    S_72,
    S_73,
    S_74,
    S_75,
    S_76,
    S_77,
    S_78,
    S_79,
    S_80,
    S_81,
    S_82,
    S_83,
    S_84,
    S_85,
    S_86,
    S_87,
    S_88,
    S_89,
    S_90,
    S_91,
    S_92,
    S_93,
    S_94,
    S_95,
    S_96,
    S_97,
    S_98,
    S_99,
    S_100,
    S_101,
    S_102,
    S_103,
    S_104,
    S_105,
    S_106,
    S_107,
    S_108,
    S_109,
    S_110,
    S_111,
    S_112,
    S_113,
    S_114,
    S_115,
    S_116,
    S_117,
    S_118,
    S_119,
    S_120,
    S_121,
    S_122,
    S_123,
    S_124,
    S_125,
    S_126,
    S_127,
    S_128,
    S_129,
    S_130,
    S_131,
    S_132,
    S_133,
    S_134,
    S_135,
    S_136,
    S_137,
    S_138,
    S_139,
    S_140,
    S_141,
    S_142,
    S_143,
    S_144,
    S_145,
    S_146,
    S_147,
    S_148,
    S_149,
    S_150,
    S_151,
    S_152,
    S_153,
    S_154,
    S_155,
    S_156,
    S_157,
    S_158,
    S_159,
    S_160,
    S_161,
    S_162,
    S_163,
    S_164,
    S_165,
    S_166,
    S_167,
    S_168,
    S_169,
    S_170,
    S_171,
    S_172,
    S_173,
    S_174,
    S_175,
    S_176,
    S_177,
    S_178,
    S_179,
    S_180,
    S_181,
    S_182,
    S_183,
    S_184,
    S_185,
    S_186,
    S_187,
    S_188,
    S_189,
    S_190,
    S_191,
    S_192,
    S_193,
    S_194,
    S_195,
    S_196,
    S_197,
    S_198,
    S_199,
    S_200,
    S_201,
    S_202,
    S_203,
    S_204,
    S_205,
    S_206,
    S_207,
    S_208,
    S_209,
    S_210,
    S_211,
    S_212,
    S_213,
    S_214,
    S_215,
    S_216,
    S_217,
    S_218,
    S_219,
    S_220,
    S_221,
    S_222,
    S_223,
    S_224,
    S_225,
    S_226,
    S_227,
    S_228,
    S_229,
    S_230,
    S_231,
    S_232,
    S_233,
    S_234,
    S_235,
    S_236,
    S_237,
    S_238,
    S_239,
    S_240,
    S_241,
    S_242,
    S_243,
    S_244,
    S_245,
    S_246,
    S_247,
    S_248,
    S_249,
    S_250,

    S_MAX
}

/// <summary>
/// ///////////////////////////////////////////////////////////////////////
/// </summary>
public partial class PanelSystem
{

    /// <summary>
    /// //////////////////////////////////////////////////////////////
    /// </summary>
    partial void StageData(int stageNum)
    {
        const int TIME_RATIO = 300;

        GlobalVariables.FireSpeed = 10;     // デフォルトのスピード　1/6[dot/sec.] 16dot/panelなので2.6秒で1パネルが燃える

        switch ((Stage)stageNum)
        {
            case Stage.S_49:
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Up;
//                nextStartDirection = FuseDirection.Left;
                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;
                excellentScore = (int)timeRemain * TIME_RATIO;
                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Disabled,
                    PanelType.Disabled,
                    PanelType.Disabled,
                    PanelType.CurveLeftUp,

                     // X 2列目
                    PanelType.Disabled,
                    PanelType.Disabled,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,

                     // X 3列目
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveDownLeft,
                    PanelType.Disabled,

                     // X 4列目
                    PanelType.Disabled,
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.Disabled,
                    PanelType.Disabled,

        };
        break;

        case Stage.S_1:     // Stage1
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;
                excellentScore = (int)timeRemain * TIME_RATIO;
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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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
                excellentScore = (int)timeRemain * TIME_RATIO;

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

            case Stage.S_22:     // Stage1 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;
                excellentScore = (int)timeRemain * TIME_RATIO;
                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                    // X 2列目
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.Nothing,

                    // X 3列目
                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.Nothing,

                    // X 4列目
            //      PanelType.CurveLeftUp,
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
            //      PanelType.CurveDownLeft,
                    PanelType.CurveRightDown,
            //      PanelType.CurveDownLeft,
                    PanelType.Nothing,

                    };
                break;

            case Stage.S_23:     // Stage2 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.StraightVertical,
                    PanelType.StraightVertical,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft,

                    // X 2列目
                    PanelType.StraightVertical,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,

                    // X 3列目
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.CurveDownLeft_UpRight,

                    // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.StraightVertical,
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_24:     // Stage3 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;
                excellentScore = (int)timeRemain * TIME_RATIO;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                // X 1列目
                PanelType.StraightVertical,
                PanelType.StraightVertical,
                PanelType.StraightVertical,
                PanelType.StraightHorizontal,

                // X 2列目
                PanelType.CurveLeftUp_RightDown,
                PanelType.StraightVertical,
                PanelType.CurveLeftUp_RightDown,
                PanelType.StraightHorizontal,

                // X 3列目
                PanelType.StraightVertical,
                PanelType.Nothing,
                PanelType.CurveLeftUp,
                PanelType.CurveDownLeft,

                // X 4列目
                PanelType.CurveUpRight,
                PanelType.StraightVertical,
                PanelType.StraightHorizontal,
                PanelType.StraightHorizontal,

            };
                break;

            case Stage.S_25:     // Stage4 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;
                excellentScore = (int)timeRemain * TIME_RATIO;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;

                stage = new PanelType[]
                {
                    // X 1列目
//                    PanelType.CurveLeftUp,//bug
                    PanelType.CurveLeftUp,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                     // X 2列目
//                    PanelType.CurveUpRight,
//                    PanelType.StraightVertical,
//                    PanelType.StraightVertical,
//                    PanelType.CurveDownLeft,

                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,


                     // X 3列目
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,

                     // X 4列目
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.Nothing,
                    PanelType.CurveUpRight,

                };
                break;

            case Stage.S_26:     // Stage5 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.CurveDownLeft,

                     // X 2列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.CurveRightDown,

                     // X 3列目
                    PanelType.CurveDownLeft_UpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                     // X 4列目
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.CurveDownLeft,
                    PanelType.Nothing,
                    PanelType.Nothing,

                };
                break;


            case Stage.S_27:     // Stage 6 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Down;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 20f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveDownLeft,

                     // X 2列目
                    PanelType.CurveUpRight,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.CurveRightDown,

                     // X 3列目
                    PanelType.CurveLeftUp,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveDownLeft,

                     // X 4列目
                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_28:     // Stage 7 bug  uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Up;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 60f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,//bug

                     // X 2列目
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp ,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,

                     // X 3列目
                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                     // X 4列目
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,

                };
                break;


            case Stage.S_29:     // Stage 8 bug  uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Up;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.CurveLeftUp_RightDown,//bug

                     // X 2列目
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,

                     // X 3列目
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,

                     // X 4列目
                    PanelType.CurveLeftUp_RightDown,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_30:     // Stage 9 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 30f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.Nothing,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.Nothing,

                     // X 3列目
                    PanelType.Nothing,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.Nothing,

                     // X 4列目
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_31:     // Stage 10 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 60f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,

                     // X 3列目
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,

                     // X 4列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                };
                break;

            case Stage.S_32:     // Stage 11 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 60f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.CurveLeftUp,

                     // X 3列目
                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveUpRight,

                     // X 4列目
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,

                };
                break;

            case Stage.S_33:     // Stage 12 uehara
                currentX = 0;
                currentY = 3;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage1 [4, 4]
                panelSize = 4;

                timeRemain = 60f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[]
                {
                    // X 1列目
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.StraightHorizontal,

                     // X 2列目
                    PanelType.CurveDownLeft,
                    PanelType.CurveUpRight,
                    PanelType.CurveRightDown,
                    PanelType.CurveLeftUp,

                     // X 3列目
                    PanelType.CurveRightDown,
                    PanelType.CurveLeftUp,
                    PanelType.CurveDownLeft,
                    PanelType.CurveUpRight,

                     // X 4列目
                    PanelType.StraightHorizontal,
                    PanelType.Nothing,
                    PanelType.Nothing,
                    PanelType.Nothing,

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
                            excellentScore = (int)timeRemain * TIME_RATIO;

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
                            excellentScore = (int)timeRemain * TIME_RATIO;

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
                            excellentScore = (int)timeRemain * TIME_RATIO;
                    
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
                if (UnityEngine.Random.value < 0.5f)
                {
                    panelSize = 5;
                }
                else
                {
                    panelSize = 5;
                }

                currentX = 0;
                currentY = panelSize - 1;
                CurrentFusePanelDirection = FuseDirection.Right;

                // Stage X [Random, Random]
                timeRemain = 60f;
                excellentScore = (int)timeRemain * TIME_RATIO;

                stage = new PanelType[panelSize * panelSize];

                for (int i = 0; i < stage.Length; i++)
                {
                    stage[i] = (PanelType)((int)(UnityEngine.Random.value * 8f) + 1);
                }

                for (int i = 0; i < UnityEngine.Random.value * 7 + 2; i++)
                {
                    stage[(int)(UnityEngine.Random.value * (float)panelSize * (float)panelSize)] = PanelType.Nothing;
                }
                stage[panelSize - 1] = PanelType.StraightHorizontal;

                break;
        }
    }


}


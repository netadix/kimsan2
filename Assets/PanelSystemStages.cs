//#define DRAG_CONTROL
#define PUSH_CONTROL


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

        switch ((Stage)stageNum)
        {
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
                break;
        }
    }


}


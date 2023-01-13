// using System;
// using BepInEx;
// using HarmonyLib;
// using Amplitude;
// using Amplitude.Framework;
// using Amplitude.Framework.Networking;
// using Amplitude.Framework.Overlay;
// using Amplitude.Mercury.Interop;
// using Amplitude.Mercury.Runtime;
// using Amplitude.Mercury.Sandbox;
// using UnityEngine;
// using System.Collections;
// using Amplitude.Framework.Simulation;
// using Amplitude.Mercury.Simulation;
// using Amplitude.Mercury.Data.Simulation;
// using Modding.Humankind.DevTools;
// using HumankindModTool;
// using Amplitude.Mercury.UI;
// using Amplitude.Mercury.UI.Helpers;
// using Amplitude.Mercury.UI.Tooltips;
// using Amplitude.Mercury.UI.Windows;
// using Amplitude.UI;
// using Amplitude.UI.Interactables;
// using Amplitude.UI.Tooltips;
// using Amplitude.UI.Windows;
// using Amplitude.Mercury;

// namespace shakee.Humankind.FameByScoring
// {
   
//     public class FameUITest : MonoBehaviour
//     {
//         GameObject var = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/");
//         GameObject test = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/");
//         UITooltip varTooltTip = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/").GetComponent<UITooltip>();
//         UIInteractable fameScoreHover = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/").GetComponent<UITooltip>().GetComponent<UIInteractable>();
//         public static RectOffset TreeRowPadding = new RectOffset(2, 0, 0, 0);
//         public static RectOffset TreeItemPadding = new RectOffset(22, 8, 0, 0);
//         bool isHovered = false;
//         void OnGUI()        
//         {   
//             if (fameScoreHover.Hovered)
//             {   
//                 MajorEmpireExtension empireSave = MajorEmpireSaveExtension.GetExtension(Convert.ToInt32(R.Utils_GameUtils().GetCurrentEmpireInfo().EmpireIndex));
//                 MajorEmpire empire = empireSave.empire;

//                 if (empire.EraLevel.Value > 0 && empireSave.FameHistoryList.Count > 0)
//                 {
//                     GUILayout.BeginArea(new Rect(0.25f * Screen.width , Screen.height * 0.25f, 350f, 300f));                    
//                         GUILayout.Space(5);                        
//                         GUILayout.BeginVertical();                        
//                             GUILayout.Space(5);
//                             GUILayout.Box("Fame History");
//                             GUILayout.Space(5);
//                             GUILayout.BeginHorizontal();
//                                 GUILayout.BeginVertical();
//                                     GUILayout.Label("Scoring Turn");
//                                     GUILayout.Label("Famegain");
//                                     GUILayout.Label("Total Rank");
//                                     GUILayout.Label("Military");
//                                     GUILayout.Label("State");
//                                     GUILayout.Label("Economy");
//                                     GUILayout.Label("City");
                                    
//                                 GUILayout.EndVertical();
//                                 for (int i = 0; i < Mathf.Min(empireSave.FameHistoryList.Count, 3); i++)
//                                 {
//                                     GUILayout.BeginVertical();
//                                         FameHistory fame = ScoringRound.GetHistory(empire,empireSave.FameHistoryList.Count - 1 - i);
//                                         GUILayout.Label(fame.turn.ToString());
//                                         GUILayout.Label(fame.fame.ToString());
//                                         GUILayout.Label(empireSave.listRanking[fame.totalRank]);
//                                         GUILayout.Label(empireSave.listRanking[(fame.categoryRank[0])]);
//                                         GUILayout.Label(empireSave.listRanking[(fame.categoryRank[1])]);
//                                         GUILayout.Label(empireSave.listRanking[(fame.categoryRank[2])]);
//                                         GUILayout.Label(empireSave.listRanking[(fame.categoryRank[3])]);
//                                     GUILayout.EndVertical();
//                                 }
//                             GUILayout.EndHorizontal();
//                             GUILayout.Space(5);
//                             GUILayout.Box("Last Era Change");
//                             GUILayout.Space(TreeRowPadding.top);
//                         GUILayout.EndVertical();
//                     GUILayout.EndArea();
//                 }   
//             }
//         }     


//     }

// }
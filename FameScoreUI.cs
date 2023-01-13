using System;
using BepInEx;
using HarmonyLib;
using Amplitude;
using Amplitude.Framework;
using Amplitude.Framework.Networking;
using Amplitude.Framework.Overlay;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Runtime;
using Amplitude.Mercury.Sandbox;
using UnityEngine;
using System.Collections;
using Amplitude.Framework.Simulation;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.Simulation;
using Modding.Humankind.DevTools;
using HumankindModTool;
using Amplitude.Mercury.UI;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.Mercury.UI.Tooltips;
using Amplitude.Mercury.UI.Windows;
using Amplitude.UI;
using Amplitude.UI.Interactables;
using Amplitude.UI.Tooltips;
using Amplitude.UI.Windows;
using Amplitude.Mercury;

namespace shakee.Humankind.FameByScoring
{
   
    public class FameScoreUI : MonoBehaviour
    {      
        GameObject var = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/");
        UITooltip varTooltTip = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/").GetComponent<UITooltip>();
        UIInteractable fameScoreHover = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/").GetComponent<UITooltip>().GetComponent<UIInteractable>();
        public static RectOffset TreeRowPadding = new RectOffset(2, 0, 0, 0);
        public static RectOffset TreeItemPadding = new RectOffset(22, 8, 0, 0);
        public static int fontSizeNormal = 13;
        public static int fontSizeLarge = 16;
        public static string[] labelList = new string[] {
                "Scoring Turn",
                "Famegain",
                "Total Rank",
                "Military",
                "State",
                "Economy",
                "City",

            };
        void Start()
        {
            Console.WriteLine("FameHistory Attach Test");
            // GameObject FameHistoryGUI = new GameObject("FameHistoryGUI");
            // FameHistoryGUI.transform.SetParent(var.transform);
            // FameHistoryGUI.AddComponent<UITransform>();
            // FameHistoryGUI.AddComponent<UITooltip>();
            // FameHistoryGUI.GetComponent<UITransform>().LeftAnchor.SetAttach(true);
            // FameHistoryGUI.AddComponent<FameUITest>();
        }   
        void OnGUI()        
        {    


            if (fameScoreHover.Hovered)
            {   
                MajorEmpireExtension empireSave = MajorEmpireSaveExtension.GetExtension(Convert.ToInt32(R.Utils_GameUtils().GetCurrentEmpireInfo().EmpireIndex));
                MajorEmpire empire = empireSave.empire;

                if (empire.EraLevel.Value > 0 && empireSave.FameHistoryList.Count > 0)
                {
                    GUILayout.BeginArea(new Rect(0.15f * Screen.width , Screen.height * 0.15f, 340f, 350f));                    
                        GUILayout.Space(TreeRowPadding.top);                        
                        GUILayout.BeginVertical(BackgroundStyle.Get(Color.grey));
                            GUILayout.Label("Scoring Rounds History",BackgroundStyle.Headline(BackgroundStyle.DarkishTextColor));
                            if (GameOptionHelper.CheckGameOption(FameByScoring.EraStarSettingStars, "False"))
                            {                            
                                
                                GUILayout.BeginHorizontal(BackgroundStyle.Get(BackgroundStyle.DarkishTextColor));                                
                                    GUILayout.Label("Fame Threshold: " + empire.FameScore.Value + " / " + (empireSave.lastFameScoreEraChange + FameThresholds.fameThresholdStep * (1 + empire.EraStarsCount.Value)) + " (" + (empireSave.lastFameScoreEraChange + FameThresholds.fameThresholdStep * (1 + empire.EraStarsCount.Value) - empire.FameScore.Value) + ")",BackgroundStyle.NormalCenter(fontSizeLarge));
                                    //GUILayout.Label(FameThresholds.fameThesholdStep.ToString());                               
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.Space(TreeRowPadding.top);                                    

                                GUILayout.BeginHorizontal();
                                    GUILayout.BeginVertical();                                    
                                    for (int k = 0; k < labelList.GetLength(0); k++)
                                    {
                                        GUILayout.Label(labelList[k],BackgroundStyle.NormalLeft(fontSizeNormal));
                                    }
                                    GUILayout.EndVertical();
                                    for (int i = 0; i < Mathf.Min(empireSave.FameHistoryList.Count, 3); i++)
                                    {
                                        GUILayout.BeginVertical();
                                        FameHistory fame = ScoringRound.GetHistory(empire,empireSave.FameHistoryList.Count - 1 - i);
                                        GUILayout.Label(fame.turn.ToString(),BackgroundStyle.NormalCenter(fontSizeNormal));
                                        GUILayout.Label(fame.fame.ToString(),BackgroundStyle.NormalCenter(fontSizeNormal));
                                        GUILayout.Label(empireSave.listRanking[fame.totalRank],BackgroundStyle.NormalCenter(fontSizeNormal));
                                        for (int k = 0; k < 4; k++)
                                        {
                                            GUILayout.Label(empireSave.listRanking[fame.categoryRank[k]],BackgroundStyle.RankColor(empireSave.listRanking[(fame.categoryRank[k])]));
                                        }
                                        GUILayout.EndVertical();
                                    }                                      
                                    GUILayout.EndHorizontal();                                                                       
                                
                                
                            GUILayout.Space(TreeRowPadding.top);
                            GUILayout.Label("Last Era Change",BackgroundStyle.Headline(BackgroundStyle.DarkishTextColor));
                            GUILayout.Space(TreeRowPadding.top);
                                GUILayout.BeginHorizontal();
                                    GUILayout.Space(1f);
                                    string rank = empireSave.listRanking[empireSave.lastFameRankEraChange];
                                    GUILayout.Label("Total Rank: ",BackgroundStyle.NormalCenter(fontSizeNormal));
                                    GUILayout.Label(empireSave.listRanking[empireSave.lastFameRankEraChange],BackgroundStyle.RankColor(rank));
                                    GUILayout.Label("Fame Gain: ",BackgroundStyle.NormalCenter(fontSizeNormal));
                                    GUILayout.Label("+" + empireSave.lastFameGainEraChange.ToString(),BackgroundStyle.NormalCenter(fontSizeNormal));
                                GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                    GUILayout.EndArea();
                }   
            }

            
        }   
            
        IEnumerator DoCheck()
        {            
            MajorEmpireExtension empireSave = MajorEmpireSaveExtension.GetExtension(Convert.ToInt32(R.Utils_GameUtils().GetCurrentEmpireInfo().EmpireIndex));
            MajorEmpire empire = empireSave.empire;

            Console.WriteLine("Famescore test 2");
            GUILayout.BeginArea(new Rect(0.2f * Screen.width , Screen.height * 0.25f, 100f, 100f));
            GUILayout.BeginVertical();
            GUILayout.Label("Fame History");
            
            GUILayout.EndVertical();
            GUILayout.EndArea();


            yield return new WaitForSeconds(1.15f);            
        }

    }
}
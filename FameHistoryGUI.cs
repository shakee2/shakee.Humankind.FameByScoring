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
   
    public class FameHistoryGUI : UIWindow
    {      
        GameObject var = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/");
        UITooltip varTooltTip = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/").GetComponent<UITooltip>();
        UIInteractable fameScoreHover = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/").GetComponent<UITooltip>().GetComponent<UIInteractable>();
        public static RectOffset TreeRowPadding = new RectOffset(2, 0, 0, 0);
        public static RectOffset TreeItemPadding = new RectOffset(22, 8, 0, 0);

        void DoLoad()
        {
            Console.WriteLine("Gameobject DoLoad Test");            
        }
        new void Start()
        {
            Console.WriteLine("Gameobject Start Test"); 
 
        }
        FameHistoryGUI()
        {
            Console.WriteLine("Class Object Start Test"); 
        }
        
        void OnGUI()        
        {        
            if (fameScoreHover.Hovered)
            {   
                MajorEmpireExtension empireSave = MajorEmpireSaveExtension.GetExtension(Convert.ToInt32(R.Utils_GameUtils().GetCurrentEmpireInfo().EmpireIndex));
                MajorEmpire empire = empireSave.empire;

                if (empire.EraLevel.Value > 0 && empireSave.FameHistoryList.Count > 0)
                {
                    GUILayout.BeginArea(new Rect(0.35f * Screen.width , Screen.height * 0.25f, 350f, 300f));
                    GUI.backgroundColor = Color.gray;
                        GUILayout.Space(TreeRowPadding.top);
                        
                        GUILayout.BeginVertical();
                            GUILayout.Space(TreeRowPadding.top);
                            GUILayout.Box("Fame History");
                            GUILayout.Space(TreeRowPadding.top);
                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical();
                                    GUILayout.Label("Scoring Turn");
                                    GUILayout.Label("Famegain");
                                    GUILayout.Label("Total Rank");
                                    GUILayout.Label("Military");
                                    GUILayout.Label("State");
                                    GUILayout.Label("Economy");
                                    GUILayout.Label("City");
                                    
                                GUILayout.EndVertical();
                                for (int i = 0; i < Mathf.Min(empireSave.FameHistoryList.Count, 3); i++)
                                {
                                    GUILayout.BeginVertical();
                                        FameHistory fame = ScoringRound.GetHistory(empire,empireSave.FameHistoryList.Count - 1 - i);
                                        GUILayout.Label(fame.turn.ToString());
                                        GUILayout.Label(fame.fame.ToString());
                                        GUILayout.Label(empireSave.listRanking[fame.totalRank]);
                                        GUILayout.Label(empireSave.listRanking[(fame.categoryRank[0])]);
                                        GUILayout.Label(empireSave.listRanking[(fame.categoryRank[1])]);
                                        GUILayout.Label(empireSave.listRanking[(fame.categoryRank[2])]);
                                        GUILayout.Label(empireSave.listRanking[(fame.categoryRank[3])]);
                                    GUILayout.EndVertical();
                                }
                            GUILayout.EndHorizontal();
                            GUILayout.Space(TreeRowPadding.top);
                            GUILayout.Box("Last Era Change");
                            GUILayout.Space(TreeRowPadding.top);
                        GUILayout.EndVertical();
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
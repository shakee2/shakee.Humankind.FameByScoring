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
    [HarmonyPatch(typeof(EmpireBanner_FamePennant))]
    public class EmpireBanner_FamePennant_Patch : MonoBehaviour
    {     
        static bool doneObject = false; 

        [HarmonyPatch("OnBeginShow")]    
        [HarmonyPostfix]  
        public static void OnBeginShow(EmpireBanner_FamePennant __instance, bool instant)
        {
            Console.WriteLine("Show Fame Pennant");
            //Change
            
        }    

        public static void SetupComponent()
        {
            GameObject var = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/");
            var bool1 = var.TryGetComponent<FameScoreUI>(out FameScoreUI test2);

            if (doneObject == false)
            {
                doneObject = true;
                GameObject FameHistoryGUI = new GameObject("FameHistoryGUI");
                FameHistoryGUI.transform.SetParent(var.transform);
                FameHistoryGUI.AddComponent<UITransform>();
                FameHistoryGUI.AddComponent<UITooltip>();
                FameHistoryGUI.AddComponent<FameScoreUI>();
                FameHistoryGUI.AddComponent<FameHistoryGUI>();
                // var.AddComponent<FameScoreUI>();
                // var var2 = var.GetComponent<FameScoreUI>();
                
            }
        }
    }    

    public class FameHistory_GUI
    {
        
    }
}
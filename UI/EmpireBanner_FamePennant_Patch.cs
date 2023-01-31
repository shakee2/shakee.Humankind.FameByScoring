using System;
using HarmonyLib;
using UnityEngine;
using Amplitude.Mercury.UI;
using Amplitude.UI;
using Amplitude.UI.Interactables;

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
            //Console.WriteLine("Show Fame Pennant");
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
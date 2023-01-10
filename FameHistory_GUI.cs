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

        [HarmonyPatch("OnBeginShow")]    
        [HarmonyPostfix]  
        public static void OnBeginShow(EmpireBanner_FamePennant __instance, bool instant)
        {
            Console.WriteLine("Show Fame Pennant");
            GameObject var = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/");
            //var.AddComponent<FameScoreUI>();

            
            // ExampleClass test = new ExampleClass().;
            // test.Counter();
            //new FameHistory_GUI(__instance, instant);
            
        }    
    }    

    public class FameHistory_GUI
    {
        public static void SetupComponent()
        {
            GameObject var = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/");
            var test = var.GetComponent<FameScoreUI>();
            if (test == null)
            {
                var.AddComponent<FameScoreUI>();
            }
            
        }
        public static void RemoveSetupComponent()
        {
            GameObject var = GameObject.Find("WindowsRoot/InGameOverlays/EmpireBanner/_FamePennant/FameScore/");
            var.GetComponent<FameScoreUI>();
            
        }

    }
}
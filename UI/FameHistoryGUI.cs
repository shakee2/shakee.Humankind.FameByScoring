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

    }
}
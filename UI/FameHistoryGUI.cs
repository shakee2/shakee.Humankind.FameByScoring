using System;

using UnityEngine;
using Amplitude.UI.Interactables;
using Amplitude.UI.Windows;

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
            //Console.WriteLine("Gameobject DoLoad Test");            
        }
        new void Start()
        {
            //Console.WriteLine("Gameobject Start Test"); 
 
        }
        FameHistoryGUI()
        {
            //Console.WriteLine("Class Object Start Test"); 
        }

    }
}
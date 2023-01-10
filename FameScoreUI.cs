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
        
        bool isHovered = false;
        void Start()
        {
            Console.WriteLine("Attach Test");

        }   

        // void Update()
        // {        
        //     var varTooltTip = var.GetComponent<UITooltip>();
        //     var hover = varTooltTip.GetComponent<UIInteractable>();
        //     if (hover.Hovered)
        //     {
        //         //Console.WriteLine("Famescore Visible");






        //         //var.AddComponent<FameScore>();
        //     }
        // }   
        void OnGUI()        
        {        
            if (fameScoreHover.Hovered)
            {   
                isHovered = true;
                StartCoroutine(DoCheck());
            }
            if (!fameScoreHover.Hovered && isHovered)
            {   
                isHovered = false;
                //StartCoroutine(DoCheck());
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
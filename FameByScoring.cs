using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Amplitude;
using Amplitude.Mercury;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Data.World;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Terrain;

namespace shakee.Humankind.FameByScoring
{

    [BepInPlugin(PLUGIN_GUID, "Fame By Scoring Rounds", "1.0.0.0")]
    public class FameByScoring : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "shakee.Humankind.FameByScoring";
        void Awake()
        {
            var harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
        }
    }

//Add Harmony patches here

}

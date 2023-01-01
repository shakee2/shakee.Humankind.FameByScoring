using System;
using System.Collections.Generic;
using System.Linq;
using Amplitude.Mercury.Simulation;
using HarmonyLib;
using Amplitude.Mercury.Sandbox;
using Amplitude.Framework;
using Amplitude;
//using Amplitude.Mercury.Game;
using Amplitude.Framework.Session;
//using Amplitude.Mercury;
using System.Reflection;
//using Amplitude.Mercury.Interop;
using Amplitude.Framework.Networking;
using Amplitude.Framework.Simulation;
using Amplitude.Serialization;
using Amplitude.Framework.Storage;

namespace shakee.Humankind.FameByScoring
{

	//*
	[HarmonyPatch(typeof(Sandbox))]
	public class TCL_Sandbox
	{

        [HarmonyPatch("Load")]
		[HarmonyPatch(new Type[] { typeof(StorageContainerInfo) } )]
		[HarmonyPrefix]
		public static bool Load(Sandbox __instance, StorageContainerInfo storageContainerInfo)
		{
			
			Diagnostics.LogError($"[FameByScoring] [Sandbox] [Load] {storageContainerInfo.GetMetadata("GameSaveMetadata::Title")}, {storageContainerInfo.GetMetadata("GameSaveMetadata::DateTime")}");
			return true;
		}

        [HarmonyPatch("ThreadStart")]
		[HarmonyPostfix]
		public static void ThreadStartExit(Sandbox __instance, object parameter)
		{
			Diagnostics.LogWarning($"[FameByScoring] exiting Sandbox, ThreadStart");
			MajorEmpireSaveExtension.OnExitSandbox();
		}

        [HarmonyPatch("ThreadStart")]
		[HarmonyPrefix]
		public static bool ThreadStart(Sandbox __instance, object parameter)
		{
			Diagnostics.LogWarning($"[FameByScoring] entering Sandbox, ThreadStart");
			MajorEmpireSaveExtension.OnSandboxStart();
            return true;
        }
    }
}
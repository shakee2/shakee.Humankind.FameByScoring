using System;
using HarmonyLib;
using Amplitude.Mercury.Sandbox;

using Amplitude;

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
			//FameHistory_GUI.SetupComponent();
            return true;
        }
		[HarmonyPatch("ThreadStarted")]
		[HarmonyPrefix]
		public static bool ThreadStarted(Sandbox __instance)
		{
			Diagnostics.LogWarning($"[FameByScoring] entering Sandbox, ThreadStart");
			//MajorEmpireSaveExtension.OnSandboxStart();
			//FameHistory_GUI.SetupComponent();
            return true;
        }

    }
}
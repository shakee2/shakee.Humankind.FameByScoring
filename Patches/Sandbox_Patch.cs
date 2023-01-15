using System;
using HarmonyLib;
using Amplitude.Mercury.Sandbox;
using Amplitude;
using Amplitude.Mercury.Game;
using Amplitude.Mercury.Runtime;
using Amplitude.Framework.Storage;
using Amplitude.Framework;
using HumankindModTool;

namespace shakee.Humankind.FameByScoring
{

	[HarmonyPatch(typeof(Sandbox))]
	public class Sandbox_Patch
	{
		public static bool ScoringOn = false;

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
			if (ScoringOn == true)
			{
				Diagnostics.LogWarning($"[FameByScoring] exiting Sandbox, ThreadStart");
				MajorEmpireSaveExtension.OnExitSandbox();
				ScoringOn = false;
			}
			
		}
		public static bool ModDefaultingOff;

        [HarmonyPatch("ThreadStart")]
		[HarmonyPrefix]
		public static bool ThreadStart(Sandbox __instance, object parameter)		
		{
			ModDefaultingOff = false;
			try
			{
				int ranking = -1;
				ranking = int.Parse(GameOptionHelper.GetGameOption(FameByScoring.FameScoringOption));
				//Console.WriteLine(ranking.ToString());
				GameSaveDescriptor gameSave = parameter as GameSaveDescriptor;
				if (gameSave == null && ranking <= 0)
				{
					throw new Exception("[FameByScoring] No GameSave and Ranking Off");
				}
				SandboxStartSettings sandboxStartSettings = parameter as SandboxStartSettings;
				if (sandboxStartSettings == null && ranking <= 0)
				{
					throw new Exception("[FameByScoring] No SandBox Startsettings");
				}
			}
			catch (System.Exception exception)
			{		
				// Diagnostics.LogError("[FameByScoring] GameSave null");		
				Diagnostics.LogException(exception);
				ModDefaultingOff = true;
			}
			
			if (ModDefaultingOff == false)	
			{	
				Diagnostics.LogWarning($"[FameByScoring] entering Sandbox, ThreadStart");
				MajorEmpireSaveExtension.OnSandboxStart();
				ScoringOn = true;
			}
            return true;
			
        }
    }
}
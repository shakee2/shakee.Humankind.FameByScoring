using System;
using HarmonyLib;
using Amplitude.Mercury.Sandbox;
using Amplitude;
using Amplitude.Mercury.Game;
using Amplitude.Mercury.Options;
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
			Console.WriteLine("Loading Savegame");
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
		public static bool IsScenarioGame;
		static SandboxThreadStartSettings SandboxThreadStartSettings { get; set; }

        [HarmonyPatch("ThreadStart")]
		[HarmonyPrefix]
		public static bool ThreadStart(Sandbox __instance, object parameter)		
		{
			IsScenarioGame = false;
			SandboxThreadStartSettings = parameter as SandboxThreadStartSettings;
			ModDefaultingOff = false;
			bool scoringRoundsOff = true;
			scoringRoundsOff = GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption,"false");
			//Console.WriteLine("Mod Before Tries: " + scoringRoundsOff + " OptionSetting: " + GameOptionHelper.GetGameOption(FameByScoring.FameScoringOption));
			try			
			{			
				scoringRoundsOff = GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption,"false");
				SandboxStartSettings sandboxStartSettings = SandboxThreadStartSettings.Parameter() as SandboxStartSettings;	
				if (sandboxStartSettings != null)
				{					
					//Console.WriteLine("Sandbox Found");
					scoringRoundsOff = GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption,"false");
					if (scoringRoundsOff)
						throw new Exception("[FameByScoring] New Game started -> Mod is disabled");
					else
						Diagnostics.LogWarning("[FameByScoring] New Game started -> Mod is enabled");
				}
				else
				{					
					ScenarioStartSettings scenarioStartSettings = SandboxThreadStartSettings.Parameter() as ScenarioStartSettings;
					if (scenarioStartSettings != null)
					{						
						ModDefaultingOff = true;
						IsScenarioGame = true;
						throw new Exception("[FameByScoring] New Scenario started -> Mod is turned Off");
					}
					GameSaveDescriptor gameSave = SandboxThreadStartSettings.Parameter() as GameSaveDescriptor;					
					if (gameSave != null)
					{
						scoringRoundsOff = GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption,"false");
							//Console.WriteLine(gameSave.ScenarioName.ToString() + " Scoring Off: " + scoringRoundsOff);
						if (gameSave.ScenarioName != StaticString.Empty)
						{
							IsScenarioGame = true;																			
							throw new Exception("[FameByScoring] Loading Scenario Savegame; Mod is turned off");												
						}
						else if (scoringRoundsOff)
							throw new Exception("[FameByScoring] Loading Base Savegame -> Mod is disabled");
						else
							Diagnostics.LogWarning("[FameByScoring] Loading Base Savegame -> Mod is enabled");						
					}
				}				

			}
			catch (System.Exception exception)
			{							
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
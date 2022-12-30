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
using UnityEngine;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Data.World;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Terrain;
using HumankindModTool;
using Amplitude.Framework.Options;
using Amplitude.Mercury.Data.GameOptions;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.Mercury.UI;

namespace shakee.Humankind.FameByScoring

//
// Ranking Alternative Test
//
{

    [BepInPlugin(PLUGIN_GUID, "Fame By Scoring Rounds", "1.0.0.0")]
    public class FameByScoring : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "shakee.Humankind.FameByScoring";
        void Awake()
        {
            //Console.WriteLine("Mod: FamebyScoring loaded...");
            var harmony = new Harmony(PLUGIN_GUID);
            //FameByScoring.Instance = this;
            harmony.PatchAll();
            
        }

        //public static FameByScoring Instance;
        public const string GameOptionGroup_LobbyPaceOptions = "GameOptionGroup_LobbyPaceOptions";   

#region GameoOptions
		public static GameOptionInfo FameScoringOption = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_FameScoring",
			DefaultValue = "true",
            editbleInGame = true,
			Title = "[FAME] Fame by Scoring Rounds Distribution",
			Description = "Sets how fame is generated and distributed. If activated, you gain fame every few turns and when an empire changes era.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Deactivate",
                    Description = "Fame by Scoring Rounds is deactivated.",
                    Value = "off"
                },
                new GameOptionStateInfo{
                    Title = "Fame by Ratio",
                    Description = "Fame is distributed by your ratio in comparison to all other Empires in the scoring categories. The fame gains can vary wildy between empires.",
                    Value = "false"
                },
                new GameOptionStateInfo{
                    Title = "Fame by Ranking",
                    Description = "Fame is distributed by ranking per category. Depending on your ranking in descending order, you get a certain amount of fame. The first 3 places get more fame. Uses the base fame setting. The fame gain is more evenly distributed.",
                    Value = "true"
                },
			}
		};
        public static GameOptionInfo NumberScoringRounds = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_NumberFameScoring",
			DefaultValue = "8",
            editbleInGame = true,
			Title = "[FAME] Scoring Rounds Turn",
			Description = "Sets after how many turns a fame scoring round is triggered. Does not affect a fame scoring when an empire changes era.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Every 4 Turns",
                    Description = "-",
                    Value = "4"
                },
                new GameOptionStateInfo{
                    Title = "Every 8 Turns",
                    Description = "-",
                    Value = "8"
                },
                new GameOptionStateInfo{
                    Title = "Every 12 Turns",
                    Description = "-",
                    Value = "12"
                },
                new GameOptionStateInfo{
                    Title = "Every 16 Turns",
                    Description = "-",
                    Value = "16"
                },
            }
        };

        public static GameOptionInfo FameGainMultiplier = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_FameGainMultiplier",
			DefaultValue = "1",
            editbleInGame = true,
			Title = "[FAME] Scoring Rounds Fame modifier",
			Description = "Setting for adjusting the fame gain per Scoring Round. Multiplies the base fame by this amount.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "0.5x",
                    Description = "Fame is multiplied by.",
                    Value = "0.5"
                },
                new GameOptionStateInfo{
                    Title = "0.75x",
                    Description = "Fame is multiplied by.",
                    Value = "0.75"
                },
                new GameOptionStateInfo{
                    Title = "1x",
                    Description = "Fame is multiplied by.",
                    Value = "1"
                },
                new GameOptionStateInfo{
                    Title = "1.5x",
                    Description = "Fame is multiplied by.",
                    Value = "1.5"
                },
                new GameOptionStateInfo{
                    Title = "2x",
                    Description = "Fame is multiplied by.",
                    Value = "2"
                },
            }
        };
        public static GameOptionInfo FameTurnMultiplier = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_FameTurnMultiplier",
			DefaultValue = "true",
            editbleInGame = true,
			Title = "[FAME] Fame Scoring Rounds GameSpeed modifier",
			Description = "If enabled, the turn for scoring will be modified by the gamespeed multiplier.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "On",
                    Description = "Turn trigger will be modified by the gamespeed multiplier.",
                    Value = "true"
                },
                new GameOptionStateInfo{
                    Title = "Off",
                    Description = "Turn trigger won't be modified.",
                    Value = "false"
                },
            }
        };
        public static GameOptionInfo FameBaseGain = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_FameBaseGain",
			DefaultValue = "20",
            editbleInGame = true,
			Title = "[FAME] Base Fame for Scoring Rounds",
			Description = "Customize the base fame which is used for scoring rounds per scoring category. For Ratio it gets multiplied by number of empires. For Ranking it is used as the base fame gain.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "10",
                    Description = "10 Fame.",
                    Value = "10"
                },
                new GameOptionStateInfo{
                    Title = "20",
                    Description = "20 Fame.",
                    Value = "20"
                },
                new GameOptionStateInfo{
                    Title = "30",
                    Description = "30 Fame.",
                    Value = "30"
                },
                new GameOptionStateInfo{
                    Title = "40",
                    Description = "40 Fame.",
                    Value = "40"
                },
            }
        };
#endregion

    }

    [HarmonyPatch(typeof(SimulationEvent_NewTurnBegin))]

    public class NewTurn_Patch 
    {
        [HarmonyPatch("Raise")]    
        [HarmonyPostfix]   

        public static void Raise (SimulationEvent_NewTurnBegin __instance, object sender, ushort turn) {
            if(!GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "off"))
            {
                
                //Console.WriteLine("Current Turn: {0}", turn.ToString());
                float gameSpeed;
                int gameOptionTurns = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.NumberScoringRounds));
                if (GameOptionHelper.CheckGameOption(FameByScoring.FameTurnMultiplier,"true"))
                {
                    gameSpeed = Amplitude.Mercury.Interop.AI.Snapshots.Game.GameSpeedMultiplier;
                }
                else
                {
                    gameSpeed = 1f;
                }
                
                float turnTmp = gameOptionTurns * gameSpeed;
                int turnCheck = (int)turnTmp;             

                while ((int)turn > turnCheck) 
                {
                    
                    turnCheck += (int)turnTmp;
                    
                } 

                if ((int)turn == turnCheck)
                {
                    Console.WriteLine("Scoring Needed for Turn " + turn.ToString());
                    ScoringRound.RoundScoring(turn);
                    turnCheck += (int)turnTmp;
                }
                
                else if ((int)turn < turnCheck)
                {
                    
                    Console.WriteLine("No Scoring for Turn " + turn.ToString());

                }

                int finalTurn = turnCheck;

                Console.WriteLine("Next TurnCheck {0} | Gamespeed mod: {1}", finalTurn.ToString(), gameSpeed.ToString());
            }
        } 

    }


    [HarmonyPatch(typeof(SimulationEvent_EraChanged))]

    public class EraChange_Patch
    {     
        [HarmonyPatch("Raise")]
        [HarmonyPostfix]

        public static void Raise (object sender, int empireIndex, int eraIndex, StaticString previousFactionName)
        {
            if(!GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "off"))
            {
                ScoringRound.RoundScoring(empireIndex: empireIndex);
            }
        }
    }

    [HarmonyPatch(typeof(OptionsManager<GameOptionDefinition>))]
	public class OptionsManager_Patch
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002914 File Offset: 0x00000B14
		[HarmonyPatch("Load")]
		[HarmonyPrefix]
		public static bool Load(OptionsManager<GameOptionDefinition> __instance)
		{
			Console.WriteLine("Adding GameOptions...");
		    GameOptionHelper.Initialize(new GameOptionInfo[]
			{
				FameByScoring.FameScoringOption,
                FameByScoring.NumberScoringRounds,
                FameByScoring.FameTurnMultiplier,
                FameByScoring.FameGainMultiplier,
                FameByScoring.FameBaseGain,
			});
			return true;
		}
	}

}




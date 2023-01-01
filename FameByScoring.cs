using System;
using BepInEx;
using HarmonyLib;
using Amplitude;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Interop;
using HumankindModTool;
using Amplitude.Framework.Options;
using Amplitude.Mercury.Data.GameOptions;

using Amplitude.Framework;


namespace shakee.Humankind.FameByScoring  
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
			DefaultValue = "2",
            editbleInGame = true,
			Title = "[FAME] Scoring Rounds: Fame Distribution",
			Description = "Sets how fame is generated and distributed. If activated, you gain fame every few turns and when an empire changes era.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Deactivate",
                    Description = "Fame by Scoring Rounds is deactivated.",
                    Value = "0"
                },
                new GameOptionStateInfo{
                    Title = "by Ratio",
                    Description = "Fame is distributed by your ratio in comparison to all other Empires per category. The fame gains can vary wildy between empires.",
                    Value = "1"
                },
                new GameOptionStateInfo{
                    Title = "by Rank per Category",
                    Description = "Ranks are determined per category. Fame is distributed by your rank in each category. Depending on your ranking in descending order, you get a certain amount of fame. The first 3 places get more fame. Uses the base fame setting. The fame gain is more evenly distributed.",
                    Value = "2"
                },
                new GameOptionStateInfo{
                    Title = "by Rank per Property",
                    Description = "Ranks are determined per property. Fame is distributed by your rank in each category. Depending on your ranking in descending order, you get a certain amount of fame. The first 3 places get more fame. Uses the base fame setting. The fame gain is even more evenly distributed.",
                    Value = "3"
                },
			}
		};
        public static GameOptionInfo NumberScoringRounds = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_NumberFameScoring",
			DefaultValue = "8",
            editbleInGame = true,
			Title = "[FAME] Scoring Rounds: Turn Intervall",
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
			Title = "[FAME] Scoring Rounds: Fame Modifier",
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
			Title = "[FAME] Scoring Rounds: GameSpeed Modifier",
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
			Title = "[FAME] Scoring Rounds: Base Fame",
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
        public static GameOptionInfo EraStarSettingFame = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_EraStarSettingFame",
			DefaultValue = "True",
            editbleInGame = true,
			Title = "[FAME] Era Stars: Fame Gain Setting",
			Description = "Setting for controlling fame gain from Era Stars.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Default",
                    Description = "Same as vanilla",
                    Value = "True"
                },
                new GameOptionStateInfo{
                    Title = "Off",
                    Description = "Disables fame from Era Stars",
                    Value = "False"
                },
                new GameOptionStateInfo{
                    Title = "0.25x",
                    Description = "0.25x fame from Era Stars",
                    Value = "0.25"
                },
                new GameOptionStateInfo{
                    Title = "0.50x",
                    Description = "0.50x fame from Era Stars",
                    Value = "0.5"
                },
                new GameOptionStateInfo{
                    Title = "0.75x",
                    Description = "0.75x fame from Era Stars",
                    Value = "0.75"
                },
                new GameOptionStateInfo{
                    Title = "1.25x",
                    Description = "1.25x fame from Era Stars",
                    Value = "1.25"
                },
                new GameOptionStateInfo{
                    Title = "1.50x",
                    Description = "1.50x fame from Era Stars",
                    Value = "1.5"
                },
            }
        };
        
        public static GameOptionInfo EraStarSettingStars = new GameOptionInfo
		{
			ControlType = 0,
			Key = "GameOption_shakee_EraStarSettingStars",
			DefaultValue = "True",
            editbleInGame = true,
			Title = "[FAME] Era Stars: Gain Method",
			Description = "Setting for changing how era stars are gained. Default or a new method depending on your current fame score. When changing era, the new thresholds are always current famescore + threashold.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Default",
                    Description = "Era Stars are gained as in Vanilla.",
                    Value = "True"
                },
                new GameOptionStateInfo{
                    Title = "Fame Thresholds",
                    Description = "Reaching certain fame thresholds rewards an era star.",
                    Value = "False"
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
            if(!GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "0"))
            {        
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
                    ScoringRound.RoundScoring(true, turn);
                    turnCheck += (int)turnTmp;
                }
                
                else if ((int)turn < turnCheck)
                {
                    
                    Console.WriteLine("No Scoring for Turn " + turn.ToString());
                    
                }

                int finalTurn = turnCheck;

                Console.WriteLine("Next TurnCheck {0}", finalTurn.ToString());
            }
            if (GameOptionHelper.CheckGameOption(FameByScoring.EraStarSettingStars, "False"))
            {
                FameThresholds.CheckThreshold();
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
            if(!GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "0"))
            {
                ScoringRound.RoundScoring(false, empireIndex: empireIndex);            
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
                FameByScoring.FameBaseGain,
                FameByScoring.FameGainMultiplier,
                FameByScoring.EraStarSettingStars,
                FameByScoring.EraStarSettingFame,


			});
			return true;
		}
	}
    
    [HarmonyPatch(typeof(DepartmentOfDevelopment))]
    public class EraStar_Patch
    {		
        public static IDatabase<EraStarDefinition> eraStarDefinitionDatabase;

        [HarmonyPatch("TryEarnEraStar_One")]    
        [HarmonyPrefix]
        public static bool TryEarnEraStar_One(DepartmentOfDevelopment __instance, ref EraStarInfo eraStarInfo)
		{
            // if (GameOptionHelper.CheckGameOption(FameByScoring.EraStarSetting, "On"))
            // {
            //     return true;
            // }
            
                if (eraStarInfo.Thresholds == null)
                {
                    return false;
                }
                if (eraStarInfo.Level >= eraStarInfo.Thresholds.Length)
                {
                    return false;
                }
                if (eraStarInfo.Thresholds[eraStarInfo.Level] > eraStarInfo.Score)
                {
                    return false;
                }
            //Console.WriteLine("Getting Era Stars - Turn " + R.SandboxManager_Sandbox().Turn().ToString());
                //eraStarInfo.CurrentReward
                if (GameOptionHelper.CheckGameOption(FameByScoring.EraStarSettingFame, "True"))
                {
                    FixedPoint x = eraStarInfo.CurrentReward * ( 1 + R.majorEmpire(__instance).FameGainBonus.Value);
                    R.majorEmpire(__instance).FameScore.Value -= x;
                    R.majorEmpire(__instance).FameScore.Value += x;          
                    //ScoringRound.runDebug("Empire " + R.majorEmpire(__instance).Index().ToString() + " gains +" + x.ToString() + " Fame from " + eraStarInfo.EraStarDefinitionName.ToString(), 2);          
                    //FameByScoring_Helper.AccumulateFame_New(R.majorEmpire(__instance), eraStarInfo.CurrentReward * 0.5f);
                }
                else if (GameOptionHelper.CheckGameOption(FameByScoring.EraStarSettingFame, "False"))
                {
                    FixedPoint x = eraStarInfo.CurrentReward * ( 1 + R.majorEmpire(__instance).FameGainBonus.Value);
                    R.majorEmpire(__instance).FameScore.Value -= x;
                    //FameByScoring_Helper.AccumulateFame_New(R.majorEmpire(__instance), eraStarInfo.CurrentReward);
                }
                else
                {
                    FixedPoint x = eraStarInfo.CurrentReward * ( 1 + R.majorEmpire(__instance).FameGainBonus.Value) * (float)Convert.ToSingle(GameOptionHelper.GetGameOption(FameByScoring.EraStarSettingFame)); 
                    R.majorEmpire(__instance).FameScore.Value -= eraStarInfo.CurrentReward * ( 1 + R.majorEmpire(__instance).FameGainBonus.Value);
                    R.majorEmpire(__instance).FameScore.Value += x;
                    //ScoringRound.runDebug("Empire " + R.majorEmpire(__instance).Index().ToString() + " gains +" + x.ToString() + " Fame from " + eraStarInfo.EraStarDefinitionName.ToString(), 2);               
                }

            //     eraStarInfo.UnlockTurn[eraStarInfo.Level] = R.SandboxManager_Sandbox().Turn();
            //     eraStarInfo.UnlockedRewards[eraStarInfo.Level] = eraStarInfo.CurrentRewardWithFameBonus;
            // Console.WriteLine("Level " + eraStarInfo.EraStarDefinitionName.ToString() + " | Rewards: " +eraStarInfo.UnlockedRewards[eraStarInfo.Level].ToString());
            
            //     R.SimulationEvent_FameScoreChanged_Raise(__instance, R.majorEmpire(__instance), (int)eraStarInfo.CurrentReward);
            //     eraStarInfo.Level++;
                
                //var eraStarDefinitionDatabase = Databases.GetDatabase<EraStarDefinition>();
                if (!GameOptionHelper.CheckGameOption(FameByScoring.EraStarSettingStars, "True") && R.majorEmpire(__instance).EraLevel.Value > 0)
                {
                    R.majorEmpire(__instance).EraStarsCount.Value -= 1;
                    R.majorEmpire(__instance).SumOfEraStars.Value -= 1;
                }
            //     EraStarDefinition value = ScriptableObject.CreateInstance<EraStarDefinition>();
            //     value = eraStarDefinitionDatabase.GetValue(eraStarInfo.EraStarDefinitionName);
                
            // Console.WriteLine("GameplayOri: {0} | Name: {1} | MaxReward: {3}",value.GameplayOrientation.ToString(), value.Name.ToString(), value.MaxFameReward.ToString());
            // Console.WriteLine("Threshold: " + eraStarInfo.Thresholds.ToString());
            //     R.ComputeEraStarRewards(value, ref eraStarInfo);
            //     R.SimulationEvent_EraStarEarned_Raise(__instance, eraStarInfo.EraStarDefinitionName, eraStarInfo.Level - 1, R.majorEmpire(__instance).Index());
            //     R.SetupEvolutionPeriodicNotificationIfRequired();

                return true;
            
	  	}
    }
}




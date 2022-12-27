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


		public static GameOptionInfo FameScoringOption = new GameOptionInfo
		{
            
			ControlType = 0,
			Key = "GameOption_shakee_FameScoring",
			DefaultValue = "true",
			Title = "Fame Distribution",
			Description = "Sets how fame is generated and distributed.",
			GroupKey = "GameOptionGroup_LobbyPaceOptions",
			States = 
			{
                new GameOptionStateInfo{
                    Title = "Fame by Ratio",
                    Description = "Fame is distributed by your ratio in comparison to all other Empires in the scoring categories. The fame gains can vary wildy between empires.",
                    Value = "false"
                },
                new GameOptionStateInfo{
                    Title = "Fame by Ranking",
                    Description = "Fame is distributed by ranking. Depending on your ranking in descending order, you get a certain amount of fame. 1st = 120; 2nd = 110; 3rd = 100; all other 90.",
                    Value = "true"
                },
			}
		};

        public static GameOptionStateInfo FameScoring_Ratio = new GameOptionStateInfo
		{
			Value = "false",
			Title = "Fame by Ratio",
			Description = "Fame is distributed by your ratio in comparison to all other Empires in the scoring categories. The fame gains can vary wildy between empires"
		};
        public static GameOptionStateInfo FameScoring_Rank = new GameOptionStateInfo
		{
			Value = "true",
			Title = "Fame by Ranks",
			Description = "Fame is distributed by ranking. Depending on your ranking in descending order, you get a certain amount of fame. 1st = 120; 2nd = 110; 3rd = 100; all other 90"
		};
    }


    [HarmonyPatch(typeof(SimulationEvent_NewTurnBegin))]

    public class NewTurn_Patch 
    {
        [HarmonyPatch("Raise")]    
        [HarmonyPostfix]   

        public static void Raise (SimulationEvent_NewTurnBegin __instance, object sender, ushort turn) {
            Console.WriteLine("Current Turn: {0}", turn.ToString());

            float gamespeed = Amplitude.Mercury.Interop.AI.Snapshots.Game.GameSpeedMultiplier;
            float turnTmp = 2 * gamespeed;  // DEBUG TESTING --> 8 or 12 as default
            int turnCheck = (int)turnTmp;
            int turnMulti = 1;
            

            while ((int)turn > turnCheck * turnMulti) 
            {
                
                turnMulti += 1;
                
            } 

            if ((int)turn == turnCheck * turnMulti)
            {
                Console.WriteLine("Round Scoring Needed | Multi {0}", turnMulti.ToString());
                ScoringRound.RoundScoring(turn);
                
            }
            
            else if ((int)turn < turnCheck * turnMulti)
            {
                
                Console.WriteLine("No Scoring | Multi {0}", turnMulti.ToString());

            }

            int finalTurn = turnCheck * (turnMulti + 1);

            Console.WriteLine("Next TurnCheck {0} | Gamespeed mod: {1}", finalTurn.ToString(), gamespeed.ToString());
            
        } 

    }


    [HarmonyPatch(typeof(SimulationEvent_EraChanged))]

    public class EraChange_Patch
    {     
        [HarmonyPatch("Raise")]
        [HarmonyPostfix]

        public static void Raise (object sender, int empireIndex, int eraIndex, StaticString previousFactionName)
        {
            ScoringRound.RoundScoring(empireIndex: empireIndex);
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
			});
			return true;
		}
	}

    [HarmonyPatch(typeof(FormatUtils))]
        public class FormatUtils_Patch
    {
		
        [HarmonyPatch("FormatAmount")]    
        [HarmonyPostfix]  
        
        public static void FormatAmount(FormatUtils __instance, string __result, FixedPoint amount, bool handleHighAmount = false, Rounding rounding = Rounding.Floor, bool signed = false, int decimals = 0, bool percentage = false)
        {
            
            Console.WriteLine("[shakee.Tooltip.Fix] Done");
            decimals = 1;
            if (amount == FixedPoint.MaxValue)
			{
				if (!signed)
				{
					__result = TextUtils.InfinityCharacter;
				}
				__result =  TextUtils.PositiveInfinity;
			}
			if (amount == FixedPoint.MinValue)
			{
				__result =  TextUtils.NegativeInfinity;
			}
			string empty = string.Empty;
            if (percentage)
			{
				decimals += 1;
			}
			if (decimals > 0)
			{
				FixedPoint fixedPoint = FixedPoint.Floor(amount);
				FixedPoint fixedPoint2 = amount - fixedPoint;
				float num = Mathf.Pow(10f, decimals);
				fixedPoint2 *= num;
				switch (rounding)
				{
				case Rounding.Floor:
					fixedPoint2 = FixedPoint.Round(fixedPoint2);
					break;
				case Rounding.Ceil:
					fixedPoint2 = FixedPoint.Round(fixedPoint2);
					break;
				case Rounding.Round:
					fixedPoint2 = FixedPoint.Round(fixedPoint2);
					break;
				}
				fixedPoint2 /= num;
				amount = fixedPoint + fixedPoint2;
			}
			else
			{
				switch (rounding)
				{
				case Rounding.Floor:
					amount = FixedPoint.Floor(amount);
					break;
				case Rounding.Ceil:
					amount = FixedPoint.Ceiling(amount);
					break;
				case Rounding.Round:
					amount = FixedPoint.Round(amount);
					break;
				}
			}
			if (handleHighAmount)
			{
				FixedPoint fixedPoint3 = FixedPoint.Abs(amount);
				if (fixedPoint3 < 10000f)
				{
					__result =  amount.Format(decimals, percentage, signed);
				}
				if (fixedPoint3 < 100000f)
				{
					//__result =  TextUtils.Localize("%HighValueFormat", (amount * 0.001f).Format(1, percentage, signed));
				}
				if (fixedPoint3 < 1000000f)
				{
					//__result =  TextUtils.Localize("%HighValueFormat", (amount * 0.001f).Format(0, percentage, signed));
				}
				if (fixedPoint3 < 10000000f)
				{
					//__result = TextUtils.Localize("%VeryHighValueFormat", (amount * 1E-06f).Format(1, percentage, signed));
				}
				//__result = TextUtils.Localize("%VeryHighValueFormat", (amount * 1E-06f).Format(0, percentage, signed));
			}
			__result = amount.Format(decimals, percentage, signed);

        }
    }

}




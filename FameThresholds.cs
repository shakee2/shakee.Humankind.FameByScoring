using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
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
using Amplitude.Mercury.UI.Helpers;
using Amplitude.Framework.Simulation.Collections;
using Amplitude.Framework.Simulation.DataStructures;
using Amplitude.Serialization;
using Amplitude.Framework.Simulation;
using HumankindModTool;

namespace shakee.Humankind.FameByScoring
{
    public class FameThresholds
    {

        public static void CheckThreshold ()
        {
            int numEmpires = Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires;
            FixedPoint baseFame = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.FameBaseGain));
            float baseFameMulti = Convert.ToSingle(GameOptionHelper.GetGameOption(FameByScoring.FameGainMultiplier));
            float turn = Convert.ToSingle(GameOptionHelper.GetGameOption(FameByScoring.FameGainMultiplier));
            int gameOptionTurns = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.NumberScoringRounds));
            float gameSpeed = Amplitude.Mercury.Interop.AI.Snapshots.Game.GameSpeedMultiplier;

            for (int i = 0; i < numEmpires; i++)
            {
                MajorEmpire empire = Sandbox.MajorEmpires[i];

                FixedPoint tmpFame = baseFame * baseFameMulti;  // 20 * 1
                FixedPoint checkturn = (FixedPoint.Floor((FixedPoint)50 / gameOptionTurns)); // 50 / 2 = 25
                FixedPoint fameTreshold = FixedPoint.Floor(tmpFame * checkturn * gameSpeed * 4); // 20 * 25 * 4 * 0,5 = 240

                FixedPoint currentScore = FixedPoint.Floor(empire.FameScore.Value / fameTreshold);
                FixedPoint currentStars = empire.EraStarsCount.Value;
                if (currentScore > currentStars)
                {
                    empire.EraStarsCount.Value += currentScore - currentStars;
                    empire.SumOfEraStars.Value += currentScore - currentStars;
                }
                
                Console.WriteLine("Empire {4} | Turns: {0} | Next Threshold: {1} | Current Stars {2} | Current Fixed Stars: {3}", checkturn.ToString(), (fameTreshold * currentScore).ToString(),currentStars.ToString(), currentScore.ToString(), i.ToString());

            }
        }
    }
}
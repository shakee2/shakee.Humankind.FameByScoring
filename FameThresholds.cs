using System;
using Amplitude;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Interop;
using HumankindModTool;

namespace shakee.Humankind.FameByScoring
{
    public class FameThresholds
    {
        public static FixedPoint fameThresholdStep = 0;
        public static void CheckThreshold ()
        {
            int numEmpires = Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires;
            FixedPoint baseFame = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.FameBaseGain));
            float baseFameMulti = Convert.ToSingle(GameOptionHelper.GetGameOption(FameByScoring.FameGainMultiplier));
            int gameOptionTurns = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.NumberScoringRounds));
            float gameSpeed;
            MajorEmpire empire;
            MajorEmpireExtension majorSave;

            if (GameOptionHelper.CheckGameOption(FameByScoring.FameTurnMultiplier,"true"))
            {
                gameSpeed = Amplitude.Mercury.Interop.AI.Snapshots.Game.GameSpeedMultiplier;
            }
            else
            {
                gameSpeed = 1f;
            }

            for (int i = 0; i < numEmpires; i++)
            {
                empire = Sandbox.MajorEmpires[i];
                majorSave = MajorEmpireSaveExtension.GetExtension(empire.Index());
                Console.WriteLine("Last Saved Fame for Empire: " + empire.Index() + " with Fame: " + majorSave.lastFameScoreEraChange);
                EmpireInfo empireInfo = R.Utils_GameUtils().GetCurrentEmpireInfo();
                FixedPoint eraStarsReq =  empireInfo.EraStarsRequirement;

                FixedPoint tmpFame = baseFame * baseFameMulti * 4 * 1.2f;  // 20 * 1 * 4 = 96; 4 = number of categories
                FixedPoint checkturn = (FixedPoint.Floor((FixedPoint)50 * gameSpeed / (gameOptionTurns * gameSpeed))); // 50 * 0,5 / 8 * 0,5 = 6

                FixedPoint fameTreshold = GetFameThreshold(gameSpeed); // FixedPoint.Floor(tmpFame * checkturn / eraStarsReq); // 96 * 6 / 7 = 
                FixedPoint currentScore = FixedPoint.Floor((empire.FameScore.Value - majorSave.lastFameScoreEraChange) / fameTreshold);
                FixedPoint currentStars = empire.EraStarsCount.Value;
                if (currentScore > currentStars)
                {
                    empire.EraStarsCount.Value += currentScore - currentStars;
                    empire.SumOfEraStars.Value += currentScore - currentStars;
                }
                
                Console.WriteLine("Empire {4} | Turns: {0} | Next Threshold: {1} | Current Stars {2} | Goal Stars: {3}", checkturn.ToString(), (majorSave.lastFameScoreEraChange + fameTreshold * (currentScore + 1)).ToString(),currentStars.ToString(), currentScore.ToString(), i.ToString());
                R.DepartmentOfDevelopment(empire).RefreshNextFactionInfos();
                
            }
        }
        public static FixedPoint GetFameThreshold(float gameSpeed)
        {
            int numEmpires = Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires;
            FixedPoint baseFame = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.FameBaseGain));
            float baseFameMulti = Convert.ToSingle(GameOptionHelper.GetGameOption(FameByScoring.FameGainMultiplier));
            int gameOptionTurns = Convert.ToInt32(GameOptionHelper.GetGameOption(FameByScoring.NumberScoringRounds));
            EmpireInfo empireInfo = R.Utils_GameUtils().GetCurrentEmpireInfo();
            FixedPoint eraStarsReq =  empireInfo.EraStarsRequirement;

            FixedPoint tmpFame = baseFame * baseFameMulti * 4 * 1.2f;  // 20 * 1 * 4 = 96; 4 = number of categories
            FixedPoint checkturn = (FixedPoint.Floor((FixedPoint)50 * gameSpeed / (gameOptionTurns * gameSpeed))); // 50 * 0,5 / 8 * 0,5 = 6
            FixedPoint fameTreshold = FixedPoint.Floor(tmpFame * checkturn / eraStarsReq); // 96 * 6 / 7 = 
            fameThresholdStep = fameTreshold;
            return fameTreshold;

        }
    }
}
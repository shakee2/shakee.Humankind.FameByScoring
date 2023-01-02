using System;
using Amplitude;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Sandbox;
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

                FixedPoint tmpFame = baseFame * baseFameMulti * 4;  // 20 * 1 * 4 = 80
                FixedPoint checkturn = (FixedPoint.Floor((FixedPoint)50 / gameOptionTurns)); // 50 / 4 = 12
                FixedPoint fameTreshold = FixedPoint.Floor(tmpFame * checkturn * gameSpeed); // 80 * 12 * 0,5 = 480                         
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
    }
}
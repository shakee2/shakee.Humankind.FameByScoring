using BepInEx;
using HarmonyLib;
using System;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using HumankindModTool;
using Amplitude;



namespace shakee.Humankind.FameByScoring
{
    [HarmonyPatch(typeof(SimulationEvent_NewTurnBegin))]
    public class NewTurn_Patch 
    {
        [HarmonyPatch("Raise")]    
        [HarmonyPostfix]   
        public static void Raise (SimulationEvent_NewTurnBegin __instance, object sender, ushort turn) 
        {
            //Console.WriteLine("New Turn Begin");
            if(GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "true"))
            {        
                EmpireBanner_FamePennant_Patch.SetupComponent();
                float gameSpeed;
                int gameOptionTurns = int.Parse(GameOptionHelper.GetGameOption(FameByScoring.NumberScoringRounds));
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


                int finalTurn = turnCheck;

                Console.WriteLine("Next TurnCheck {0}", finalTurn.ToString());
                if (GameOptionHelper.CheckGameOption(FameByScoring.EraStarSettingStars, "False"))
                {
                    FameThresholds.CheckThreshold();
                }   
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
            //Console.WriteLine("Era Change");
            if(GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "true"))
            {
                ScoringRound.RoundScoring(false, empireIndex: empireIndex);            
            }            
        }
    }
    [HarmonyPatch(typeof(SimulationEvent_TurnEnd))]
    public class SimulationEvent_TurnEnd_Patch
    {     
        [HarmonyPatch("Raise")]
        [HarmonyPostfix]
        public static void Raise (object sender, ushort turn)
        {
            //Console.WriteLine("Turn End");
        }
    }
    
    [HarmonyPatch(typeof(SimulationEvent_BattleTerminated))]
    public class SimulationEvent_BattleTerminated_Patch
    {
        [HarmonyPatch("Raise")]
        [HarmonyPrefix]
        public static bool Raise (object sender, Battle battle)
        {
            if (GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "true"))
            {
                int numEmpires = Sandbox.NumberOfMajorEmpires;
                int attackerIndex = battle.AttackerGroup().LeaderEmpireIndex();
                int defenderIndex = battle.DefenderGroup().LeaderEmpireIndex();
                BattleResult attackerResult = battle.AttackerGroup().Result();
                BattleResult defenderResult = battle.DefenderGroup().Result();

                if (attackerResult == BattleResult.Victory)
                {   
                    //Console.WriteLine("Attacker Win: " + attackerIndex.ToString() + " / " + defenderIndex.ToString());
                    if (attackerIndex < numEmpires && attackerIndex >= 0)
                        MajorEmpireSaveExtension.GetExtension(attackerIndex).CountBattle(1,1);
                    if (defenderIndex < numEmpires && defenderIndex >= 0)
                        MajorEmpireSaveExtension.GetExtension(defenderIndex).CountBattle(1,0);
                }
                else if (attackerResult == BattleResult.Defeat)
                {
                    //Console.WriteLine("Attacker Draw: " + attackerIndex.ToString() + " / " + defenderIndex.ToString());
                    if (attackerIndex < numEmpires && attackerIndex >= 0)
                        MajorEmpireSaveExtension.GetExtension(attackerIndex).CountBattle(1,0);
                    if (defenderIndex < numEmpires && defenderIndex >= 0)
                        MajorEmpireSaveExtension.GetExtension(defenderIndex).CountBattle(1,1);
                }
                else if (attackerResult == BattleResult.Draw)
                {
                    //Console.WriteLine("Attacker Loss: " + attackerIndex.ToString() + " / " + defenderIndex.ToString());
                    if (attackerIndex < numEmpires && attackerIndex >= 0)
                        MajorEmpireSaveExtension.GetExtension(attackerIndex).CountBattle(1,0);
                    if (defenderIndex < numEmpires && defenderIndex >= 0)
                        MajorEmpireSaveExtension.GetExtension(defenderIndex).CountBattle(1,0);
                }           
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(SimulationEvent_UnitKilledByOther))]
    public class SimulationEvent_UnitKilledByOther_Patch
    {
        [HarmonyPatch("Raise")]
        [HarmonyPostfix]
        public static void Raise (object sender, Unit killed, Empire aggressor)    
        {
            if (GameOptionHelper.CheckGameOption(FameByScoring.FameScoringOption, "true"))
            {
                int numEmpires = Sandbox.NumberOfMajorEmpires;
                if (aggressor.Index() < numEmpires && aggressor.Index() >= 0)
                {
                    if (aggressor.EraLevel.Value >= 1)
                    MajorEmpireSaveExtension.GetExtension(aggressor.Index()).killedUnits += 1;               
                }
            }
        }
    }
    [HarmonyPatch(typeof(EndGameController))]
    public class EndGameController_Patch
    {
        [HarmonyPatch("UpdateEndGameStatus")]
        [HarmonyPostfix]
        public static void UpdateEndGameStatus (EndGameController __instance)    
        {
            bool endGame = GameOptionHelper.CheckGameOption(FameByScoring.EndGameScoringSetting,"true");
            //Console.WriteLine("Check EndgameStatus");
            if (__instance.EndGameStatus() == EndGameStatus.LastTurn && endGame)
            {
                //Console.WriteLine("Last Turn");                
                ScoringRound.RoundScoring(true, endGame: endGame);
                //Console.WriteLine("End Game");
            }



        }
    }
}


using BepInEx;
using HarmonyLib;
using System;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using HumankindModTool;


namespace shakee.Humankind.FameByScoring
{
    
    [HarmonyPatch(typeof(SimulationEvent_BattleTerminated))]
    public class SimulationEvent_BattleTerminated_Patch
    {
        [HarmonyPatch("Raise")]
        [HarmonyPrefix]
        public static bool Raise (object sender, Battle battle)
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
            if (GameOptionHelper.GetGameOption(FameByScoring.FameScoringOption) != "0")
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
            if (__instance.EndGameStatus() == EndGameStatus.LastTurn)
            {
                ScoringRound.RoundScoring(true, endGame: true);
                Console.WriteLine("End Game");
            }
        }
    }
}


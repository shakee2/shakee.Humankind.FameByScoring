using System;
using System.Collections.Generic;
using HarmonyLib;
using Amplitude;
using Amplitude.Mercury.Simulation;
using Amplitude.Serialization;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;


namespace shakee.Humankind.FameByScoring
{
    [HarmonyPatch(typeof(StatisticReporter_BattleTerminated))]
    public class StatisticReporter_BattleTerminated_Patch
    {
        [HarmonyPatch("SimulationEventRaised_BattleTerminated")]
        [HarmonyPostfix]
        public static void SimulationEventRaised_BattleTerminated (StatisticReporter_BattleTerminated __instance, object sender, SimulationEvent_BattleTerminated battleTerminated)
        {
            int numEmpires = Sandbox.NumberOfMajorEmpires - 1;
            int attackerEmpire = battleTerminated.AttackerLeaderEmpireIndex();
            int defenderEmpire = battleTerminated.DefenderLeaderEmpireIndex();

            if (battleTerminated.AttackerResult() == BattleResult.Victory)
            {   
                if (attackerEmpire <= numEmpires)
                    MajorEmpireSaveExtension.GetExtension(attackerEmpire).CountBattle(1,1);
                if (defenderEmpire <= numEmpires)
                    MajorEmpireSaveExtension.GetExtension(defenderEmpire).CountBattle(1,0);
            }
            else if (battleTerminated.AttackerResult() == BattleResult.Draw)
            {
                if (attackerEmpire <= numEmpires)
                    MajorEmpireSaveExtension.GetExtension(attackerEmpire).CountBattle(1,0);
                if (defenderEmpire <= numEmpires)
                    MajorEmpireSaveExtension.GetExtension(defenderEmpire).CountBattle(1,0);
            }
            else
            {
                if (attackerEmpire <= numEmpires)
                    MajorEmpireSaveExtension.GetExtension(attackerEmpire).CountBattle(1,0);
                if (defenderEmpire <= numEmpires)
                    MajorEmpireSaveExtension.GetExtension(defenderEmpire).CountBattle(1,1);
            }
        }
    }
}
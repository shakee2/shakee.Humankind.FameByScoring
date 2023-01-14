using System;

using System.Collections.Generic;

using HarmonyLib;
using Amplitude;

using Amplitude.Mercury.Simulation;

using Amplitude.Serialization;


namespace shakee.Humankind.FameByScoring
{
    public class MajorEmpireExtension : ISerializable
	{
        public List<FameHistory> FameHistoryList = new List<FameHistory>();
        public MajorEmpire empire {get; set;}
        public int empireIndex {get; set;}
        public FixedPoint lastFameScoreEraChange {get; set;} // for setting up next Threshold after Era Change
        public int lastFameRankEraChange {get; set;}
        public FixedPoint lastFameGainEraChange {get; set;}
        public List<string> listRanking { get; set; } = new List<string> {
                "1st", 
                "2nd",
                "3rd",
                "4th",
                "5th",
                "6th",
                "7th",
                "8th",
                "9th",
                "10th",
        };
        public List<FameHistory> listScoring {get;set;}

		public MajorEmpireExtension(MajorEmpire empire)
		{
            lastFameScoreEraChange = 0;
            empireIndex = empire.Index();
            this.empire = empire;           
        }

        public void Serialize(Serializer serializer)        
        {			
            lastFameScoreEraChange = serializer.SerializeElement("lastFameScoreEraChange", lastFameScoreEraChange);
            listRanking = serializer.SerializeElement("listRanking", listRanking);
            //listScoring = serializer.SerializeElement("listScoring", listScoring);
            FameHistoryList = serializer.SerializeElement("FameHistoryList", FameHistoryList);
            //empire = serializer.SerializeElement("empire", empire);
            empireIndex = serializer.SerializeElement("empireIndex", empireIndex);
            lastFameRankEraChange = serializer.SerializeElement("lastFameRankEraChange", lastFameRankEraChange);
            lastFameGainEraChange = serializer.SerializeElement("lastFameGainEraChange", lastFameGainEraChange);
		}
        public void CheckDispose ()
        {
            
            if (FameHistoryList.Count > 6)
            {
                Console.WriteLine("List Scoring reached Limit {0}/6 -> cleanup. (Empire " + this.empire.Index().ToString() + ")", FameHistoryList.Count);
                FameHistory var2 = ScoringRound.GetHistory(empire, 0);
                var2.Dispose();
                FameHistoryList.Remove(FameHistoryList[0]);
            }
        }
    }

    public class FameHistory : ISerializable
    {
        public MajorEmpireExtension MajorEmpireExtension;
        public int empireIndex;
        public MajorEmpire empire;
        public FixedPoint fame;
        public int turn { get; set; }
        public int totalRank  { get; set; }
        public int[] categoryRank { get; set; } = new int[5] {
            0,
            0,
            0,
            0,
            0,            
            };
        public FameHistory()
        {

        }
        public FameHistory(MajorEmpire empire)
        {
            this.empireIndex = empire.Index();
            this.empire = empire;
            MajorEmpireExtension = MajorEmpireSaveExtension.GetExtension(empire.Index());
            MajorEmpireExtension.FameHistoryList.Add(this);
            turn = 0;
            fame = 0;

        }
        public void Serialize(Serializer serializer)
        {
            fame = serializer.SerializeElement("fame", fame);
            turn = serializer.SerializeElement("turn", turn);
            categoryRank = serializer.SerializeElement("categoryRank", categoryRank);
            totalRank = serializer.SerializeElement("totalRank", totalRank);
            //empire = serializer.SerializeElement("empire", empire);
		}
        ~FameHistory()
		{
			Dispose(disposing: false);
			GC.SuppressFinalize(this);
		}
        public void Dispose()
		{
			Dispose(disposing: true);
		}
        protected virtual void Dispose(bool disposing)
		{
		}
    }

	public static class MajorEmpireSaveExtension
    {
		public static IDictionary<int, MajorEmpireExtension> EmpireExtensionPerEmpireIndex;

		public static void OnSandboxStart()
        {
            EmpireExtensionPerEmpireIndex = new Dictionary<int, MajorEmpireExtension>();
		}

		public static void OnExitSandbox()
		{
			
			EmpireExtensionPerEmpireIndex = null;
		}

		public static MajorEmpireExtension GetExtension(int empireIndex)
        {
			return EmpireExtensionPerEmpireIndex[empireIndex];
		}
	}

	[HarmonyPatch(typeof(MajorEmpire))]
	public class MajorEmpire_Patch
	{

		//*
		[HarmonyPostfix]
		[HarmonyPatch(nameof(InitializeOnStart))]
		public static void InitializeOnStart(MajorEmpire __instance)
		{
            if (Sandbox_Patch.ModDefaultingOff == false)
            {       
                                     
                MajorEmpireExtension majorEmpireExtension = new MajorEmpireExtension(__instance);
                MajorEmpireSaveExtension.EmpireExtensionPerEmpireIndex.Add(__instance.Index(), majorEmpireExtension);			
            }
		}
		//*/

		[HarmonyPatch("Serialize")]
		[HarmonyPostfix]
		public static void Serialize(MajorEmpire __instance, Serializer serializer)
		{
            if (Sandbox_Patch.ModDefaultingOff == true)
            {
                return;
            }
			int empireIndex = __instance.Index();
			switch (serializer.SerializationMode)
			{
				case SerializationMode.Read:
					{
						MajorEmpireExtension majorEmpireExtension = serializer.SerializeElement("MajorEmpireExtension", new MajorEmpireExtension(__instance));
						MajorEmpireSaveExtension.EmpireExtensionPerEmpireIndex.Add(empireIndex, majorEmpireExtension);
						break;
					}
				case SerializationMode.Write:
					{
						MajorEmpireExtension majorEmpireExtension = MajorEmpireSaveExtension.EmpireExtensionPerEmpireIndex[empireIndex];
						serializer.SerializeElement("MajorEmpireExtension", majorEmpireExtension);
						break;
					}
			}
		}
	}
}
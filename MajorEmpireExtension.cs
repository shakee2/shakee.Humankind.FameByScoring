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
        public FixedPoint [,] lastFameScoring {get; set;}
        public FixedPoint lastFameGain {get; set;}
        public FixedPoint lastFameScoreEraChange {get; set; }
        public List<string> listRanking = new List<string> {
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
			lastFameGain = 0;
            lastFameScoreEraChange = 0;
            empireIndex = empire.Index();
            this.empire = empire;
            lastFameScoring = new FixedPoint[5,3];        
        }

        public void Serialize(Serializer serializer)        
        {			
            lastFameGain = serializer.SerializeElement("lastFameGain", lastFameGain);
            lastFameScoreEraChange = serializer.SerializeElement("lastFameScoreEraChange", lastFameScoreEraChange);
            listRanking = serializer.SerializeElement("listRanking", listRanking);
            //listScoring = serializer.SerializeElement("listScoring", listScoring);
            FameHistoryList = serializer.SerializeElement("FameHistoryList", FameHistoryList);
            //empire = serializer.SerializeElement("empire", empire);
            empireIndex = serializer.SerializeElement("empireIndex", empireIndex);
		}
        public void CheckDispose ()
        {
            
            if (FameHistoryList.Count > 4)
            {
                Console.WriteLine("List Scoring reached Limit {0}/4 -> cleanup. (Empire " + this.empire.Index().ToString() + ")", FameHistoryList.Count);
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
        public int turn;
        public string[] categoryRank = new string[4] {
            "none",
            "none",
            "none",
            "none",            
            };
        public FameHistory()
        {

        }
        public FameHistory(MajorEmpire empire)
        {
            this.empireIndex = empire.Index();
            MajorEmpireExtension = MajorEmpireSaveExtension.GetExtension(empire.Index());
            MajorEmpireExtension.FameHistoryList.Add(this);
            turn = 0;
            fame = 0;

        }
        public FameHistory(ref MajorEmpireExtension MajorEmpireExtension)
        {
            this.MajorEmpireExtension = MajorEmpireExtension;
            this.empireIndex = MajorEmpireExtension.empireIndex;
            MajorEmpireExtension.FameHistoryList.Add(this);
        }
        public void Serialize(Serializer serializer)
        {
            fame = serializer.SerializeElement("fame", fame);
            turn = serializer.SerializeElement("turn", turn);
            categoryRank = serializer.SerializeElement("categoryRank", categoryRank);
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
            Console.WriteLine("Initialize MajorEmpire ave Extension");
            MajorEmpireExtension majorEmpireExtension = new MajorEmpireExtension(__instance);
            MajorEmpireSaveExtension.EmpireExtensionPerEmpireIndex.Add(__instance.Index(), majorEmpireExtension);			

		}
		//*/

		[HarmonyPatch("Serialize")]
		[HarmonyPostfix]
		public static void Serialize(MajorEmpire __instance, Serializer serializer)
		{

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
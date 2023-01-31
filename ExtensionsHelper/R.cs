using System;
using System.Collections.Generic;
using System.Reflection;
using Amplitude;
using Amplitude.Framework.Simulation;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Sandbox;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.UI;
using Amplitude.Mercury.Data.Simulation;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.UI.Interactables;

namespace shakee.Humankind.FameByScoring
{
	// Token: 0x02000037 RID: 55
	public static class R
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00005E0C File Offset: 0x0000400C
		public static MajorEmpire majorEmpire(this DepartmentOfReligion self)
		{
			return (MajorEmpire)R.DepartmentOfReligion_majorEmpire_FieldInfo.GetValue(self);
		}
		public static MajorEmpire majorEmpire(this DepartmentOfDevelopment self)
		{
			return (MajorEmpire)R.DepartmentOfDevelopment_majorEmpire_FieldInfo.GetValue(self);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005E2E File Offset: 0x0000402E
		public static void majorEmpire(this DepartmentOfReligion self, MajorEmpire value)
		{
			R.DepartmentOfReligion_majorEmpire_FieldInfo.SetValue(self, value);
		}

		public static void majorEmpire(this DepartmentOfDevelopment self, MajorEmpire value)
		{
			R.DepartmentOfDevelopment_majorEmpire_FieldInfo.SetValue(self, value);
		}
		// Token: 0x06000107 RID: 263 RVA: 0x00005E40 File Offset: 0x00004040
		public static DepartmentOfScience DepartmentOfScience(this MajorEmpire self)
		{
			return (DepartmentOfScience)R.MajorEmpire_DepartmentOfScience_FieldInfo.GetValue(self);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005E62 File Offset: 0x00004062
		public static void DepartmentOfScience(this MajorEmpire self, DepartmentOfScience value)
		{
			R.MajorEmpire_DepartmentOfScience_FieldInfo.SetValue(self, value);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005E74 File Offset: 0x00004074
		public static bool IsTechnologyResearched(this DepartmentOfScience self, StaticString technologyName)
		{
			return (bool)R.DepartmentOfScience_IsTechnologyResearched_MethodInfo_StaticString.Invoke(self, new object[]
			{
				technologyName
			});
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005EA8 File Offset: 0x000040A8
		public static bool ContainsDescriptor(this BaseSimulationEntity self, StaticString descriptorName)
		{
			return (bool)R.BaseSimulationEntity_ContainsDescriptor_MethodInfo_StaticString.Invoke(self, new object[]
			{
				descriptorName
			});
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005ED9 File Offset: 0x000040D9
		public static void SimulationEvent_ReligionTenetChosen_Raise(object sender, Empire empire, Religion religion, int tenetIndex)
		{
			R.SimulationEvent_ReligionTenetChosen_Raise_MethodInfo_Object_Empire_Religion_Int32.Invoke(null, new object[]
			{
				sender,
				empire,
				religion,
				tenetIndex
			});
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005F04 File Offset: 0x00004104
		public static int EmpireIndex(this SimulationEvent_ReligionTenetChosen self)
		{
			return (int)R.SimulationEvent_ReligionTenetChosen_EmpireIndex_FieldInfo.GetValue(self);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005F26 File Offset: 0x00004126
		public static void EmpireIndex(this SimulationEvent_ReligionTenetChosen self, int value)
		{
			R.SimulationEvent_ReligionTenetChosen_EmpireIndex_FieldInfo.SetValue(self, value);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005F3C File Offset: 0x0000413C
		public static int ReligionIndex(this SimulationEvent_ReligionTenetChosen self)
		{
			return (int)R.SimulationEvent_ReligionTenetChosen_ReligionIndex_FieldInfo.GetValue(self);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005F5E File Offset: 0x0000415E
		public static void ReligionIndex(this SimulationEvent_ReligionTenetChosen self, int value)
		{
			R.SimulationEvent_ReligionTenetChosen_ReligionIndex_FieldInfo.SetValue(self, value);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005F74 File Offset: 0x00004174
		public static int TenetIndex(this SimulationEvent_ReligionTenetChosen self)
		{
			return (int)R.SimulationEvent_ReligionTenetChosen_TenetIndex_FieldInfo.GetValue(self);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005F96 File Offset: 0x00004196
		public static void TenetIndex(this SimulationEvent_ReligionTenetChosen self, int value)
		{
			R.SimulationEvent_ReligionTenetChosen_TenetIndex_FieldInfo.SetValue(self, value);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005FAB File Offset: 0x000041AB
		public static void RegisterVariableInternal(this DeedsManager self, string variableName, Type typeOfVariable, SimulationEntity[] simulationEntities)
		{
			R.DeedsManager_RegisterVariable_MethodInfo_String_Type_SimulationEntityArray.Invoke(self, new object[]
			{
				variableName,
				typeOfVariable,
				simulationEntities
			});
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005FCC File Offset: 0x000041CC
		public static void RegisterVariableInternal(this DeedsManager self, string variableName, Type typeOfVariable, SimulationEntity simulationEntity)
		{
			R.DeedsManager_RegisterVariable_MethodInfo_String_Type_SimulationEntity.Invoke(self, new object[]
			{
				variableName,
				typeOfVariable,
				simulationEntity
			});
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005FF0 File Offset: 0x000041F0
		public static int Index(this Empire self)
		{
			return (int)R.Empire_Index_FieldInfo.GetValue(self);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006012 File Offset: 0x00004212
		public static void Index(this Empire self, int value)
		{
			R.Empire_Index_FieldInfo.SetValue(self, value);
		}
		public static int Index(this MajorEmpire self)
		{
			return (int)R.MajorEmpire_Index_FieldInfo.GetValue(self);
		}
		public static ReferenceCollection<Settlement> Settlements(this MajorEmpire self)
		{
			return (ReferenceCollection<Settlement>)R.MajorEmpire_Settlements_FieldInfo.GetValue(self);
		}


		// Token: 0x06000116 RID: 278 RVA: 0x00006028 File Offset: 0x00004228

		// Token: 0x06000118 RID: 280 RVA: 0x0000606E File Offset: 0x0000426E
		public static void AddWinner(this CompetitiveDeedInfo self, int empireIndex, int turn)
		{
			R.CompetitiveDeedInfo_AddWinner_MethodInfo_Int32_Int32.Invoke(self, new object[]
			{
				empireIndex,
				turn
			});
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000609C File Offset: 0x0000429C
		public static DepartmentOfDevelopment DepartmentOfDevelopment(this MajorEmpire self)
		{
			return (DepartmentOfDevelopment)R.MajorEmpire_DepartmentOfDevelopment_FieldInfo.GetValue(self);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000060BE File Offset: 0x000042BE
		public static void DepartmentOfDevelopment(this MajorEmpire self, DepartmentOfDevelopment value)
		{
			R.MajorEmpire_DepartmentOfDevelopment_FieldInfo.SetValue(self, value);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000060CE File Offset: 0x000042CE
		public static void GainFame(this DepartmentOfDevelopment self, FixedPoint gain, bool raiseSimulationEvents = true)
		{
			R.DepartmentOfDevelopment_GainFame_MethodInfo_FixedPoint_Boolean.Invoke(self, new object[]
			{
				gain,
				raiseSimulationEvents
			});
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000060F8 File Offset: 0x000042F8
		public static DepartmentOfCommunication DepartmentOfCommunication(this Empire self)
		{
			return (DepartmentOfCommunication)R.Empire_DepartmentOfCommunication_FieldInfo.GetValue(self);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000611C File Offset: 0x0000431C
		public static int Sandbox_Frame()
		{
			return (int)R.Sandbox_Frame_FieldInfo.GetValue(null);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006140 File Offset: 0x00004340
		public static int nextNotificationId(this DepartmentOfCommunication self)
		{
			return (int)R.DepartmentOfCommunication_nextNotificationId_FieldInfo.GetValue(self);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006162 File Offset: 0x00004362
		public static void nextNotificationId(this DepartmentOfCommunication self, int value)
		{
			R.DepartmentOfCommunication_nextNotificationId_FieldInfo.SetValue(self, value);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006178 File Offset: 0x00004378
		public static Dictionary<Type, NotificationPriority> DepartmentOfCommunication_notificationPriorityByNotificationType()
		{
			return (Dictionary<Type, NotificationPriority>)R.DepartmentOfCommunication_notificationPriorityByNotificationType_FieldInfo.GetValue(null);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000619C File Offset: 0x0000439C
		public static List<Notification> Notifications(this DepartmentOfCommunication self)
		{
			return (List<Notification>)R.DepartmentOfCommunication_Notifications_FieldInfo.GetValue(self);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000061BE File Offset: 0x000043BE
		public static void Notifications(this DepartmentOfCommunication self, List<Notification> value)
		{
			R.DepartmentOfCommunication_Notifications_FieldInfo.SetValue(self, value);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000061D0 File Offset: 0x000043D0
		public static int NotificationFrame(this DepartmentOfCommunication self)
		{
			return (int)R.DepartmentOfCommunication_NotificationFrame_FieldInfo.GetValue(self);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000061F2 File Offset: 0x000043F2
		public static void NotificationFrame(this DepartmentOfCommunication self, int value)
		{
			R.DepartmentOfCommunication_NotificationFrame_FieldInfo.SetValue(self, value);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006208 File Offset: 0x00004408
		public static void DepartmentOfCommunication_Notify<T>(this DepartmentOfCommunication self, T data) where T : struct, INotificationData
		{
			List<Notification> Notifications = self.Notifications();
			Dictionary<Type, NotificationPriority> notificationPriorityByNotificationType = R.DepartmentOfCommunication_notificationPriorityByNotificationType();
			Notification<T> item = new Notification<T>
			{
				NotificationId = self.nextNotificationId(),
				Data = data,
				NotificationPriority = notificationPriorityByNotificationType[typeof(T)],
				NotificationStatus = 0,
				NotificationFlags = 0
			};
			self.nextNotificationId(self.nextNotificationId() + 1);
			Notifications.Add(item);
			self.NotificationFrame(R.Sandbox_Frame());
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006285 File Offset: 0x00004485
		public static void SimulationEvent_CompetitiveDeedWon_Raise(object sender, int majorEmpireIndex, int competitiveDeedIndex)
		{
			R.SimulationEvent_CompetitiveDeedWon_Raise_MethodInfo_Object_Int32_Int32.Invoke(null, new object[]
			{
				sender,
				majorEmpireIndex,
				competitiveDeedIndex
			});
		}

		public static void SimulationEvent_FameScoreChanged_Raise(object sender, MajorEmpire majorEmpire, int fameGain)
		{
			R.SimulationEvent_FameScoreChanged_Raise_MethodInfo.Invoke(null, new object[]
			{
				sender,
				majorEmpire,
				fameGain
			});
		}
		public static void SimulationEvent_EraStarEarned_Raise(object sender, StaticString eraStarDefinitionName, int unlockedLevel, int empireIndex)
		{
			R.SimulationEvent_EraStarEarned_Raise_MethodInfo.Invoke(null, new object[]
			{
				sender,
				eraStarDefinitionName,
				unlockedLevel,
				empireIndex,
			});
		}

		public static void SetupEvolutionPeriodicNotificationIfRequired()
		{
			R.DepartmentOfDevelopment_SetupEvolutionPeriodicNotificationIfRequired.Invoke(null, new object[]
			{
				
			});
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000062B0 File Offset: 0x000044B0
		public static void CreateCompetitiveDeedInfoAt(this CompetitiveDeedsManager self, StaticString competitiveDeedDefinitionName, StaticString deedDefinitionName, FixedPoint gain, int index, out CompetitiveDeedInfo result)
		{
			object r = R.CompetitiveDeedsManager_CreateCompetitiveDeedInfoAt_MethodInfo_StaticString_StaticString_FixedPoint_Int32.Invoke(self, new object[]
			{
				competitiveDeedDefinitionName,
				deedDefinitionName,
				gain,
				index
			});
			result = (CompetitiveDeedInfo)r;
		}
		public static void AccumulateFame(this DepartmentOfDevelopment self, FixedPoint gain, out DepartmentOfDevelopment result)
		{
			object r = R.DeptDevelop_AccumulateFame.Invoke(self, new object[]
			{

				gain,

			});
			result = (DepartmentOfDevelopment)r;
		}
		public static void RefreshNextFactionInfos(this DepartmentOfDevelopment self)
		{
			object r = R.DepartmentOfDevelopment_RefreshNextFactionInfos.Invoke(self, new object[]
			{

			});

		}
		
		public static void ComputeEraStarRewards(Amplitude.Mercury.Data.Simulation.EraStarDefinition eraStarDefinition, ref EraStarInfo eraStarInfo)
		{
			object r = R.DepartmentOfDevelopment_ComputeEraStarRewards.Invoke(null, new object[]
			{
				eraStarDefinition,
				eraStarInfo,
			});

		}
		
		// Token: 0x06000128 RID: 296 RVA: 0x00006304 File Offset: 0x00004504
		public static ReligionSnapshot ReligionSnapshot()
		{
			return (ReligionSnapshot)R.Snapshots_ReligionSnapshot_FieldInfo.GetValue(null);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006326 File Offset: 0x00004526
		public static void ReligionSnapshot(ReligionSnapshot value)
		{
			R.Snapshots_ReligionSnapshot_FieldInfo.SetValue(null, value);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006338 File Offset: 0x00004538
		public static GameUtils Utils_GameUtils()
		{
			return (GameUtils)R.Utils_GameUtils_FieldInfo.GetValue(null);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000635C File Offset: 0x0000455C
		public static TextUtils Utils_TextUtils()
		{
			return (TextUtils)R.Utils_TextUtils_FieldInfo.GetValue(null);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006380 File Offset: 0x00004580
		public static ReligionTenetsScreen_TenetItem.TooltipTarget tooltipTarget(this ReligionTenetsScreen_TenetItem self)
		{
			return (ReligionTenetsScreen_TenetItem.TooltipTarget)R.ReligionTenetsScreen_TenetItem_tooltipTarget_FieldInfo.GetValue(self);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000063A4 File Offset: 0x000045A4
		public static string Failures(this ReligionTenetsScreen_TenetItem.TooltipTarget self)
		{
			return (string)R.ReligionTenetsScreen_TenetItem_TooltipTarget_Failures_PropertyInfo.GetValue(self);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000063C6 File Offset: 0x000045C6
		public static void Failures(this ReligionTenetsScreen_TenetItem.TooltipTarget self, string value)
		{
			R.ReligionTenetsScreen_TenetItem_TooltipTarget_Failures_PropertyInfo.SetValue(self, value);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000063D6 File Offset: 0x000045D6
/* 		public static void AddFlagInternal(this FailureUtils.FailureTranslator<ReligionFailureFlags> self, ReligionFailureFlags flags, string localizationKey, int numberOfParams)
		{
			R.FailureTranslator_AddFlag_MethodInfo_ReligionFailureFlags_String_Int32.Invoke(self, new object[]
			{
				flags,
				localizationKey,
				numberOfParams
			});
		} */

		// Token: 0x0400006F RID: 111
		private static FieldInfo DepartmentOfReligion_majorEmpire_FieldInfo = typeof(DepartmentOfReligion).GetField("majorEmpire", BindingFlags.Instance | BindingFlags.NonPublic);

		private static FieldInfo DepartmentOfDevelopment_majorEmpire_FieldInfo = typeof(DepartmentOfDevelopment).GetField("majorEmpire", BindingFlags.Instance | BindingFlags.NonPublic);
		// Token: 0x04000070 RID: 112
		private static FieldInfo MajorEmpire_DepartmentOfScience_FieldInfo = typeof(MajorEmpire).GetField("DepartmentOfScience", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000071 RID: 113
		private static MethodInfo DepartmentOfScience_IsTechnologyResearched_MethodInfo_StaticString = typeof(DepartmentOfScience).GetMethod("IsTechnologyResearched", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(StaticString)
		}, null);

		// Token: 0x04000072 RID: 114
		private static MethodInfo BaseSimulationEntity_ContainsDescriptor_MethodInfo_StaticString = typeof(BaseSimulationEntity).GetMethod("ContainsDescriptor", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(StaticString)
		}, null);

		// Token: 0x04000073 RID: 115
		private static MethodInfo SimulationEvent_ReligionTenetChosen_Raise_MethodInfo_Object_Empire_Religion_Int32 = typeof(SimulationEvent_ReligionTenetChosen).GetMethod("Raise", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(object),
			typeof(Empire),
			typeof(Religion),
			typeof(int)
		}, null);

		// Token: 0x04000074 RID: 116
		private static FieldInfo SimulationEvent_ReligionTenetChosen_EmpireIndex_FieldInfo = typeof(SimulationEvent_ReligionTenetChosen).GetField("EmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000075 RID: 117
		private static FieldInfo SimulationEvent_ReligionTenetChosen_ReligionIndex_FieldInfo = typeof(SimulationEvent_ReligionTenetChosen).GetField("ReligionIndex", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000076 RID: 118
		private static FieldInfo SimulationEvent_ReligionTenetChosen_TenetIndex_FieldInfo = typeof(SimulationEvent_ReligionTenetChosen).GetField("TenetIndex", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000077 RID: 119
		private static MethodInfo DeedsManager_RegisterVariable_MethodInfo_String_Type_SimulationEntityArray = typeof(DeedsManager).GetMethod("RegisterVariable", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(string),
			typeof(Type),
			typeof(SimulationEntity[])
		}, null);

		// Token: 0x04000078 RID: 120
		private static MethodInfo DeedsManager_RegisterVariable_MethodInfo_String_Type_SimulationEntity = typeof(DeedsManager).GetMethod("RegisterVariable", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(string),
			typeof(Type),
			typeof(SimulationEntity)
		}, null);

		// Token: 0x04000079 RID: 121
		private static FieldInfo Empire_Index_FieldInfo = typeof(Empire).GetField("Index", BindingFlags.Instance | BindingFlags.NonPublic);

		private static FieldInfo MajorEmpire_Index_FieldInfo = typeof(MajorEmpire).GetField("Index", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo MajorEmpire_Settlements_FieldInfo = typeof(MajorEmpire).GetField("Settlements", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400007A RID: 122
		

		// Token: 0x0400007C RID: 124
		private static MethodInfo CompetitiveDeedInfo_AddWinner_MethodInfo_Int32_Int32 = typeof(CompetitiveDeedInfo).GetMethod("AddWinner", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(int),
			typeof(int)
		}, null);

		// Token: 0x0400007D RID: 125
		private static FieldInfo MajorEmpire_DepartmentOfDevelopment_FieldInfo = typeof(MajorEmpire).GetField("DepartmentOfDevelopment", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400007E RID: 126
		private static MethodInfo DepartmentOfDevelopment_GainFame_MethodInfo_FixedPoint_Boolean = typeof(DepartmentOfDevelopment).GetMethod("GainFame", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(FixedPoint),
			typeof(bool)
		}, null);

		// Token: 0x0400007F RID: 127
		private static FieldInfo Empire_DepartmentOfCommunication_FieldInfo = typeof(Empire).GetField("DepartmentOfCommunication", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000080 RID: 128
		private static FieldInfo Sandbox_Frame_FieldInfo = typeof(Sandbox).GetField("Frame", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000081 RID: 129
		private static FieldInfo DepartmentOfCommunication_nextNotificationId_FieldInfo = typeof(DepartmentOfCommunication).GetField("nextNotificationId", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000082 RID: 130
		private static FieldInfo DepartmentOfCommunication_notificationPriorityByNotificationType_FieldInfo = typeof(DepartmentOfCommunication).GetField("notificationPriorityByNotificationType", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000083 RID: 131
		private static FieldInfo DepartmentOfCommunication_Notifications_FieldInfo = typeof(DepartmentOfCommunication).GetField("Notifications", BindingFlags.Instance | BindingFlags.Public);

		// Token: 0x04000084 RID: 132
		private static FieldInfo DepartmentOfCommunication_NotificationFrame_FieldInfo = typeof(DepartmentOfCommunication).GetField("NotificationFrame", BindingFlags.Instance | BindingFlags.Public);

		// Token: 0x04000085 RID: 133
		private static MethodInfo SimulationEvent_CompetitiveDeedWon_Raise_MethodInfo_Object_Int32_Int32 = typeof(SimulationEvent_CompetitiveDeedWon).GetMethod("Raise", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(object),
			typeof(int),
			typeof(int)
		}, null);

		private static MethodInfo SimulationEvent_FameScoreChanged_Raise_MethodInfo = typeof(SimulationEvent_FameScoreChanged).GetMethod("Raise", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(object),
			typeof(MajorEmpire),
			typeof(int)
		}, null);
		private static MethodInfo SimulationEvent_EraStarEarned_Raise_MethodInfo = typeof(SimulationEvent_EraStarEarned).GetMethod("Raise", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(object),
			typeof(StaticString),
			typeof(int),
			typeof(int)
		}, null);

		// Token: 0x04000086 RID: 134
		private static MethodInfo CompetitiveDeedsManager_CreateCompetitiveDeedInfoAt_MethodInfo_StaticString_StaticString_FixedPoint_Int32 = typeof(CompetitiveDeedsManager).GetMethod("CreateCompetitiveDeedInfoAt", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(StaticString),
			typeof(StaticString),
			typeof(FixedPoint),
			typeof(int)
		}, null);

		private static MethodInfo DeptDevelop_AccumulateFame = typeof(DepartmentOfDevelopment).GetMethod("AccumulateFame", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(FixedPoint),
		}, null);
		private static MethodInfo DepartmentOfDevelopment_RefreshNextFactionInfos = typeof(DepartmentOfDevelopment).GetMethod("RefreshNextFactionInfos", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{

		}, null);
		
		private static MethodInfo DepartmentOfDevelopment_ComputeEraStarRewards = typeof(DepartmentOfDevelopment).GetMethod("ComputeEraStarRewards", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(object),
			typeof(object),
		}, null);
		
		private static MethodInfo DepartmentOfDevelopment_SetupEvolutionPeriodicNotificationIfRequired = typeof(DepartmentOfDevelopment).GetMethod("SetupEvolutionPeriodicNotificationIfRequired", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
		
		}, null);


		// Token: 0x04000087 RID: 135
		private static FieldInfo Snapshots_ReligionSnapshot_FieldInfo = typeof(Snapshots).GetField("ReligionSnapshot", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000088 RID: 136
		private static FieldInfo Utils_GameUtils_FieldInfo = typeof(Utils).GetField("GameUtils", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000089 RID: 137
		private static FieldInfo Utils_TextUtils_FieldInfo = typeof(Utils).GetField("TextUtils", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x0400008A RID: 138
		private static FieldInfo ReligionTenetsScreen_TenetItem_tooltipTarget_FieldInfo = typeof(ReligionTenetsScreen_TenetItem).GetField("tooltipTarget", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400008B RID: 139
		private static PropertyInfo ReligionTenetsScreen_TenetItem_TooltipTarget_Failures_PropertyInfo = typeof(ReligionTenetsScreen_TenetItem.TooltipTarget).GetProperty("Failures", BindingFlags.Instance | BindingFlags.Public);

		// Token: 0x0400008C RID: 140
/* 		private static MethodInfo FailureTranslator_AddFlag_MethodInfo_ReligionFailureFlags_String_Int32 = typeof(FailureUtils.FailureTranslator).MakeGenericType(new Type[]
		{
			typeof(ReligionFailureFlags)
		}).GetMethod("AddFlag", BindingFlags.Instance | BindingFlags.Public, null, new Type[]
		{
			typeof(ReligionFailureFlags),
			typeof(string),
			typeof(int)
		}, null); */

#region District
		private static FieldInfo Settlements_Districts_FieldInfo = typeof(Settlement).GetField("Districts", BindingFlags.Instance | BindingFlags.NonPublic);

		public static ReferenceCollection<District> Districts(this Settlement self)
		{
			return (ReferenceCollection<District>)R.Settlements_Districts_FieldInfo.GetValue(self);
		}
		private static FieldInfo StatisticsController_EmpireStatistics = typeof(GameStatisticsController).GetField("EmpireStatistics", BindingFlags.Instance | BindingFlags.NonPublic);
		
		public static EmpireStatistics[] EmpireStatistics (this GameStatisticsController self)
		{
			return (EmpireStatistics[])R.StatisticsController_EmpireStatistics.GetValue(self);
		}

#endregion
# region GUI
		//[RequireComponent(typeof(Amplitude.UI.UIHierarchyManager))]
		private static FieldInfo UITooltipManager_HoveredTooltip = typeof(UITooltipManager).GetField("currentlyHoveredTooltip", BindingFlags.Instance | BindingFlags.NonPublic);

		public static UITooltip currentlyHoveredTooltip(this UITooltipManager self)
		{
			return (UITooltip)R.UITooltipManager_HoveredTooltip.GetValue(self);
		}


#endregion
#region battle
		private static FieldInfo Battle_List_EmpiresOrdered = typeof(Battle).GetField("EmpiresOrdered", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo Battle_Dictionary_DeadUnitsByEmpireIndex = typeof(Battle).GetField("DeadUnitsByEmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo Battle_AftermathInfo = typeof(Battle).GetField("AftermathInfo", BindingFlags.Instance | BindingFlags.NonPublic);

		public static List<Empire> EmpiresOrdered(this Battle self)
		{
			return (List<Empire>)R.Battle_List_EmpiresOrdered.GetValue(self);
		}
		public static Dictionary<int, List<BattleUnit>> DeadUnitsByEmpireIndex(this Battle self)
		{
			return (Dictionary<int, List<BattleUnit>>)R.Battle_Dictionary_DeadUnitsByEmpireIndex.GetValue(self);
		}
		public static BattleAftermathInfo AftermathInfo(this Battle self)
		{
			return (BattleAftermathInfo)R.Battle_AftermathInfo.GetValue(self);
		}

		


#endregion
#region Sandbox

		private static FieldInfo SandboxManager_Sandbox_FieldInfo = typeof(SandboxManager).GetField("Sandbox", BindingFlags.Static | BindingFlags.NonPublic);

		private static PropertyInfo Sandbox_Turn_PropertyInfo = typeof(Sandbox).GetProperty("Turn", BindingFlags.Instance | BindingFlags.NonPublic);
		private static PropertyInfo Sandbox_SandboxThreadStartSettings_PropertyInfo = typeof(Sandbox).GetProperty("SandboxThreadStartSettings", BindingFlags.Instance | BindingFlags.NonPublic);
		private static PropertyInfo SandboxThreadStartSettings_Parameter_PropertyInfo = typeof(SandboxThreadStartSettings).GetProperty("Parameter", BindingFlags.Instance | BindingFlags.NonPublic);

		public static Sandbox SandboxManager_Sandbox()
		{
			return (Sandbox)R.SandboxManager_Sandbox_FieldInfo.GetValue(null);
		}
		public static int Turn(this Sandbox self)
		{
			return (int)R.Sandbox_Turn_PropertyInfo.GetValue(self);
		}
		public static SandboxThreadStartSettings SandboxThreadStartSettings(Sandbox self)
		{
			return (SandboxThreadStartSettings)R.Sandbox_SandboxThreadStartSettings_PropertyInfo.GetValue(self);
		}
		public static void SandboxThreadStartSettings(Sandbox self, object parameter)
		{
			R.Sandbox_SandboxThreadStartSettings_PropertyInfo.SetValue(self, parameter);
		}
		public static object Parameter(this SandboxThreadStartSettings self)
		{
			return (object)R.SandboxThreadStartSettings_Parameter_PropertyInfo.GetValue(self);
		}


		
#endregion
#region Simlulation Events
		// internal int AttackerLeaderEmpireIndex;

		// internal int DefenderLeaderEmpireIndex;

		// internal BattleResult AttackerResult;

		// internal BattleResult DefenderResult;

		// internal BattleVictoryType VictoryType;

		// internal Battle Battle;

		private static FieldInfo SimulationEvent_BattleTerminated_AttackerLeaderEmpireIndex_FieldInfo = typeof(SimulationEvent_BattleTerminated).GetField("AttackerLeaderEmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo SimulationEvent_BattleTerminated_DefenderLeaderEmpireIndex_FieldInfo = typeof(SimulationEvent_BattleTerminated).GetField("DefenderLeaderEmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo SimulationEvent_BattleTerminated_AttackerResult_FieldInfo = typeof(SimulationEvent_BattleTerminated).GetField("AttackerResult", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo SimulationEvent_BattleTerminated_DefenderResult_FieldInfo = typeof(SimulationEvent_BattleTerminated).GetField("DefenderResult", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo SimulationEvent_BattleTerminated_VictoryType_FieldInfo = typeof(SimulationEvent_BattleTerminated).GetField("VictoryType", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo SimulationEvent_BattleTerminated_Battle_FieldInfo = typeof(SimulationEvent_BattleTerminated).GetField("Battle", BindingFlags.Instance | BindingFlags.NonPublic);
	
		public static int AttackerLeaderEmpireIndex(this SimulationEvent_BattleTerminated self)
		{
			return (int)R.SimulationEvent_BattleTerminated_AttackerLeaderEmpireIndex_FieldInfo.GetValue(self);
		}
		public static int DefenderLeaderEmpireIndex(this SimulationEvent_BattleTerminated self)
		{
			return (int)R.SimulationEvent_BattleTerminated_DefenderLeaderEmpireIndex_FieldInfo.GetValue(self);
		}
		public static BattleResult AttackerResult(this SimulationEvent_BattleTerminated self)
		{
			return (BattleResult)R.SimulationEvent_BattleTerminated_AttackerResult_FieldInfo.GetValue(self);
		}
		public static BattleResult DefenderResult(this SimulationEvent_BattleTerminated self)
		{
			return (BattleResult)R.SimulationEvent_BattleTerminated_DefenderResult_FieldInfo.GetValue(self);
		}
		public static BattleVictoryType VictoryType(this SimulationEvent_BattleTerminated self)
		{
			return (BattleVictoryType)R.SimulationEvent_BattleTerminated_VictoryType_FieldInfo.GetValue(self);
		}
		public static Battle Battle(this SimulationEvent_BattleTerminated self)
		{
			return (Battle)R.SimulationEvent_BattleTerminated_Battle_FieldInfo.GetValue(self);
		}

		private static FieldInfo Battle_AttackerGroup_FieldInfo = typeof(Battle).GetField("AttackerGroup", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo Battle_DefenderGroup_FieldInfo = typeof(Battle).GetField("DefenderGroup", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo BattleGroup_LeaderEmpireIndex_FieldInfo = typeof(BattleGroup).GetField("LeaderEmpireIndex", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo BattleGroup_Result_FieldInfo = typeof(BattleGroup).GetField("Result", BindingFlags.Instance | BindingFlags.NonPublic);
		public static BattleGroup AttackerGroup(this Battle self)
		{
			return (BattleGroup)R.Battle_AttackerGroup_FieldInfo.GetValue(self);
		}
		public static BattleGroup DefenderGroup(this Battle self)
		{
			return (BattleGroup)R.Battle_DefenderGroup_FieldInfo.GetValue(self);
		}
		public static int LeaderEmpireIndex(this BattleGroup self)
		{
			return (int)R.BattleGroup_LeaderEmpireIndex_FieldInfo.GetValue(self);
		}
		public static BattleResult Result(this BattleGroup self)
		{
			return (BattleResult)R.BattleGroup_Result_FieldInfo.GetValue(self);
		}
		private static FieldInfo EndGameController_EndGameStatus = typeof(EndGameController).GetField("EndGameStatus", BindingFlags.Instance | BindingFlags.NonPublic);
		public static EndGameStatus EndGameStatus(this EndGameController self)
		{
			return (EndGameStatus)R.EndGameController_EndGameStatus.GetValue(self);
		}

#endregion



	}
}

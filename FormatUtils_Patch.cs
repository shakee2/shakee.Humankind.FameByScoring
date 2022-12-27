using System;
using UnityEngine;
using HarmonyLib;
using Amplitude;
using Amplitude.Mercury;
using Amplitude.Mercury.UI.Helpers;

namespace shakee.Humankind.FameByScoring
{
	
    //[HarmonyPatch(typeof(FormatUtils))]

/*     public class FormatUtils_Patch
    {
		
        [HarmonyPatch("FormatAmount")]    
        [HarmonyPostfix]  
        
        public void FormatAmount(string __result, FixedPoint amount, bool handleHighAmount = false, Rounding rounding = Rounding.Floor, bool signed = false, int decimals = 0, bool percentage = false)
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
            decimals = 2;
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

        [HarmonyPatch("FormatAmount")]    
        [HarmonyPostfix]  
        public void FormatAmount(string __result, int amount, bool signed = false)
		{
            Console.WriteLine("[shakee.Tooltip.Fix] Done");
			switch (amount)
			{
			case int.MaxValue:
				if (!signed)
				{
					__result = TextUtils.InfinityCharacter;
				}
				__result =  TextUtils.PositiveInfinity;
                break;
			case int.MinValue:
				__result =  TextUtils.NegativeInfinity;
                break;
			default:
				if (!signed)
				{
					__result =  amount.ToString();
				}
				__result =  amount.ToString("+0;-#");
                break;
			}
		}

    } */

   
}




using Amplitude;
using Amplitude.Mercury.Simulation;


namespace shakee.Humankind.FameByScoring
{
    public class FameByScoring_Helper
    {
		public static void AccumulateFame_New(MajorEmpire majorEmpire, FixedPoint gain)
		{
			FixedPoint fixedPoint = gain * (1 + majorEmpire.FameGainBonus.Value);
			FixedPoint value = majorEmpire.FameScore.Value;
			if (fixedPoint > 0)
			{
				if (value > 0 && fixedPoint > FixedPoint.MaxValue - value)
				{
					return;
				}
			}
			else if (value < 0 && fixedPoint < FixedPoint.MinValue - value)
			{
				return;
			}
			majorEmpire.FameScore.Value = value + fixedPoint;
		}
    }
}
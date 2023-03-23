using MessagePack;

namespace KitchenLib.Stats
{
	[MessagePackObject]
	public class IntStat : BaseStat<int>
	{
		public void Add(int value)
		{
			Set(Get() + value);
		}
	}

	[MessagePackObject]
	public class FloatStat : BaseStat<float>
	{
		public void Add(float value)
		{
			Set(Get() + value);
		}
	}

	[MessagePackObject]
	public class BoolStat : BaseStat<bool> { }

	[MessagePackObject]
	public class StringStat : BaseStat<string> { }

	[MessagePackObject]
	public class IntDailyStat : BaseDailyStat<int>
	{
		public void Add(int day, int value)
		{
			Set(day, Get(day) + value);
		}
	}

	[MessagePackObject]
	public class FloatDailyStat : BaseDailyStat<float>
	{
		public void Add(int day, float value)
		{
			Set(day, Get(day) + value);
		}
	}
}

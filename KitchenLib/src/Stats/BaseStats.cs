using MessagePack;
using System;
using System.Collections.Generic;

namespace KitchenLib.Stats
{
	[AutoUnion]
	public abstract class BaseStat
	{
		[Key(0)]
		public string Key { get; internal set; }

		[Key(1)]
		public int? ID { get; internal set; }

		[Key(2)]
		public bool IsPlayerSpecific { get; internal set; } = false;
	}

	[AutoUnion]
	public abstract class BaseStat<T>: BaseStat
	{
		[Key(3)]
		[Obsolete("Do not use this directly, use Get() and Set().")]
		public T Value { get; set; }

		public T Get()
		{
			return Value;
		}

		public void Set(T value)
		{
			Value = value;
		}
	}

	public abstract class BaseDailyStat<T> : BaseStat<List<T>>
	{
		[Key(2)]
		internal List<T> Value { get; set; }

		public T Get(int day)
		{
			return Value[day];
		}

		public void Set(int day, T value)
		{
			while (day < Value.Count)
			{
				Value.Add(default);
			}

			Value[day] = value;
		}
	}
}

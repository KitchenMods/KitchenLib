using System;
using System.Collections.Generic;
using System.Linq;

namespace KitchenLib.Utils
{
	public static class ListUtils
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
		{
			return list == null || list.Count() == 0;
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			foreach (var item in list)
			{
				action(item);
			}
		}
	}
}

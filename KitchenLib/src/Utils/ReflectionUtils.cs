using System;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Utils
{
	public class ReflectionUtils
	{
		private static Dictionary<(Type, string), MethodInfo> cachedMethods = new Dictionary<(Type, string), MethodInfo>();

		public static MethodInfo GetMethod<T>(string methodName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
		{
			var tuple = (typeof(T), methodName);
			if (cachedMethods.TryGetValue(tuple, out var cachedVal))
				return cachedVal;
			cachedMethods[tuple] = typeof(T).GetMethod(methodName, flags);
			return cachedMethods[tuple];
		}
	}
}

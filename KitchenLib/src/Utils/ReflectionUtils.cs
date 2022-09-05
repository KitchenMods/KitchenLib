using System;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Utils
{
	public class ReflectionUtils
	{
		private static Dictionary<(Type, string), MethodInfo> cachedMethods = new Dictionary<(Type, string), MethodInfo>();

		public static Dictionary<(Type, string), FieldInfo> cachedFields = new Dictionary<(Type, string), FieldInfo>();

		public static MethodInfo GetMethod<T>(string methodName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
		{
			var tuple = (typeof(T), methodName);
			if (cachedMethods.TryGetValue(tuple, out var cachedVal))
				return cachedVal;
			cachedMethods[tuple] = typeof(T).GetMethod(methodName, flags);
			return cachedMethods[tuple];
		}
		public static FieldInfo GetField<T>(string fieldName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
		{
			var tuple = (typeof(T), fieldName);
			if (cachedFields.TryGetValue(tuple, out var cachedVal))
				return cachedVal;
			cachedFields[tuple] = typeof(T).GetField(fieldName, flags);
			return cachedFields[tuple];
		}
	}
}

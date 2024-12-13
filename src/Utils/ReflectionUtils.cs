using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Utils
{
	public class ReflectionUtils
	{
		private static Dictionary<(Type, string), MethodInfo> cachedMethods = new Dictionary<(Type, string), MethodInfo>();

		public static Dictionary<(Type, string), FieldInfo> cachedFields = new Dictionary<(Type, string), FieldInfo>();

		public static MethodInfo GetMethod(Type type, string methodName)
		{
			var tuple = (type, methodName);
			if (cachedMethods.TryGetValue(tuple, out var cachedVal))
				return cachedVal;
			cachedMethods[tuple] = AccessTools.Method(type, methodName);
			return cachedMethods[tuple];
		}
		
		public static FieldInfo GetField(Type type, string fieldName)
		{
			var tuple = (type, fieldName);
			if (cachedFields.TryGetValue(tuple, out var cachedVal))
				return cachedVal;
			cachedFields[tuple] = AccessTools.Field(type, fieldName);
			return cachedFields[tuple];
		}
		
		public static MethodInfo GetMethod<T>(string methodName)
		{
			var tuple = (typeof(T), methodName);
			if (cachedMethods.TryGetValue(tuple, out var cachedVal))
				return cachedVal;
			cachedMethods[tuple] = AccessTools.Method(typeof(T), methodName);
			return cachedMethods[tuple];
		}
		public static FieldInfo GetField<T>(string fieldName)
		{
			var tuple = (typeof(T), fieldName);
			if (cachedFields.TryGetValue(tuple, out var cachedVal))
				return cachedVal;
			cachedFields[tuple] = AccessTools.Field(typeof(T), fieldName);
			return cachedFields[tuple];
		}
		
		[Obsolete("Please use GetMethod<T>(string) instead")]
		public static MethodInfo GetMethod<T>(string methodName, BindingFlags flags)
		{
			return GetMethod<T>(methodName);
		}
		
		[Obsolete("Please use GetField<T>(string) instead")]
		public static FieldInfo GetField<T>(string fieldName, BindingFlags flags)
		{
			return GetField<T>(fieldName);
		}
		
		[Obsolete("Please use GetField(type, string) instead")]
		public static FieldInfo GetField(Type type, string fieldName, BindingFlags flags)
		{
			return GetField(type, fieldName);
		}
	}
}

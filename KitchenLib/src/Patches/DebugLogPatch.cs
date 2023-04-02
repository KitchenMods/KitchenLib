using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KitchenLib.Patches
{

	[HarmonyPatch]
	internal class DebugLogPatch
	{
		static IEnumerable<MethodBase> TargetMethods()
		{
			foreach (var method in AccessTools.GetDeclaredMethods(typeof(Logger)))
			{
				if (method.Name.Contains("Log") && method.GetParameters().Any(param => param.ParameterType == typeof(LogType)) && method.GetParameters().Any(param => param.ParameterType == typeof(object) && param.Name == "message"))
				{
					yield return method;
				}
			}
		}

		static void Prefix(LogType logType, ref object message)
		{
			var typePrefix = "";
			switch (logType)
			{
				case LogType.Error:
					typePrefix = "ERROR";
					break;
				case LogType.Assert:
					typePrefix = "ASSERT";
					break;
				case LogType.Warning:
					typePrefix = "WARN";
					break;
				case LogType.Log:
					typePrefix = "INFO";
					break;
				case LogType.Exception:
					typePrefix = "EXCEPTION";
					break;
			}

			var modIdPrefix = "";
			if (!Regex.IsMatch(message.ToString(), "\\[.+\\]"))
			{
				modIdPrefix = "[PlateUp!] ";
			}

			message = $"[{typePrefix}] " + modIdPrefix + message.ToString().Replace(Environment.UserName, "[USERNAME]");
		}
	}
}

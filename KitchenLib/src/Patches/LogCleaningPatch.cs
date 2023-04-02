using HarmonyLib;
using System;
using UnityEngine;

namespace KitchenLib.Patches
{

	[HarmonyPatch(typeof(Debug), "LogWarning", new[] { typeof(object) })]
	internal class LogCleaningPatch
	{
		static void Prefix(ref object message)
		{
			message = message.ToString().Replace(Environment.UserName, "[USERNAME]");
		}
	}
}

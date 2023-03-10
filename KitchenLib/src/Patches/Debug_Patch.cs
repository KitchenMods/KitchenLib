using HarmonyLib;
using System;
using UnityEngine;

namespace KitchenLib.Patches
{

	[HarmonyPatch(typeof(Debug), "LogWarning", new[] { typeof(object) })]
	public class Debug_Patch
	{
		static void Prefix(ref object message)
		{
			message = message.ToString().Replace(Environment.UserName, "[USERNAME]");
		}
	}
}

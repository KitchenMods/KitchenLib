using HarmonyLib;
using KitchenData;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(Unlock))]
	public class Unlock_Patch
	{

		private static Dictionary<int, Color> ColourOverrides = new Dictionary<int, Color>();
		private static Dictionary<int, string> IconOverrides = new Dictionary<int, string>();

		public static void AddColourOverride(int id, Color colour)
		{
			ColourOverrides[id] = colour;
		}
		public static void RemoveColourOverride(int id)
		{
			ColourOverrides.Remove(id);
		}
		public static void AddIconOverride(int id, string icon)
		{
			IconOverrides[id] = icon;
		}
		public static void RemoveIconOverride(int id)
		{
			IconOverrides.Remove(id);
		}

		[HarmonyPatch("get_Icon")]
		public static void Postfix(Unlock __instance, ref string __result)
		{
			if (IconOverrides.ContainsKey(__instance.ID))
				__result = IconOverrides[__instance.ID];
		}
		[HarmonyPatch("get_Colour")]
		public static void Postfix(Unlock __instance, ref Color __result)
		{
			if (ColourOverrides.ContainsKey(__instance.ID))
				__result = ColourOverrides[__instance.ID];
		}
	}
}

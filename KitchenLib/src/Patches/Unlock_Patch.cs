using HarmonyLib;
using KitchenData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(Unlock))]
	public class Unlock_Patch
	{

		public static Dictionary<int, Color> ColourOverrides = new Dictionary<int, Color>();
		public static Dictionary<int, string> IconOverrides = new Dictionary<int, string>();

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

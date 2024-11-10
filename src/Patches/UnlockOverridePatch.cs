using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(Unlock))]
	internal class UnlockOverridePatch
	{
		[HarmonyPatch("get_Icon")]
		static void Postfix(Unlock __instance, ref string __result)
		{
			if (UnlockOverrides.IconOverrides.ContainsKey(__instance.ID))
				__result = UnlockOverrides.IconOverrides[__instance.ID];
		}

		[HarmonyPatch("get_Colour")]
		static void Postfix(Unlock __instance, ref Color __result)
		{
			if (UnlockOverrides.ColourOverrides.ContainsKey(__instance.ID))
				__result = UnlockOverrides.ColourOverrides[__instance.ID];
		}
	}
}

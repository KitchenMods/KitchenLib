using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Patches
{
	public class GameDataObjectOverrides
	{
		[HarmonyPatch(typeof(Appliance), "SetupForGame")]
		public class ApplianceCostOverridePatch
		{
			static void Postfix(Appliance __instance)
			{
				if (ApplianceOverrides.PurchaseCostOverrides.ContainsKey(__instance.ID))
				{
					__instance.PurchaseCost = ApplianceOverrides.PurchaseCostOverrides[__instance.ID];
				}
			}
		}
		
		[HarmonyPatch(typeof(Item), "SetupForGame")]
		public class ItemRewardOverridePatch
		{
			static void Postfix(Item __instance)
			{
				if (ItemOverrides.RewardOverrides.ContainsKey(__instance.ID))
				{
					__instance.Reward = ItemOverrides.RewardOverrides[__instance.ID];
				}
			}
		}
		
		[HarmonyPatch(typeof(Unlock))]
		public class UnlockOverridePatch
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
}
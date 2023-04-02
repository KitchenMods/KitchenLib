using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(Item), "SetupForGame")]
	internal class ItemRewardOverridePatch
	{
		static void Postfix(Item __instance)
		{
			if (ItemOverrides.RewardOverrides.ContainsKey(__instance.ID))
			{
				__instance.Reward = ItemOverrides.RewardOverrides[__instance.ID];
			}
		}
	}
}

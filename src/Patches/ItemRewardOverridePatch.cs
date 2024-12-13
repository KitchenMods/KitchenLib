using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;

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

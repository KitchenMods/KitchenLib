using System.Collections.Generic;
using KitchenData;
using HarmonyLib;

namespace KitchenLib
{
	[HarmonyPatch(typeof(Item), "SetupForGame")]
	public class Item_Patch
	{
		private static Dictionary<int, int> RewardOverrides = new Dictionary<int, int>();
		public static void AddRewardOverride(int id, int reward)
		{
			RewardOverrides[id] = reward;
		}
		public static void RemoveRewardOverride(int id)
		{
			RewardOverrides.Remove(id);
		}
		static void Postfix(Item __instance)
		{
			if (RewardOverrides.ContainsKey(__instance.ID))
			{
				__instance.Reward = RewardOverrides[__instance.ID];
			}
		}
	}
}

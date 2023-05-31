using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(Appliance), "SetupForGame")]
	internal class ApplianceCostOverridePatch
	{
		static void Postfix(Appliance __instance)
		{
			if (ApplianceOverrides.PurchaseCostOverrides.ContainsKey(__instance.ID))
			{
				__instance.PurchaseCost = ApplianceOverrides.PurchaseCostOverrides[__instance.ID];
			}
		}
	}
}

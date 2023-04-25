using HarmonyLib;
using Kitchen;
using KitchenLib.References;
using KitchenLib.Systems;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(GrantNecessaryAppliances), "TotalPlates")]
	internal class GrantNecessaryAppliances_Patch
	{
		static void Postfix(ref int __result)
		{
			if (!GrantNecessarySystem.GameRequiresItem(ItemReferences.Plate))
				__result = 99;
		}
	}
}

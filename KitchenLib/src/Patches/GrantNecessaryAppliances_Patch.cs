using HarmonyLib;
using Kitchen;
using KitchenLib.References;
using KitchenLib.src.Systems;

namespace KitchenLib.src.Patches
{
	[HarmonyPatch(typeof(GrantNecessaryAppliances), "TotalPlates")]
	public class GrantNecessaryAppliances_Patch
	{
		static void Postfix(ref int __result)
		{
			if (!GrantNecessarySystem.GameRequiresItem(ItemReferences.Plate))
				__result = 99;
		}
	}
}

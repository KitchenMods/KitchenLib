using HarmonyLib;
using KitchenData;
using KitchenLib.Materials;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData")]
	public class BuildGameDataPatch
	{
		/*
		 * This runs before BuildGameData is called
		 */
		static void Prefix()
		{
			MaterialManager.CollectVanillaMaterials(); // Create a Material Index, allowing for existing materials to be used where needed.
		}
	}
}
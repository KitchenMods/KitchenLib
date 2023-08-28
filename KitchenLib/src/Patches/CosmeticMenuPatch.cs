using HarmonyLib;
using Kitchen.Modules;
using KitchenData;
using KitchenLib.UI.PlateUp.Grids;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.src.Patches
{
	[HarmonyPatch(typeof(GridMenuNavigationConfig), "Instantiate")]
	public class CosmeticMenuPatch
	{
		public static List<PlayerCosmetic> Outfits = new List<PlayerCosmetic>();
		public static List<PlayerCosmetic> Hats = new List<PlayerCosmetic>();
		private static List<GridMenuConfig> registeredConfigs = new List<GridMenuConfig>
		{
			new KLGridMenuHatConfig
			{
				Cosmetics = Hats,
				Icon = Main.bundle.LoadAsset<Texture2D>("hats")
			},
			new KLGridMenuCosmeticConfig
			{
				Cosmetics = Outfits,
				Icon = Main.bundle.LoadAsset<Texture2D>("vest")
			}
		};
		static void Prefix(GridMenuNavigationConfig __instance)
		{
			foreach (GridMenuConfig custom in registeredConfigs)
			{
				if (!__instance.Links.Contains(custom))
				{
					__instance.Links.Add(custom);
				}
			}
		}
	}
}

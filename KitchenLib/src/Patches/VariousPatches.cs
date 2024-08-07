using System;
using HarmonyLib;
using Kitchen;

namespace KitchenLib.Patches
{
	#region Main menu data collection indicator
	[HarmonyPatch(typeof(DisplayVersion), "Awake")]
	internal class DisplayVersion_Patch
	{
		public static void Postfix(DisplayVersion __instance)
		{
			if (!String.IsNullOrEmpty(Main.MOD_BETA_VERSION ))
			{
				__instance.Text.text = __instance.Text.text + "?";
			}
			else
			{
				__instance.Text.text = __instance.Text.text + "!";
			}

		}
	}
	#endregion
}
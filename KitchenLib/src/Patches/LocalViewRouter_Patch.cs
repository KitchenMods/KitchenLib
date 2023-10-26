using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using KitchenLib.Views;
using UnityEngine;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch
	{
		static GameObject SyncModsPrefab;
		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (view_type != (ViewType)VariousUtils.GetID("KitchenLib.Views.SyncMods"))
				return true;

			if (SyncModsPrefab == null)
			{
				SyncModsPrefab = new GameObject("KitchenLib.Views.SyncMods");
				SyncModsPrefab.AddComponent<SyncMods>();
			}
			__result = SyncModsPrefab;
			return false;
		}
	}
}
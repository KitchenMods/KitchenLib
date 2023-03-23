using HarmonyLib;
using Kitchen;
using UnityEngine;

namespace KitchenLib.Views
{
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	class CustomViewTypePrefabPatch
	{
		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (CustomViewType.Prefabs.TryGetValue(view_type, out var value))
			{
				__result = value;
				return false;
			}

			return true;
		}
	}
}

using HarmonyLib;
using Kitchen;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Views
{
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	internal class CustomViewTypePrefabPatch
	{
		internal static Dictionary<ViewType, GameObject> _cache = new();

		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (CustomViewType.Prefabs.TryGetValue(view_type, out var value))
			{
				if (!_cache.ContainsKey(view_type))
				{
					_cache.Add(view_type, value.Invoke());
				}

				__result = _cache[view_type];

				return false;
			}

			return true;
		}
	}
}

using System.Collections.Generic;
using HarmonyLib;
using Kitchen;
using KitchenLib.Systems;
using UnityEngine;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch
	{
		public static readonly Dictionary<ViewType, GameObject> RegisteredViews = new Dictionary<ViewType, GameObject>();
		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (!ViewCreator.RegisteredViews.ContainsKey(view_type))
				return true;
			if (RegisteredViews.ContainsKey(view_type))
			{
				if (RegisteredViews[view_type] == null)
				{
					RegisteredViews.Remove(view_type);
				}
				else
				{
					__result = RegisteredViews[view_type];
					return false;
				}
			}

			GameObject prefab = new GameObject(ViewCreator.RegisteredViews[view_type].Item2.FullName);
			prefab.AddComponent(ViewCreator.RegisteredViews[view_type].Item2);
			RegisteredViews.Add(view_type, prefab);
			__result = prefab;
			return false;
		}
	}
}
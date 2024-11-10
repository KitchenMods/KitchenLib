using System.Collections.Generic;
using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Systems;
using KitchenLib.Utils;
using KitchenLib.Views;
using TMPro;
using UnityEngine;
using Font = KitchenData.Font;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch
	{
		/*
		 * This patch is ONLY for persistent views which are always active in the game.
		 */
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
	
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch_ModAchievementDistributionView
	{
		private static GameObject prefab;
		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (view_type != (ViewType)VariousUtils.GetID("KitchenLib.Views.ModAchievementDistributionView")) return true;
			
			if (prefab == null)
			{
				prefab = new GameObject();
				prefab.AddComponent<ModAchievementDistributionView>();
			}
			
			__result = prefab;
			return false;
		}
	}
	
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch_AchievementTicketView
	{
		private static GameObject prefab;
		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (view_type != (ViewType)VariousUtils.GetID("KitchenLib.Views.AchievementNotification.Ticket")) return true;
			
			if (prefab == null)
			{
				prefab = Main.bundle.LoadAsset<GameObject>("Ticket").AssignMaterialsByNames();
				AchievementNotification view = prefab.AddComponent<AchievementNotification>();
				view.Animator = prefab.GetComponent<Animator>();
				view.Title = prefab.GetChild("Ticket/Title").GetComponent<TextMeshPro>();
				view.Description = prefab.GetChild("Ticket/Description").GetComponent<TextMeshPro>();

				view.Title.fontStyle = FontStyles.Normal;
				view.Description.fontStyle = FontStyles.Normal;
				view.Title.font = GameData.Main.GlobalLocalisation.Fonts[Font.Default];
				view.Description.font = GameData.Main.GlobalLocalisation.Fonts[Font.Default];
			}
			
			__result = prefab;
			return false;
		}
	}
	
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch_AchievementSteamCloneView
	{
		private static GameObject prefab;
		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (view_type != (ViewType)VariousUtils.GetID("KitchenLib.Views.AchievementNotification.SteamClone")) return true;
			
			if (prefab == null)
			{
				prefab = Main.bundle.LoadAsset<GameObject>("Steam Clone");
				prefab = prefab.AssignMaterialsByNames();
				AchievementNotification view = prefab.AddComponent<AchievementNotification>();
				view.Animator = prefab.GetComponent<Animator>();
				view.Title = prefab.GetChild("Steam Clone/Title").GetComponent<TextMeshPro>();
				view.Description = prefab.GetChild("Steam Clone/Description").GetComponent<TextMeshPro>();
				view.Icon = prefab.GetChild("Steam Clone/Icon").GetComponent<Renderer>();
			}
			
			__result = prefab;
			return false;
		}
	}
	
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch_AchievementNoneView
	{
		private static GameObject prefab;
		static bool Prefix(ViewType view_type, ref GameObject __result)
		{
			if (view_type != (ViewType)VariousUtils.GetID("KitchenLib.Views.AchievementNotification.None")) return true;
			
			if (prefab == null)
			{
				prefab = new GameObject();
				AchievementNotification view = prefab.AddComponent<AchievementNotification>();
			}
			
			__result = prefab;
			return false;
		}
	}
}
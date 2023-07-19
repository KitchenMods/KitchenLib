using HarmonyLib;
using KitchenData;
using KitchenLib.JSON.Enums;
using KitchenLib.JSON.Models.Jsons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static KitchenLib.JSON.JSONPackUtils;

namespace KitchenLib.JSON.Patches
{
	[HarmonyPatch]
	internal class JsonDishPatches
	{
		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.ExtraOrderUnlocks), MethodType.Getter)]
		public static void Postfix_get_ExtraOrderUnlocks(JsonDish __instance, ref HashSet<Dish.IngredientUnlock> __result)
		{
			__result = __instance.TempExtraOrderUnlocks
				.Select(_ => _.Convert())
				.ToHashSet();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.MinimumIngredients), MethodType.Getter)]
		public static void Postfix_get_MinimumIngredients(JsonDish __instance, ref HashSet<Item> __result)
		{
			__result = __instance.TempRequiredProcesses
				.Select(_ => GDOConverter<Item>(_))
				.ToHashSet();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.RequiredProcesses), MethodType.Getter)]
		public static void Postfix_get_RequiredProcesses(JsonDish __instance, ref HashSet<Process> __result)
		{
			__result = __instance.TempRequiredProcesses
				.Select(_ => GDOConverter<Process>(_))
				.ToHashSet();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.BlockProviders), MethodType.Getter)]
		public static void Postfix_get_BlockProviders(JsonDish __instance, ref HashSet<Item> __result)
		{
			__result = __instance.TempBlockProviders
				.Select(_ => GDOConverter<Item>(_))
				.ToHashSet();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.IconPrefab), MethodType.Getter)]
		public static void Postfix_get_IconPrefab(JsonDish __instance, ref GameObject __result)
		{
			__result = PrefabConverter<Dish>(__instance.ModName, $"{__instance.ModID}:{__instance.UniqueNameID}", PrefabContext.IconPrefab, __instance.TempIconPrefab);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.DisplayPrefab), MethodType.Getter)]
		public static void Postfix_get_DisplayPrefab(JsonDish __instance, ref GameObject __result)
		{
			__result = PrefabConverter<Dish>(__instance.ModName, $"{__instance.ModID}:{__instance.UniqueNameID}", PrefabContext.DisplayPrefab, __instance.TempDIsplayPrefab);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.ResultingMenuItems), MethodType.Getter)]
		public static void Postfix_get_ResultingMenuItems(JsonDish __instance, ref List<Dish.MenuItem> __result)
		{
			__result = __instance.TempResultingMenuItems
				.Select(_ => _.Convert())
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.IngredientsUnlocks), MethodType.Getter)]
		public static void Postfix_get_IngredientsUnlocks(JsonDish __instance, ref HashSet<Dish.IngredientUnlock> __result)
		{
			__result = __instance.TempIngredientsUnlocks
				.Select(_ => _.Convert())
				.ToHashSet();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.RequiredDishItem), MethodType.Getter)]
		public static void Postfix_get_RequiredDishItem(JsonDish __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempRequiredDishItem);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.AllowedFoods), MethodType.Getter)]
		public static void Postfix_get_AllowedFoods(JsonDish __instance, ref List<Unlock> __result)
		{
			__result = __instance.TempAllowedFoods
				.Select(_ => GDOConverter<Unlock>(_))
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonDish), nameof(JsonDish.ForceFranchiseSetting), MethodType.Getter)]
		public static void Postfix_get_ForceFranchiseSetting(JsonDish __instance, ref RestaurantSetting __result)
		{
			__result = GDOConverter<RestaurantSetting>(__instance.TempForceFranchiseSetting);
		}
	}
}
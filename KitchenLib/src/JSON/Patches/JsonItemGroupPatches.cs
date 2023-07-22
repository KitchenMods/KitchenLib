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
	internal class JsonItemGroupPatches
	{
		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.Prefab), MethodType.Getter)]
		public static void Postfix_get_Prefab(JsonItemGroup __instance, ref GameObject __result)
		{
			__result = PrefabConverter<Item>(__instance.ModName, $"{__instance.ModID}:{__instance.UniqueNameID}", PrefabContext.Prefab, __instance.TempPrefab);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.SidePrefab), MethodType.Getter)]
		public static void Postfix_get_SidePrefab(JsonItemGroup __instance, ref GameObject __result)
		{
			__result = PrefabConverter<Item>(__instance.ModName, $"{__instance.ModID}:{__instance.UniqueNameID}", PrefabContext.SidePrefab, __instance.TempSidePrefab);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.Processes), MethodType.Getter)]
		public static void Postfix_get_Processes(JsonItemGroup __instance, ref List<Item.ItemProcess> __result)
		{
			__result = __instance.TempProcesses
				.Select(_ => _.Convert())
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.AutomaticItemProcess), MethodType.Getter)]
		public static void Postfix_get_AutomaticItemProcess(JsonItemGroup __instance, ref Item.ItemProcess __result)
		{
			__result = __instance.TempAutomaticItemProcess.Convert();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.DirtiesTo), MethodType.Getter)]
		public static void Postfix_get_DirtiesTo(JsonItemGroup __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempDirtiesTo);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.MayRequestExtraItems), MethodType.Getter)]
		public static void Postfix_get_MayRequestExtraItems(JsonItemGroup __instance, ref List<Item> __result)
		{
			__result = __instance.TempMayRequestExtraItems
				.Select(_ => GDOConverter<Item>(_))
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.SplitSubItem), MethodType.Getter)]
		public static void Postfix_get_SplitSubItem(JsonItemGroup __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempSplitSubItem);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.SplitDepletedItems), MethodType.Getter)]
		public static void Postfix_get_SplitDepletedItems(JsonItemGroup __instance, ref List<Item> __result)
		{
			__result = __instance.TempSplitDepletedItems
				.Select(_ => GDOConverter<Item>(_))
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.RefuseSplitWith), MethodType.Getter)]
		public static void Postfix_get_RefuseSplitWith(JsonItemGroup __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempRefuseSplitWith);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.DisposesTo), MethodType.Getter)]
		public static void Postfix_get_DisposesTo(JsonItemGroup __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempDisposesTo);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.DedicatedProvider), MethodType.Getter)]
		public static void Postfix_get_DedicatedProvider(JsonItemGroup __instance, ref Appliance __result)
		{
			__result = GDOConverter<Appliance>(__instance.TempDedicatedProvider);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.ExtendedDirtItem), MethodType.Getter)]
		public static void Postfix_get_ExtendedDirtItem(JsonItemGroup __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempExtendedDirtItem);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItemGroup), nameof(JsonItemGroup.Sets), MethodType.Getter)]
		public static void Postfix_get_Sets(JsonItemGroup __instance, ref List<ItemGroup.ItemSet> __result)
		{
			__result = __instance.TempSets
				.Select(_ => _.Convert())
				.ToList();
		}
	}
}

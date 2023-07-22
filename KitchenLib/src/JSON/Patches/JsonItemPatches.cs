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
	internal class JsonItemPatches
	{
		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.Prefab), MethodType.Getter)]
		public static void Postfix_get_Prefab(JsonItem __instance, ref GameObject __result)
		{
			__result = PrefabConverter<Item>(__instance.ModName, $"{__instance.ModID}:{__instance.UniqueNameID}", PrefabContext.Prefab, __instance.TempPrefab);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.SidePrefab), MethodType.Getter)]
		public static void Postfix_get_SidePrefab(JsonItem __instance, ref GameObject __result)
		{
			__result = PrefabConverter<Item>(__instance.ModName, $"{__instance.ModID}:{__instance.UniqueNameID}", PrefabContext.SidePrefab, __instance.TempSidePrefab);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.Processes), MethodType.Getter)]
		public static void Postfix_get_Processes(JsonItem __instance, ref List<Item.ItemProcess> __result)
		{
			__result = __instance.TempProcesses
				.Select(_ => _.Convert())
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.AutomaticItemProcess), MethodType.Getter)]
		public static void Postfix_get_AutomaticItemProcess(JsonItem __instance, ref Item.ItemProcess __result)
		{
			__result = __instance.TempAutomaticItemProcess.Convert();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.DirtiesTo), MethodType.Getter)]
		public static void Postfix_get_DirtiesTo(JsonItem __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempDirtiesTo);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.MayRequestExtraItems), MethodType.Getter)]
		public static void Postfix_get_MayRequestExtraItems(JsonItem __instance, ref List<Item> __result)
		{
			__result = __instance.TempMayRequestExtraItems
				.Select(_ => GDOConverter<Item>(_))
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.SplitSubItem), MethodType.Getter)]
		public static void Postfix_get_SplitSubItem(JsonItem __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempSplitSubItem);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.SplitDepletedItems), MethodType.Getter)]
		public static void Postfix_get_SplitDepletedItems(JsonItem __instance, ref List<Item> __result)
		{
			__result = __instance.TempSplitDepletedItems
				.Select(_ => GDOConverter<Item>(_))
				.ToList();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.RefuseSplitWith), MethodType.Getter)]
		public static void Postfix_get_RefuseSplitWith(JsonItem __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempRefuseSplitWith);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.DisposesTo), MethodType.Getter)]
		public static void Postfix_get_DisposesTo(JsonItem __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempDisposesTo);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.DedicatedProvider), MethodType.Getter)]
		public static void Postfix_get_DedicatedProvider(JsonItem __instance, ref Appliance __result)
		{
			__result = GDOConverter<Appliance>(__instance.TempDedicatedProvider);
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(JsonItem), nameof(JsonItem.ExtendedDirtItem), MethodType.Getter)]
		public static void Postfix_get_ExtendedDirtItem(JsonItem __instance, ref Item __result)
		{
			__result = GDOConverter<Item>(__instance.TempExtendedDirtItem);
		}
	}
}

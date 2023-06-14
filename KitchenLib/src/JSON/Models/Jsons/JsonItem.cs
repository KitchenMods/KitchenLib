using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KitchenLib.JSON.Models.Containers;
using UnityEngine;
using KitchenLib.JSON.Interfaces;
using System;

namespace KitchenLib.JSON.Models.Jsons
{
    public class JsonItem : CustomItem, IHasSidePrefab
    {
		[field:JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }

		[JsonProperty("GDOName")]
        public string GDOName { get; set; }
        
        [JsonProperty("Properties")]
        public List<ItemPropertyContainer> ItemPropertyContainers { get; set; } = new List<ItemPropertyContainer>();
        [JsonProperty("Materials")]
        public List<MaterialsContainer> MaterialsContainers { get; set; } = new List<MaterialsContainer>();

		[JsonProperty("Prefab")]
		public string TempPrefab { get; set; }
		[JsonProperty("SidePrefab")]
		public string TempSidePrefab { get; set; }
		[JsonProperty("Processes")]
		public List<ItemProcessContainer> TempProcesses { get; set; }
		[JsonProperty("DirtiesTo")]
		public string TempDirtiesTo { get; set; }
		[JsonProperty("MayRequestExtraItems")]
		public List<string> TempMayRequestExtraItems { get; set; }
		[JsonProperty("SplitSubItem")]
		public string TempSplitSubItem { get; set; }
		[JsonProperty("SplitDepletedItems")]
		public List<string> TempSplitDepletedItems { get; set; }
		[JsonProperty("SplitByComponentsHolder")]
		public string TempSplitByComponentsHolder { get; set; }
		[JsonProperty("RefuseSplitWith")]
		public string TempRefuseSplitWith { get; set; }
		[JsonProperty("DisposesTo")]
		public string TempDisposesTo { get; set; }
		[JsonProperty("DedicatedProvider")]
		public string TempDedicatedProvider { get; set; }
		[JsonProperty("ExtendedDirtItem")]
		public string TempExtendedDirtItem { get; set; }

		[JsonIgnore]
		public override List<Item.ItemProcess> Processes { get; protected set; } = new List<Item.ItemProcess>();

		[OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
			Tuple<string, string> Context = (Tuple<string, string>)context.Context;
			ModName = Context.Item2;
			ModID = $"{Context.Item1}.{Context.Item2}";
			Properties = ItemPropertyContainers.Select(p => p.Property).ToList();
        }

        public override void OnRegister(Item gameDataObject)
        {
			gameDataObject.name = GDOName;

			foreach (MaterialsContainer materialsContainer in MaterialsContainers)
			{
				Material[] Materials = ContentPackManager.ConvertMaterialContainer(materialsContainer.Materials).ToArray();
				MaterialUtils.ApplyMaterial(gameDataObject.Prefab, materialsContainer.Path, Materials);
			}
		}

		public static void get_Prefab_Postfix(JsonItem __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.PrefabConverter<Item>(__instance.ModName, __instance.TempPrefab);
			}
		}

		public static void get_SidePrefab_Postfix(JsonItem __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.SidePrefabConverter<Item>(__instance.ModName, __instance.TempSidePrefab);
			}
		}

		public static void get_Processes_Postfix(JsonItem __instance, ref List<Item.ItemProcess> __result)
		{
			Main.LogInfo($"{__instance.GetType()}: {__instance.UniqueNameID}");
			__result = ContentPackPatches.ItemProcessesConverter(__instance.TempProcesses);
		}

		public static void get_DirtiesTo_Postfix(JsonItem __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempDirtiesTo);
			}
		}

		public static void get_MayRequestExtraItems_Postfix(JsonItem __instance, ref List<Item> __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOsConverter<Item>(__instance.TempMayRequestExtraItems);
			}
		}

		public static void get_SplitSubItem_Postfix(JsonItem __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempSplitSubItem);
			}
		}

		public static void get_SplitDepletedItems_Postfix(JsonItem __instance, ref List<Item> __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOsConverter<Item>(__instance.TempSplitDepletedItems);
			}
		}

		public static void get_SplitByComponentsHolder_Postfix(JsonItem __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempSplitByComponentsHolder);
			}
		}

		public static void get_RefuseSplitWith_Postfix(JsonItem __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempRefuseSplitWith);
			}
		}

		public static void get_DisposesTo_Postfix(JsonItem __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempDisposesTo);
			}
		}

		public static void get_DedicatedProvider_Postfix(JsonItem __instance, ref Appliance __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOConverter<Appliance>(__instance.TempDedicatedProvider);
			}
		}

		public static void get_ExtendedDirtItem_Postfix(JsonItem __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonItem))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempExtendedDirtItem);
			}
		}
	}
}

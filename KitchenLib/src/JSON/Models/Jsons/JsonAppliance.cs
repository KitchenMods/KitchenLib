using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KitchenLib.JSON.Models.Containers;
using UnityEngine;
using System;

namespace KitchenLib.JSON.Models.Jsons
{
	public class JsonAppliance : CustomAppliance
	{
		[field: JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }
		[JsonProperty("BaseGameDataObjectID")]
		public override int BaseGameDataObjectID { get; protected set; } = -1;

		[JsonProperty("GDOName")]
		public string GDOName { get; set; }

		[JsonProperty("Properties")]
		public List<AppliancePropertyContainer> AppliancePropertyContainers { get; set; }
		[JsonProperty("Materials")]
		public List<MaterialsContainer> MaterialsContainers { get; set; } = new List<MaterialsContainer>();

		[JsonProperty("Prefab")]
		public string TempPrefab { get; set; }
		[JsonProperty("HeldAppliancePrefab")]
		public string TempHeldAppliancePrefab { get; set; }
		[JsonProperty("Processes")]
		public List<ApplianceProcessesContainer> TempProcesses { get; set; }
		[JsonProperty("RequiresForShop")]
		public List<string> TempRequiresForShop { get; set; }
		[JsonProperty("RequiresProcessForShop")]
		public List<string> TempRequiresProcessForShop { get; set; }
		[JsonProperty("Upgrades")]
		public List<string> TempUpgrades { get; set; }
		[JsonProperty("CrateItem")]
		public string TempCrateItem { get; set; }

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			Tuple<string, string> Context = (Tuple<string, string>)context.Context;
			ModName = Context.Item2;
			ModID = $"{Context.Item1}.{Context.Item2}";
			Properties = AppliancePropertyContainers.Select(p => p.Property).ToList();
		}

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.name = GDOName;

			foreach (MaterialsContainer materialsContainer in MaterialsContainers)
			{
				Material[] Materials = ContentPackManager.ConvertMaterialContainer(materialsContainer.Materials).ToArray();
				MaterialUtils.ApplyMaterial(gameDataObject.Prefab, materialsContainer.Path, Materials);
			}
		}

		public static void get_Prefab_Postfix(JsonAppliance __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance) && __instance.BaseGameDataObjectID == -1)
			{
				Main.LogInfo($"BaseGameDataObjectID: {__instance.BaseGameDataObjectID}");
				__result = PrefabConverter(__instance.ModName, __instance.TempPrefab);
			}
		}
		public static void get_HeldAppliancePrefab_Postfix(JsonAppliance __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance) && __instance.BaseGameDataObjectID == -1 && __instance.TempHeldAppliancePrefab != null)
			{
				__result = HeldAppliancePrefabConverter(__instance.ModName, __instance.TempHeldAppliancePrefab);
			}
		}
		public static void get_Processes_Postfix(JsonAppliance __instance, ref List<Appliance.ApplianceProcesses> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance) && __instance.BaseGameDataObjectID == -1)
			{
				__result = ContentPackPatches.ApplianceProcessesConverter(__instance.TempProcesses);
			}
		}
		public static void get_RequiresForShop_Postfix(JsonAppliance __instance, ref List<Appliance> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance) && __instance.BaseGameDataObjectID == -1)
			{
				__result = ContentPackPatches.GDOsConverter<Appliance>(__instance.TempRequiresForShop);
			}
		}
		public static void get_RequiresProcessForShop_Postfix(JsonAppliance __instance, ref List<Process> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance) && __instance.BaseGameDataObjectID == -1)
			{
				__result = ContentPackPatches.GDOsConverter<Process>(__instance.TempRequiresProcessForShop);
			}
		}
		public static void get_Upgrades_Postfix(JsonAppliance __instance, ref List<Appliance> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance) && __instance.BaseGameDataObjectID == -1)
			{
				__result = ContentPackPatches.GDOsConverter<Appliance>(__instance.TempUpgrades);
			}
		}
		public static void get_CrateItem_Postfix(JsonAppliance __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance) && __instance.BaseGameDataObjectID == -1)
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempCrateItem);
			}
		}

		public static GameObject PrefabConverter(string key, string str)
		{
			Main.LogInfo("PrefabConverterAppliance");
			if (int.TryParse(str, out int id))
				return ((Appliance)GDOUtils.GetExistingGDO(id) ?? (Appliance)GDOUtils.GetCustomGameDataObject(id)?.GameDataObject).Prefab;
			else
				return ContentPackManager.AssetBundleTable[key].FirstOrDefault(x => x.LoadAsset<GameObject>(str) != null)?.LoadAsset<GameObject>(str);
		}
		public static GameObject HeldAppliancePrefabConverter(string key, string str)
		{
			if (int.TryParse(str, out int id))
				return ((Appliance)GDOUtils.GetExistingGDO(id) ?? (Appliance)GDOUtils.GetCustomGameDataObject(id)?.GameDataObject).HeldAppliancePrefab;
			else
				return ContentPackManager.AssetBundleTable[key].FirstOrDefault(x => x.LoadAsset<GameObject>(str) != null)?.LoadAsset<GameObject>(str);
		}
	}
}

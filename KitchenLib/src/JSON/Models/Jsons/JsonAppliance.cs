using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KitchenLib.JSON.Models.Containers;
using UnityEngine;

namespace KitchenLib.JSON.Models.Jsons
{
	public class JsonAppliance : CustomAppliance
	{
		[field: JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }
		[JsonProperty("Author", Required = Required.Always)]
		public string Author { get; set; }
		[JsonProperty("GDOName")]
		public string GDOName { get; set; }

		[JsonProperty("Properties")]
		public List<AppliancePropertyContainer> AppliancePropertyContainers { get; set; } = new List<AppliancePropertyContainer>();
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
			ModName = context.Context.ToString();
			ModID = $"{Author}.{ModName}";
			Properties = AppliancePropertyContainers.Select(p => p.Property).ToList();
		}

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.name = GDOName;

			foreach (MaterialsContainer materialsContainer in MaterialsContainers)
				MaterialUtils.ApplyMaterial(gameDataObject.Prefab, materialsContainer.Path, materialsContainer.Materials);
		}

		public static void get_Prefab_Postfix(JsonAppliance __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance))
			{
				__result = ContentPackPatches.PrefabConverter(__instance.ModName, __instance.TempPrefab);
			}
		}
		public static void get_HeldAppliancePrefab(JsonAppliance __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance))
			{
				__result = ContentPackPatches.PrefabConverter(__instance.ModName, __instance.TempHeldAppliancePrefab);
			}
		}
		public static void get_Processes(JsonAppliance __instance, ref List<Appliance.ApplianceProcesses> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance))
			{
				__result = ContentPackPatches.ApplianceProcessesConverter(__instance.TempProcesses);
			}
		}
		public static void get_RequiresForShop(JsonAppliance __instance, ref List<Appliance> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance))
			{
				__result = ContentPackPatches.GDOsConverter<Appliance>(__instance.TempRequiresForShop);
			}
		}
		public static void get_RequiresProcessForShop(JsonAppliance __instance, ref List<Process> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance))
			{
				__result = ContentPackPatches.GDOsConverter<Process>(__instance.TempRequiresProcessForShop);
			}
		}
		public static void get_Upgrades(JsonAppliance __instance, ref List<Appliance> __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance))
			{
				__result = ContentPackPatches.GDOsConverter<Appliance>(__instance.TempUpgrades);
			}
		}
		public static void get_CrateItem(JsonAppliance __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonAppliance))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempCrateItem);
			}
		}

	}
}

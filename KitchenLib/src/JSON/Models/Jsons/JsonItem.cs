using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KitchenLib.src.JSON.Models.Containers;
using UnityEngine;

namespace KitchenLib.JSON.Models.Jsons
{
    public class JsonItem : CustomItem
    {
		[field:JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }
		[JsonProperty("Author", Required = Required.Always)]
		public string Author { get; set; }
		[JsonProperty("GDOName")]
        public string GDOName { get; set; }
        [JsonProperty("Prefab")]
        public string PrefabStr { get; set; }
		[JsonProperty("SidePrefab")]
		public string SidePrefabStr { get; set; }
        [JsonProperty("Properties")]
        public List<ItemPropertyContainer> ItemPropertyContainers { get; set; } = new List<ItemPropertyContainer>();
        [JsonProperty("Materials")]
        public List<MaterialsContainer> MaterialsContainers { get; set; } = new List<MaterialsContainer>();

		[JsonIgnore]
		public override GameObject Prefab { get; protected set; }
		[JsonIgnore]
		public override GameObject SidePrefab { get; protected set; }
		[JsonProperty("Processes")]
		public List<ItemProcessContainer> TempProcesses { get; set; }
		[JsonIgnore]
		public override List<Item.ItemProcess> Processes { get; protected set; }
		[JsonProperty("DirtiesTo")]
		public int TempDirtiesTo { get; set; }
		public int TempSplitSubItem { get; set; }
		public List<int> TempSplitDepletedItems { get; set; }
		public int TempSplitByComponentsHolder { get; set; }
		public int TempRefuseSplitWith { get; set; }
		public int TempDisposesTo { get; set; }
		public int TempDedicatedProvider { get; set; }
		public int TempExtendedDirtItem { get; set; }

		[OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
			ModName = context.Context.ToString();
			ModID = $"{Author}.{ModName}";
            Prefab = PrefabConverter(ModName, PrefabStr);
            Properties = ItemPropertyContainers.Select(p => p.Property).ToList();
        }

        public override void OnRegister(Item gameDataObject)
        {
            gameDataObject.name = GDOName;

            foreach (MaterialsContainer materialsContainer in MaterialsContainers)
                MaterialUtils.ApplyMaterial(gameDataObject.Prefab, materialsContainer.Path, materialsContainer.Materials);
		}

		public static GameObject PrefabConverter(string key, string str)
		{
			if (int.TryParse(str, out int id))
				return ((Item)GDOUtils.GetExistingGDO(id) ?? (Item)GDOUtils.GetCustomGameDataObject(id)?.GameDataObject).Prefab;
			else
				return ContentPackManager.AssetBundleTable[key].FirstOrDefault(x => x.LoadAsset<GameObject>(str) != null)?.LoadAsset<GameObject>(str);
		}

		public static void get_Prefab_Postfix(JsonItem __instance, ref GameObject __result)
		{
			__result = PrefabConverter(__instance.ModName, __instance.PrefabStr);
		}

		public static void get_SidePrefab_Postfix(JsonItem __instance, ref GameObject __result)
		{
			__result = PrefabConverter(__instance.ModName, __instance.SidePrefabStr);
		}

		public static void get_Processes_Postfix(JsonItem __instance, ref List<Item.ItemProcess> __result)
		{
			List<Item.ItemProcess> Processes = new List<Item.ItemProcess>();
			List<ItemProcessContainer> container = __instance.TempProcesses;

			for(int i=0; i<container.Count; i++)
			{
				Item.ItemProcess process = container[i].Convert();
				Processes.Add(process);
			}

			__result = Processes;
		}
	}
}

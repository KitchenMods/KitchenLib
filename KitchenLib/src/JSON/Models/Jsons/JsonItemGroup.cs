using KitchenData;
using KitchenLib.Customs;
using KitchenLib.JSON.Models.Containers;
using KitchenLib.src.JSON.Models.Containers;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.JSON.Models.Jsons
{
	public class JsonItemGroup : CustomItemGroup
	{
		[field: JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }
		[JsonProperty("Author", Required = Required.Always)]
		public string Author { get; set; }
		[JsonProperty("GDOName")]
		string GDOName { get; set; }
		[JsonProperty("Prefab")]
		string PrefabStr { get; set; }
		[JsonProperty("Properties")]
		List<ItemPropertyContainer> ItemPropertyContainers { get; set; } = new List<ItemPropertyContainer>();
		[JsonProperty("Materials")]
		List<MaterialsContainer> MaterialsContainers { get; set; } = new List<MaterialsContainer>();

		[JsonProperty("Sets")]
		public List<ItemSetContainer> TempSets { get; set; }
		[JsonIgnore]
		public override List<ItemGroup.ItemSet> Sets { get; protected set; }

		[JsonIgnore]
		public override GameObject Prefab { get; protected set; }

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			ModName = context.Context.ToString();
			ModID = $"{Author}.{ModName}";
			Properties = ItemPropertyContainers.Select(p => p.Property).ToList();
		}

		public override void OnRegister(ItemGroup gameDataObject)
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

		public static void get_Prefab_Postfix(JsonItemGroup __instance, ref GameObject __result)
		{
			__result = PrefabConverter(__instance.ModName, __instance.PrefabStr);
		}

		public static void get_Sets_Postfix(JsonItemGroup __instance, ref List<ItemGroup.ItemSet> __result)
		{
			List<ItemGroup.ItemSet> Sets = new List<ItemGroup.ItemSet>();
			List<ItemSetContainer> container = __instance.TempSets;

			for(int i=0; i< container.Count; i++)
			{
				ItemGroup.ItemSet Set = container[i].Convert();
				Sets.Add(Set);
			}

			__result = Sets;
		}
	}
}

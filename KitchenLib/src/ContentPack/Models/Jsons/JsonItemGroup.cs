using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using static KitchenLib.src.ContentPack.ContentPackUtils;

namespace KitchenLib.src.ContentPack.Models.Jsons
{
    public class JsonItemGroup : CustomItemGroup
    {
        [JsonProperty("Prefab")]
        string PrefabStr { get; set; }
        [JsonProperty("Properties")]
        List<ItemPropertyContainer> ItemPropertyContainers { get; set; }
        [JsonProperty("ComponentGroups")]
        List<ItemGroupView.ComponentGroup> ComponentGroups { get; set; }
        [JsonProperty("Materials")]
        List<MaterialsContainer> MaterialsContainers { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Prefab = PrefabConverter(PrefabStr);
            if (ItemPropertyContainers is not null)
                Properties = ItemPropertyContainers.Select(p => p.Property).ToList();
        }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            ItemGroup item = (ItemGroup)gameDataObject;
            ItemGroupView view = item.Prefab.GetComponent<ItemGroupView>();
        }
    }

    public class MaterialsContainer
    {
        public string Path { get; set; }
        public Material[] Materials { get; set; }
    }
}

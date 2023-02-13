using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.ContentPack.Models.Containers;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static KitchenLib.ContentPack.ContentPackUtils;

namespace KitchenLib.ContentPack.Models.Jsons
{
    public class JsonItemGroup : CustomItemGroup
    {
        [JsonProperty("GDOName")]
        string GDOName { get; set; } = "";
        [JsonProperty("Prefab")]
        string PrefabStr { get; set; } = "";
        [JsonProperty("Properties")]
        List<ItemPropertyContainer> ItemPropertyContainers { get; set; } = new List<ItemPropertyContainer>();
        [JsonProperty("ComponentGroups")]
        List<ItemGroupView.ComponentGroup> ComponentGroups { get; set; } = new List<ItemGroupView.ComponentGroup>();
        [JsonProperty("Materials")] 
        List<MaterialsContainer> MaterialsContainers { get; set; } = new List<MaterialsContainer>();

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Prefab = PrefabConverter(PrefabStr);
            Properties = ItemPropertyContainers.Select(p => p.Property).ToList();
        }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            gameDataObject.name = GDOName;

            ItemGroup item = (ItemGroup)gameDataObject;
            ItemGroupView view = item.Prefab.GetComponent<ItemGroupView>();
            view.ComponentGroups = ComponentGroups;

            foreach (MaterialsContainer materialsContainer in MaterialsContainers)
                MaterialUtils.ApplyMaterial(item.Prefab, materialsContainer.Path, materialsContainer.Materials);
        }
    }
}

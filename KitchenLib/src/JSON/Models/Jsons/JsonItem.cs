using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static KitchenLib.src.JSON.ContentPackUtils;
using KitchenLib.src.JSON.Models.Containers;

namespace KitchenLib.src.JSON.Models.Jsons
{
    public class JsonItem : CustomItem
    {
        [JsonProperty("GDOName")]
        string GDOName { get; set; } = "";
        [JsonProperty("Prefab")]
        string PrefabStr { get; set; } = "";
        [JsonProperty("Properties")]
        List<ItemPropertyContainer> ItemPropertyContainers { get; set; } = new List<ItemPropertyContainer>();
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

            Item item = gameDataObject as Item;

            foreach (MaterialsContainer materialsContainer in MaterialsContainers)
                MaterialUtils.ApplyMaterial(item.Prefab, materialsContainer.Path, materialsContainer.Materials);
        }
    }
}

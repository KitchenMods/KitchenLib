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
    public class JsonAppliance : CustomAppliance
    {
        [JsonProperty("GDOName")]
        string GDOName { get; set; } = "";
        [JsonProperty("Prefab")]
        string PrefabStr { get; set; } = "";
        [JsonProperty("Properties")]
        List<AppliancePropertyContainer> AppliancePropertyContainers { get; set; } = new List<AppliancePropertyContainer>();
        [JsonProperty("Materials")]
        List<MaterialsContainer> MaterialsContainers { get; set; } = new List<MaterialsContainer>();

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Prefab = PrefabConverter(PrefabStr);
            Properties = AppliancePropertyContainers.Select(p => p.Property).ToList();
        }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            gameDataObject.name = GDOName;

            Appliance appliance = gameDataObject as Appliance;

            foreach (MaterialsContainer materialsContainer in MaterialsContainers)
                MaterialUtils.ApplyMaterial(appliance.Prefab, materialsContainer.Path, materialsContainer.Materials);
        }
    }
}

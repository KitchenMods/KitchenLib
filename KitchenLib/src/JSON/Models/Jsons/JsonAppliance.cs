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
    public class JsonAppliance : CustomAppliance
    {
		[JsonProperty("UniqueNameID")]
		public override string UniqueNameID { get; internal set; } = "";
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

        public override void OnRegister(Appliance gameDataObject)
        {
            gameDataObject.name = GDOName;

            foreach (MaterialsContainer materialsContainer in MaterialsContainers)
                MaterialUtils.ApplyMaterial(gameDataObject.Prefab, materialsContainer.Path, materialsContainer.Materials);
        }
    }
}

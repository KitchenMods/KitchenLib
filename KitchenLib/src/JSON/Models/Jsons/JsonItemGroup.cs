using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static KitchenLib.src.JSON.ContentPackUtils;
using KitchenLib.src.JSON.Models.Containers;
using System.Reflection;

namespace KitchenLib.src.JSON.Models.Jsons
{
    public class JsonItemGroup : CustomItemGroup
    {
		[JsonProperty("UniqueNameID")]
		public override string UniqueNameID { get; internal set; } = "";
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

        public override void OnRegister(ItemGroup gameDataObject)
        {
            gameDataObject.name = GDOName;

            ItemGroupView view = gameDataObject.Prefab.GetComponent<ItemGroupView>();

            FieldInfo ComponentGroupsField = ReflectionUtils.GetField<ItemGroupView>("ComponentGroups", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            ComponentGroupsField.SetValue(view, ComponentGroups);

            foreach (MaterialsContainer materialsContainer in MaterialsContainers)
                MaterialUtils.ApplyMaterial(gameDataObject.Prefab, materialsContainer.Path, materialsContainer.Materials);
        }
    }
}

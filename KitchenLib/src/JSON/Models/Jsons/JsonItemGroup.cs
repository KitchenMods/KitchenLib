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
using System;
using System.Collections;

namespace KitchenLib.src.JSON.Models.Jsons
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
        List<ComponentGroupContainer> ComponentGroups { get; set; } = new List<ComponentGroupContainer>();
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

            FieldInfo ComponentGroupsField = ReflectionUtils.GetField<ItemGroupView>("ComponentGroups", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assembly ItemGroupViewAssembly = typeof(ItemGroupView).Assembly;
            Type ComponentGroupType = typeof(ItemGroupView).GetNestedType("ComponentGroup", BindingFlags.Instance | BindingFlags.NonPublic);
            var ComponentGroupListType = typeof(List<>).MakeGenericType(ComponentGroupType);
            var ComponentGroupList = (IList)Activator.CreateInstance(ComponentGroupListType);
            foreach(ComponentGroupContainer componentGroupContainer in ComponentGroups)
            {
                var ComponentGroup = Activator.CreateInstance(ComponentGroupType);
                FieldInfo ItemField = ReflectionUtils.GetField("Item", ComponentGroupType);
                ItemField.SetValue(ComponentGroup, componentGroupContainer.Item);
                FieldInfo GameObject = ReflectionUtils.GetField("GameObject", ComponentGroupType);
                GameObject.SetValue(ComponentGroup, componentGroupContainer.GameObject);
                FieldInfo Objects = ReflectionUtils.GetField("Objects", ComponentGroupType);
                Objects.SetValue(ComponentGroup, componentGroupContainer.Objects);
                FieldInfo DrawAll = ReflectionUtils.GetField("DrawAll", ComponentGroupType);
                DrawAll.SetValue(ComponentGroup, componentGroupContainer.DrawAll);
                FieldInfo IsDrawing = ReflectionUtils.GetField("IsDrawing", ComponentGroupType);
                IsDrawing.SetValue(ComponentGroup, componentGroupContainer.IsDrawing);
                FieldInfo ShouldDraw = ReflectionUtils.GetField("ShouldDraw", ComponentGroupType);
                ShouldDraw.SetValue(ComponentGroup, componentGroupContainer.ShouldDraw);
                ComponentGroupList.Add(ComponentGroup);
            }
            ComponentGroupsField.SetValue(view, ComponentGroupList);

            foreach (MaterialsContainer materialsContainer in MaterialsContainers)
                MaterialUtils.ApplyMaterial(item.Prefab, materialsContainer.Path, materialsContainer.Materials);
        }
    }
}

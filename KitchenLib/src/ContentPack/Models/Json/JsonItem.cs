using KitchenData;
using KitchenLib.Customs;
using KitchenLib.src.ContentPack.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.src.ContentPack.Models.Json
{
    public class JsonItem : CustomItem
    {
        [JsonProperty("ID")]
        public override int ID { get; internal set; }
        [JsonProperty("UniqueNameID")]
        public override string UniqueNameID { get; internal set; }
        [JsonProperty("BaseGameDataObjectID")]
        [DefaultValue(-1)]
        public override int BaseGameDataObjectID { get; internal set; }
        [JsonProperty("ModName")]
        [DefaultValue("")]
        public new string ModName;

        [JsonProperty("PrefabConverter")]
        public override GameObject Prefab { get; internal set; }
        [JsonProperty("Processes")]
        public override List<Item.ItemProcess> Processes { get; internal set; }
        [JsonProperty("ExtraTimeGranted")]
        public override float ExtraTimeGranted { get; internal set; }
        [JsonProperty("ItemValue")]
        [DefaultValue(ItemValue.Small)]
        public override ItemValue ItemValue { get; internal set; }
        [JsonProperty("Reward")]
        [DefaultValue(1)]
        public override int Reward { get; internal set; }
        [JsonProperty("DirtiesTo")]
        public override Item DirtiesTo { get; internal set; }
        [JsonProperty("MayRequestExtraItems")]
        public override List<Item> MayRequestExtraItems { get; internal set; }
        [JsonProperty("MaxOrderSharers")]
        public override int MaxOrderSharers { get; internal set; }
        [JsonProperty("SplitSubItem")]
        public override Item SplitSubItem { get; internal set; }
        [JsonProperty("SplitCount")]
        [DefaultValue(0)]
        public override int SplitCount { get; internal set; }
        [JsonProperty("SplitSpeed")]
        [DefaultValue(1f)]
        public override float SplitSpeed { get; internal set; }
        [JsonProperty("SplitDepletedItems")]
        public override List<Item> SplitDepletedItems { get; internal set; }
        [JsonProperty("AllowSplitMerging")]
        public override bool AllowSplitMerging { get; internal set; }
        [JsonProperty("PreventExplicitSplit")]
        public override bool PreventExplicitSplit { get; internal set; }
        [JsonProperty("SplitByComponents")]
        public override bool SplitByComponents { get; internal set; }
        [JsonProperty("SplitByComponentsHolder")]
        public override Item SplitByComponentsHolder { get; internal set; }
        [JsonProperty("SplitByCopying")]
        public override bool SplitByCopying { get; internal set; }
        [JsonProperty("RefusesSplitWith")]
        public override Item RefuseSplitWith { get; internal set; }
        [JsonProperty("DisposesTo")]
        public override Item DisposesTo { get; internal set; }
        [JsonProperty("IsIndisposable")]
        public override bool IsIndisposable { get; internal set; }
        [JsonProperty("ItemCategory")]
        public override ItemCategory ItemCategory { get; internal set; }
        [JsonProperty("ItemStorageFlags")]
        public override ItemStorage ItemStorageFlags { get; internal set; }
        [JsonProperty("DedicatedProvider")]
        public override Appliance DedicatedProvider { get; internal set; }
        [JsonProperty("HoldPose")]
        [DefaultValue(ToolAttachPoint.Generic)]
        public override ToolAttachPoint HoldPose { get; internal set; }
        [JsonProperty("IsMergeableSide")]
        public override bool IsMergeableSide { get; internal set; }
        [JsonProperty("ExtendedDirtItem")]
        public override Item ExtendedDirtItem { get; internal set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (context.Context is string modName)
                ModName = modName;

            if (Processes == null)
                Processes = new List<Item.ItemProcess>();
            if (MayRequestExtraItems == null)
                MayRequestExtraItems = new List<Item>();
            if (SplitDepletedItems == null)
                SplitDepletedItems = new List<Item>();
        }
    }
}

using KitchenData;
using KitchenLib.Customs;
using KitchenLib.src.ContentPack.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.src.ContentPack.Models.Json
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonItem : CustomItem
    {
        [JsonProperty("ID")]
        public override int ID { get; internal set; }
        [JsonProperty("UniqueNameID")]
        public override string UniqueNameID { get; internal set; }
        [JsonProperty("BaseGameDataObjectID")]
        public override int BaseGameDataObjectID { get; }
        [JsonProperty("ModName")]
        public new string ModName;

        [JsonProperty("PrefabConverter")]
        public override GameObject Prefab { get; internal set; }
        [JsonProperty("Processes")]
        public override List<Item.ItemProcess> Processes { get; }
        [JsonProperty("ExtraTimeGranted")]
        public override float ExtraTimeGranted { get; internal set; }
        [JsonProperty("ItemValue")]
        public override ItemValue ItemValue { get; }
        [JsonProperty("Reward")]
        public override int Reward { get; }
        [JsonProperty("DirtiesTo")]
        public override Item DirtiesTo { get; internal set; }
        [JsonProperty("MayRequestExtraItems")]
        public override List<Item> MayRequestExtraItems { get; }
        [JsonProperty("MaxOrderSharers")]
        public override int MaxOrderSharers { get; internal set; }
        [JsonProperty("SplitSubItem")]
        public override Item SplitSubItem { get; internal set; }
        [JsonProperty("SplitCount")]
        public override int SplitCount { get; }
        [JsonProperty("SplitSpeed")]
        public override float SplitSpeed { get; }
        [JsonProperty("SplitDepletedItems")]
        public override List<Item> SplitDepletedItems { get; }
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
        public override ToolAttachPoint HoldPose { get; }
        [JsonProperty("IsMergeableSide")]
        public override bool IsMergeableSide { get; internal set; }
        [JsonProperty("ExtendedDirtItem")]
        public override Item ExtendedDirtItem { get; internal set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (context.Context is string modName)
                ModName = modName;
        }
    }
}

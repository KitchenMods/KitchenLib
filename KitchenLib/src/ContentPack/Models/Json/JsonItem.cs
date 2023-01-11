using KitchenData;
using KitchenLib.Customs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.src.ContentPack.Models.Json
{
    public class JsonItem : CustomItem
    {
        [JsonProperty("UniqueNameID")]
        public override string UniqueNameID { get; internal set; }
        [field:JsonProperty("BaseGameDataObjectID")]
        [field:DefaultValue(-1)]
        [JsonIgnore]
        public override int BaseGameDataObjectID { get; }

        [JsonProperty("Prefab")]
        public override GameObject Prefab { get; internal set; }
        [JsonProperty("Processes")]
        public override List<Item.ItemProcess> Processes { get { return new List<Item.ItemProcess>(); } }
        public override List<IItemProperty> Properties { get { return new List<IItemProperty>(); } }
        public override float ExtraTimeGranted { get; internal set; }
        public override ItemValue ItemValue { get { return ItemValue.Small; } }
        public override int Reward { get { return 1; } }
        public override Item DirtiesTo { get; internal set; }
        public override List<Item> MayRequestExtraItems { get { return new List<Item>(); } }
        public override int MaxOrderSharers { get; internal set; }
        public override Item SplitSubItem { get; internal set; }
        public override int SplitCount { get { return 0; } }
        public override float SplitSpeed { get { return 1f; } }
        public override List<Item> SplitDepletedItems { get { return new List<Item>(); } }
        public override bool AllowSplitMerging { get; internal set; }
        public override bool PreventExplicitSplit { get; internal set; }
        public override bool SplitByComponents { get; internal set; }
        public override Item SplitByComponentsHolder { get; internal set; }
        public override bool SplitByCopying { get; internal set; }
        public override Item RefuseSplitWith { get; internal set; }
        public override Item DisposesTo { get; internal set; }
        public override bool IsIndisposable { get; internal set; }
        public override ItemCategory ItemCategory { get; internal set; }
        public override ItemStorage ItemStorageFlags { get; internal set; }
        public override Appliance DedicatedProvider { get; internal set; }
        public override ToolAttachPoint HoldPose { get { return ToolAttachPoint.Generic; } }
        public override bool IsMergeableSide { get; internal set; }
        public override Item ExtendedDirtItem { get; internal set; }
    }
}

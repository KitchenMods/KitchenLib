using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using KitchenLib.Colorblind;

namespace KitchenLib.Customs
{
    public abstract class CustomItem : CustomGameDataObject
    {
        public virtual GameObject Prefab { get; protected set; }
        public virtual List<Item.ItemProcess> Processes { get; protected set; }=new List<Item.ItemProcess>();
        public virtual List<IItemProperty> Properties { get; protected set; } = new List<IItemProperty>();
        public virtual float ExtraTimeGranted { get; protected set; }
        public virtual ItemValue ItemValue { get; protected set; } = ItemValue.Small;
        public virtual int Reward { get { return 1; } }
        public virtual Item DirtiesTo { get; protected set; }
        public virtual List<Item> MayRequestExtraItems { get; protected set; } = new List<Item>();
        public virtual int MaxOrderSharers { get; protected set; }
        public virtual Item SplitSubItem { get; protected set; }
        public virtual int SplitCount { get; protected set; } = 0;
        public virtual float SplitSpeed { get; protected set; } = 1f;
        public virtual List<Item> SplitDepletedItems { get; protected set; } = new List<Item>();
        public virtual bool AllowSplitMerging { get; protected set; }
        public virtual bool PreventExplicitSplit { get; protected set; }
        public virtual bool SplitByComponents { get; protected set; }
        public virtual Item SplitByComponentsHolder { get; protected set; }
        public virtual bool SplitByCopying { get; protected set; }
        public virtual Item RefuseSplitWith { get; protected set; }
        public virtual Item DisposesTo { get; protected set; }
        public virtual bool IsIndisposable { get; protected set; }
        public virtual ItemCategory ItemCategory { get; protected set; }
        public virtual ItemStorage ItemStorageFlags { get; protected set; }
        public virtual Appliance DedicatedProvider { get; protected set; }
        public virtual ToolAttachPoint HoldPose { get; protected set; } = ToolAttachPoint.Generic;
        public virtual bool IsMergeableSide { get; protected set; }
        public virtual Item ExtendedDirtItem { get; protected set; }
		public virtual string ColourBlindTag { get; protected set; }

		private static readonly Item empty = ScriptableObject.CreateInstance<Item>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Item result = ScriptableObject.CreateInstance<Item>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.Prefab != Prefab) result.Prefab = Prefab;
            if (empty.ExtraTimeGranted != ExtraTimeGranted) result.ExtraTimeGranted = ExtraTimeGranted;
            if (empty.ItemValue != ItemValue) result.ItemValue = ItemValue;
            if (empty.Reward != Reward) result.Reward = Reward;
            if (empty.MaxOrderSharers != MaxOrderSharers) result.MaxOrderSharers = MaxOrderSharers;
            if (empty.SplitCount != SplitCount) result.SplitCount = SplitCount;
            if (empty.SplitSpeed != SplitSpeed) result.SplitSpeed = SplitSpeed;
            if (empty.AllowSplitMerging != AllowSplitMerging) result.AllowSplitMerging = AllowSplitMerging;
            if (empty.PreventExplicitSplit != PreventExplicitSplit) result.PreventExplicitSplit = PreventExplicitSplit;
            if (empty.SplitByComponents != SplitByComponents) result.SplitByComponents = SplitByComponents;
            if (empty.SplitByCopying != SplitByCopying) result.SplitByCopying = SplitByCopying;
            if (empty.IsIndisposable != IsIndisposable) result.IsIndisposable = IsIndisposable;
            if (empty.ItemCategory != ItemCategory) result.ItemCategory = ItemCategory;
            if (empty.ItemStorageFlags != ItemStorageFlags) result.ItemStorageFlags = ItemStorageFlags;
            if (empty.HoldPose != HoldPose) result.HoldPose = HoldPose;
            if (empty.IsMergeableSide != IsMergeableSide) result.IsMergeableSide = IsMergeableSide;

			if (!string.IsNullOrEmpty(ColourBlindTag))
				ColorblindUtils.itemLabels.Add(new ItemLabel { itemId = result.ID, label = ColourBlindTag });

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
        {
            Item result = (Item)gameDataObject;

            if (empty.Properties != Properties) result.Properties = Properties;
            if (empty.DirtiesTo != DirtiesTo) result.DirtiesTo = DirtiesTo;
            if (empty.MayRequestExtraItems != MayRequestExtraItems) result.MayRequestExtraItems = MayRequestExtraItems;
            if (empty.SplitSubItem != SplitSubItem) result.SplitSubItem = SplitSubItem;
            if (empty.SplitDepletedItems != SplitDepletedItems) result.SplitDepletedItems = SplitDepletedItems;
            if (empty.SplitByComponentsHolder != SplitByComponentsHolder) result.SplitByComponentsHolder = SplitByComponentsHolder;
            if (empty.RefuseSplitWith != RefuseSplitWith) result.RefuseSplitWith = RefuseSplitWith;
            if (empty.DisposesTo != DisposesTo) result.DisposesTo = DisposesTo;
            if (empty.DedicatedProvider != DedicatedProvider) result.DedicatedProvider = DedicatedProvider;
            if (empty.ExtendedDirtItem != ExtendedDirtItem) result.ExtendedDirtItem = ExtendedDirtItem;

            FieldInfo processes = ReflectionUtils.GetField<Item>("Processes");

            if (processes.GetValue(empty) != Processes) processes.SetValue(result, Processes);
        }
    }
}
using KitchenData;
using System;
using System.Collections.Generic;
using System.Reflection;
using KitchenLib.Utils;

namespace KitchenLib.Customs
{
    public abstract class CustomItemGroup : CustomItem
    {
		[Obsolete("Use List<ItemGroup.ItemSet>Sets instead")]
		public virtual List<ItemGroup.ItemSet> DerivedSets { get { return new List<ItemGroup.ItemSet>(); } }
		public virtual List<ItemGroup.ItemSet> Sets { get { return new List<ItemGroup.ItemSet>(); } }
		public virtual bool CanContainSide { get; internal set; }
		public virtual bool ApplyProcessesToComponents { get; internal set; }
		public virtual bool AutoCollapsing { get; internal set; }
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ItemGroup result = new ItemGroup();
            ItemGroup empty = new ItemGroup();
            
            if (empty.ID != ID) result.ID = ID;
            if (empty.Prefab != Prefab) result.Prefab = Prefab;
            //if (empty.DerivedProcesses != DerivedProcesses) result.DerivedProcesses = DerivedProcesses;
            if (empty.Properties != Properties) result.Properties = Properties;
            if (empty.ExtraTimeGranted != ExtraTimeGranted) result.ExtraTimeGranted = ExtraTimeGranted;
            if (empty.ItemValue != ItemValue) result.ItemValue = ItemValue;
            if (empty.Reward != Reward) result.Reward = Reward;
            if (empty.DirtiesTo != DirtiesTo) result.DirtiesTo = DirtiesTo;
            if (empty.MayRequestExtraItems != MayRequestExtraItems) result.MayRequestExtraItems = MayRequestExtraItems;
            if (empty.MaxOrderSharers != MaxOrderSharers) result.MaxOrderSharers = MaxOrderSharers;
            if (empty.SplitSubItem != SplitSubItem) result.SplitSubItem = SplitSubItem;
            if (empty.SplitCount != SplitCount) result.SplitCount = SplitCount;
            if (empty.SplitSpeed != SplitSpeed) result.SplitSpeed = SplitSpeed;
            if (empty.SplitDepletedItems != SplitDepletedItems) result.SplitDepletedItems = SplitDepletedItems;
            if (empty.AllowSplitMerging != AllowSplitMerging) result.AllowSplitMerging = AllowSplitMerging;
            if (empty.PreventExplicitSplit != PreventExplicitSplit) result.PreventExplicitSplit = PreventExplicitSplit;
            if (empty.SplitByComponents != SplitByComponents) result.SplitByComponents = SplitByComponents;
            if (empty.SplitByComponentsHolder != SplitByComponentsHolder) result.SplitByComponentsHolder = SplitByComponentsHolder;
            if (empty.SplitByCopying != SplitByCopying) result.SplitByCopying = SplitByCopying;
            if (empty.RefuseSplitWith != RefuseSplitWith) result.RefuseSplitWith = RefuseSplitWith;
            if (empty.DisposesTo != DisposesTo) result.DisposesTo = DisposesTo;
            if (empty.IsIndisposable != IsIndisposable) result.IsIndisposable = IsIndisposable;
            if (empty.ItemCategory != ItemCategory) result.ItemCategory = ItemCategory;
            if (empty.ItemStorageFlags != ItemStorageFlags) result.ItemStorageFlags = ItemStorageFlags;
            if (empty.DedicatedProvider != DedicatedProvider) result.DedicatedProvider = DedicatedProvider;
            if (empty.HoldPose != HoldPose) result.HoldPose = HoldPose;
            if (empty.IsMergeableSide != IsMergeableSide) result.IsMergeableSide = IsMergeableSide;

			if (empty.CanContainSide != CanContainSide) result.CanContainSide = CanContainSide;
            if (empty.ApplyProcessesToComponents != ApplyProcessesToComponents) result.ApplyProcessesToComponents = ApplyProcessesToComponents;
            if (empty.AutoCollapsing != AutoCollapsing) result.AutoCollapsing = AutoCollapsing;

			FieldInfo sets = ReflectionUtils.GetField<ItemGroup>("Sets");
			FieldInfo processes = ReflectionUtils.GetField<Item>("Processes");

			if (sets.GetValue(empty) != Sets) sets.SetValue(result, Sets);
			if (processes.GetValue(empty) != Sets) processes.SetValue(result, Processes);

			gameDataObject = result;
        }
    }
}
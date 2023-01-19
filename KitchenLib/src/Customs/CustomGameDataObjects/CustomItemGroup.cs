using KitchenData;
using System;
using System.Collections.Generic;
using System.Reflection;
using KitchenLib.Utils;
using System.Linq;
using UnityEngine;
using KitchenLib.Colorblind;

namespace KitchenLib.Customs
{
    public abstract class CustomItemGroup : CustomItem
    {
        public virtual List<ItemGroup.ItemSet> Sets { get; protected set; } = new List<ItemGroup.ItemSet>();
        public virtual bool CanContainSide { get; protected set; }
        public virtual bool ApplyProcessesToComponents { get; protected set; }
        public virtual bool AutoCollapsing { get; protected set; }

        private static readonly ItemGroup empty = ScriptableObject.CreateInstance<ItemGroup>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ItemGroup result = ScriptableObject.CreateInstance<ItemGroup>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<ItemGroup>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

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

			if (ColourBlindTag != "")
				ColorblindUtils.itemLabels.Add(new ItemLabel { itemId = result.ID, label = ColourBlindTag });

			if (empty.CanContainSide != CanContainSide) result.CanContainSide = CanContainSide;
            if (empty.ApplyProcessesToComponents != ApplyProcessesToComponents) result.ApplyProcessesToComponents = ApplyProcessesToComponents;
            if (empty.AutoCollapsing != AutoCollapsing) result.AutoCollapsing = AutoCollapsing;


            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
        {
            ItemGroup result = (ItemGroup)gameDataObject;

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
            FieldInfo sets = ReflectionUtils.GetField<ItemGroup>("Sets");

            if (processes.GetValue(empty) != Processes) processes.SetValue(result, Processes);
            if (sets.GetValue(empty) != Sets) sets.SetValue(gameDataObject, Sets);
        }
    }
}
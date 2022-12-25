using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomItem : CustomGameDataObject
    {
		public virtual GameObject Prefab { get; internal set; }

		[Obsolete("Use the Hashset<Item.ItemProcess>Processes instead")]
		public virtual List<Item.ItemProcess> DerivedProcesses { get { return new List<Item.ItemProcess>(); } }
		public virtual List<Item.ItemProcess> Processes { get { return new List<Item.ItemProcess>(); } }
		public virtual List<IItemProperty> Properties { get { return new List<IItemProperty>(); } }
		public virtual float ExtraTimeGranted { get; internal set; }
		public virtual ItemValue ItemValue { get { return ItemValue.Small; } }
		public virtual int Reward { get { return 1; } }
		public virtual Item DirtiesTo  { get; internal set; }
		public virtual List<Item> MayRequestExtraItems { get { return new List<Item>(); } }
		public virtual int MaxOrderSharers { get; internal set; }
		public virtual Item SplitSubItem { get; internal set; }
		public virtual int SplitCount { get { return 0; } }
		public virtual float SplitSpeed { get { return 1f; } }
		public virtual List<Item> SplitDepletedItems { get { return new List<Item>(); } }
		public virtual bool AllowSplitMerging { get; internal set; }
		public virtual bool PreventExplicitSplit { get; internal set; }
		public virtual bool SplitByComponents { get; internal set; }
		public virtual Item SplitByComponentsHolder { get; internal set; }
		public virtual bool SplitByCopying { get; internal set; }
		public virtual Item RefuseSplitWith { get; internal set; }
		public virtual Item DisposesTo { get; internal set; }
		public virtual bool IsIndisposable { get; internal set; }
		public virtual ItemCategory ItemCategory { get; internal set; }
		public virtual ItemStorage ItemStorageFlags { get; internal set; }
		public virtual Appliance DedicatedProvider { get; internal set; }
		public virtual ToolAttachPoint HoldPose { get { return ToolAttachPoint.Generic;}}
		public virtual bool IsMergeableSide { get; internal set; }
		public virtual Item ExtendedDirtItem { get; internal set; }
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Item result = ScriptableObject.CreateInstance<Item>();
			Item empty = ScriptableObject.CreateInstance<Item>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.Prefab != Prefab) result.Prefab = Prefab;
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
			if (empty.ExtendedDirtItem != ExtendedDirtItem) result.ExtendedDirtItem = ExtendedDirtItem;

			FieldInfo processes = ReflectionUtils.GetField<Item>("Processes");

			if (processes.GetValue(empty) != Processes) processes.SetValue(result, Processes);

            gameDataObject = result;
        }
    }
}
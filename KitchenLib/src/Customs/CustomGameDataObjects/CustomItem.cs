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
        public virtual GameObject SidePrefab { get; protected set; }
        public virtual List<Item.ItemProcess> Processes { get; protected set; }=new List<Item.ItemProcess>();
        public virtual List<IItemProperty> Properties { get; protected set; } = new List<IItemProperty>();
        public virtual float ExtraTimeGranted { get; protected set; }
        public virtual ItemValue ItemValue { get; protected set; } = ItemValue.Small;

		[Obsolete("Please use ItemValue instead.")]
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

		//private static readonly Item empty = ScriptableObject.CreateInstance<Item>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Item result = ScriptableObject.CreateInstance<Item>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Prefab != Prefab) result.Prefab = Prefab;
            if (result.ExtraTimeGranted != ExtraTimeGranted) result.ExtraTimeGranted = ExtraTimeGranted;
            if (result.ItemValue != ItemValue) result.ItemValue = ItemValue;
            if (result.MaxOrderSharers != MaxOrderSharers) result.MaxOrderSharers = MaxOrderSharers;
            if (result.SplitCount != SplitCount) result.SplitCount = SplitCount;
            if (result.SplitSpeed != SplitSpeed) result.SplitSpeed = SplitSpeed;
            if (result.AllowSplitMerging != AllowSplitMerging) result.AllowSplitMerging = AllowSplitMerging;
            if (result.PreventExplicitSplit != PreventExplicitSplit) result.PreventExplicitSplit = PreventExplicitSplit;
            if (result.SplitByComponents != SplitByComponents) result.SplitByComponents = SplitByComponents;
            if (result.SplitByCopying != SplitByCopying) result.SplitByCopying = SplitByCopying;
            if (result.IsIndisposable != IsIndisposable) result.IsIndisposable = IsIndisposable;
            if (result.ItemCategory != ItemCategory) result.ItemCategory = ItemCategory;
            if (result.ItemStorageFlags != ItemStorageFlags) result.ItemStorageFlags = ItemStorageFlags;
            if (result.HoldPose != HoldPose) result.HoldPose = HoldPose;
            if (result.IsMergeableSide != IsMergeableSide) result.IsMergeableSide = IsMergeableSide;

			if (!string.IsNullOrEmpty(ColourBlindTag))
				ColorblindUtils.itemLabels.Add(new ItemLabel { itemId = result.ID, label = ColourBlindTag });

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Item result = (Item)gameDataObject;

            if (result.Properties != Properties) result.Properties = Properties;
            if (result.DirtiesTo != DirtiesTo) result.DirtiesTo = DirtiesTo;
            if (result.MayRequestExtraItems != MayRequestExtraItems) result.MayRequestExtraItems = MayRequestExtraItems;
            if (result.SplitSubItem != SplitSubItem) result.SplitSubItem = SplitSubItem;
            if (result.SplitDepletedItems != SplitDepletedItems) result.SplitDepletedItems = SplitDepletedItems;
            if (result.SplitByComponentsHolder != SplitByComponentsHolder) result.SplitByComponentsHolder = SplitByComponentsHolder;
            if (result.RefuseSplitWith != RefuseSplitWith) result.RefuseSplitWith = RefuseSplitWith;
            if (result.DisposesTo != DisposesTo) result.DisposesTo = DisposesTo;
            if (result.DedicatedProvider != DedicatedProvider) result.DedicatedProvider = DedicatedProvider;
            if (result.ExtendedDirtItem != ExtendedDirtItem) result.ExtendedDirtItem = ExtendedDirtItem;

            FieldInfo processes = ReflectionUtils.GetField<Item>("Processes");

            if (processes.GetValue(result) != Processes) processes.SetValue(result, Processes);
        }
    }
}
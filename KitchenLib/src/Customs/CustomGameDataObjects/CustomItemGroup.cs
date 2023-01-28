using KitchenData;
using System;
using System.Collections.Generic;
using System.Reflection;
using KitchenLib.Utils;
using System.Linq;
using UnityEngine;
using KitchenLib.Colorblind;
using KitchenLib.References;
using Kitchen;
using System.CodeDom;

namespace KitchenLib.Customs
{
	public abstract class CustomItemGroup : CustomItemGroup<ItemGroupView> { }
    public abstract class CustomItemGroup<T> : CustomItem where T : ItemGroupView
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

			if (!string.IsNullOrEmpty(ColourBlindTag))
				ColorblindUtils.itemLabels.Add(new ItemLabel { itemId = result.ID, label = ColourBlindTag });

			if (empty.CanContainSide != CanContainSide) result.CanContainSide = CanContainSide;
            if (empty.ApplyProcessesToComponents != ApplyProcessesToComponents) result.ApplyProcessesToComponents = ApplyProcessesToComponents;
            if (empty.AutoCollapsing != AutoCollapsing) result.AutoCollapsing = AutoCollapsing;


            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
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
            if (sets.GetValue(empty) != Sets)
            {
                for (int setIndex = 0; setIndex < Sets.Count; setIndex++)
                {
                    ItemGroup.ItemSet set = Sets[setIndex];
                    for (int itemIndex = 0; itemIndex < set.Items.Count; itemIndex++)
                    {
                        Item item = set.Items[itemIndex];
                        if (item == null || item.ID == 0)
                        {
                            Main.instance.Log($"Found null or zero-ID item in an ItemSet in class {GetType().FullName} (set index {setIndex}, item index {itemIndex}). This will likely cause the game to crash.");
                        }
                    }
                }
                sets.SetValue(gameDataObject, Sets);
            }

			//Setup ItemGroupView for this ItemGroup
			T localView = result.Prefab.GetComponent<T>();
			if (localView == null)
				localView = result.Prefab.AddComponent<T>();
			
			FieldInfo subviewContainer = ReflectionUtils.GetField<ItemGroupView>("SubviewContainer");
			FieldInfo subviewPrefab = ReflectionUtils.GetField<ItemGroupView>("SubviewPrefab");
			Transform sidesContainer = null;
			if (result.Prefab != null)
				sidesContainer = result.Prefab.transform.Find("Side Container");
			
			if (sidesContainer != null)
			{
				subviewContainer.SetValue(localView, sidesContainer.gameObject);
				ItemGroup plated_burger = gameData.Get<ItemGroup>(ItemGroupReferences.BurgerPlated);
				ItemGroupView burgerView = plated_burger.Prefab.GetComponent<ItemGroupView>();
				subviewPrefab.SetValue(localView, subviewPrefab.GetValue(burgerView));
			}
			else
			{
				Main.instance.Log($"Could not find Side Container in prefab for ItemGroup {result.ID} ({result.name}).");
			}
		}
	}
}
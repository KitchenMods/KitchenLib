using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomItemGroup : CustomItemGroup<ItemGroupView> { }
    public abstract class CustomItemGroup<T> : CustomItem<ItemGroup> where T : ItemGroupView
    {
        public virtual List<ItemGroup.ItemSet> Sets { get; protected set; } = new List<ItemGroup.ItemSet>();
        public virtual bool CanContainSide { get; protected set; }
        public virtual bool ApplyProcessesToComponents { get; protected set; }
        public virtual bool AutoCollapsing { get; protected set; }
        public virtual bool AutoSetupItemGroupView { get; protected set; } = true;
		public virtual List<ItemGroupView.ColourBlindLabel> Labels { get; protected set; } = new List<ItemGroupView.ColourBlindLabel>();

		//private static readonly ItemGroup empty = ScriptableObject.CreateInstance<ItemGroup>();
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ItemGroup result = ScriptableObject.CreateInstance<ItemGroup>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<ItemGroup>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

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

			//if (!string.IsNullOrEmpty(ColourBlindTag))
			//ColorblindUtils.itemLabels.Add(new ItemLabel { itemId = result.ID, label = ColourBlindTag });

            if (RewardOverride != -1)
                ItemOverrides.AddRewardOverride(result.ID, RewardOverride);

            if (result.CanContainSide != CanContainSide) result.CanContainSide = CanContainSide;
            if (result.ApplyProcessesToComponents != ApplyProcessesToComponents) result.ApplyProcessesToComponents = ApplyProcessesToComponents;
            if (result.AutoCollapsing != AutoCollapsing) result.AutoCollapsing = AutoCollapsing;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            ItemGroup result = (ItemGroup)gameDataObject;

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
            FieldInfo sets = ReflectionUtils.GetField<ItemGroup>("Sets");

            if (processes.GetValue(result) != Processes) processes.SetValue(result, Processes);
            if (sets.GetValue(result) != Sets)
            {
                for (int setIndex = 0; setIndex < Sets.Count; setIndex++)
                {
                    ItemGroup.ItemSet set = Sets[setIndex];
                    for (int itemIndex = 0; itemIndex < set.Items.Count; itemIndex++)
                    {
                        Item item = set.Items[itemIndex];
                        if (item == null || item.ID == 0)
                        {
                            Main.LogWarning($"Found null or zero-ID item in an ItemSet in class {GetType().FullName} (set index {setIndex}, item index {itemIndex}). This will likely cause the game to crash.");
                        }
                    }
                }
                sets.SetValue(gameDataObject, Sets);
            }

            //Setup ItemGroupView for this ItemGroup
            if (AutoSetupItemGroupView)
            {
                T localView = result.Prefab.GetComponent<T>();
                if (localView == null)
                    localView = result.Prefab.AddComponent<T>();

                if (CanContainSide)
                {
                    ItemGroupViewUtils.AddSideContainer(gameData, result, localView);
                }
			}
			Item steak = (Item)GDOUtils.GetExistingGDO(ItemReferences.SteakMedium);
			if (steak != null)
			{
				GameObject ColorBlind = GameObject.Instantiate(steak.Prefab.transform.Find("Colour Blind").gameObject);
				ColorBlind.transform.SetParent(result.Prefab.transform);
				ColorBlind.transform.localPosition = new Vector3(0, 0, 0);

				FieldInfo info = ReflectionUtils.GetField<T>("ColourblindLabel");
				T x = result.Prefab.GetComponent<T>();
				ColorBlind.transform.Find("Title").GetComponent<TextMeshPro>().text = "";
				info.SetValue(x, ColorBlind.transform.Find("Title").GetComponent<TextMeshPro>());

				if (Labels != null)
				{
					FieldInfo info2 = ReflectionUtils.GetField<T>("ComponentLabels");
					info2.SetValue(x, Labels);
				}
			}

		}
    }
}
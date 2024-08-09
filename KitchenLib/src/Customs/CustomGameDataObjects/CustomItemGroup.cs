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
	    // Base-Game Variables
        public virtual List<ItemGroup.ItemSet> Sets { get; protected set; } = new List<ItemGroup.ItemSet>();
        public virtual bool CanContainSide { get; protected set; }
        public virtual bool ApplyProcessesToComponents { get; protected set; }
        public virtual bool AutoCollapsing { get; protected set; }
        
        // KitchenLib Variables
        public virtual bool AutoSetupItemGroupView { get; protected set; } = true;
		public virtual List<ItemGroupView.ColourBlindLabel> Labels { get; protected set; } = new List<ItemGroupView.ColourBlindLabel>();
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ItemGroup result = ScriptableObject.CreateInstance<ItemGroup>();

			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Prefab", Prefab);
			OverrideVariable(result, "AutomaticItemProcess", AutomaticItemProcess);
			OverrideVariable(result, "ExtraTimeGranted", ExtraTimeGranted);
			OverrideVariable(result, "EatingTime", EatingTime);
			OverrideVariable(result, "ItemValue", ItemValue);
			OverrideVariable(result, "IsConsumedByCustomer", IsConsumedByCustomer);
			OverrideVariable(result, "MaxOrderSharers", MaxOrderSharers);
			OverrideVariable(result, "AlwaysOrderAdditionalItem", AlwaysOrderAdditionalItem);
			OverrideVariable(result, "AutoSatisfied", AutoSatisfied);
			OverrideVariable(result, "SplitCount", SplitCount);
			OverrideVariable(result, "SplitSpeed", SplitSpeed);
			OverrideVariable(result, "AllowSplitMerging", AllowSplitMerging);
			OverrideVariable(result, "PreventExplicitSplit", PreventExplicitSplit);
			OverrideVariable(result, "SplitByComponents", SplitByComponents);
			OverrideVariable(result, "SplitByCopying", SplitByCopying);
			OverrideVariable(result, "IsIndisposable", IsIndisposable);
			OverrideVariable(result, "ItemCategory", ItemCategory);
			OverrideVariable(result, "ItemStorageFlags", ItemStorageFlags);
			OverrideVariable(result, "HoldPose", HoldPose);
			OverrideVariable(result, "IsMergeableSide", IsMergeableSide);
			OverrideVariable(result, "RepeatOrderMin", RepeatOrderMin);
			OverrideVariable(result, "RepeatOrderMax", RepeatOrderMax);
			OverrideVariable(result, "CanContainSide", CanContainSide);
			OverrideVariable(result, "ApplyProcessesToComponents", ApplyProcessesToComponents);
			OverrideVariable(result, "AutoCollapsing", AutoCollapsing);
			
			if (RewardOverride != -1)
			{
				Main.LogDebug($"Assigning : {RewardOverride} >> RewardOverride");
				ItemOverrides.AddRewardOverride(result.ID, RewardOverride);
			}

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            ItemGroup result = (ItemGroup)gameDataObject;

            OverrideVariable(result, "Properties", Properties);
            OverrideVariable(result, "DirtiesTo", DirtiesTo);
            OverrideVariable(result, "MayRequestExtraItems", MayRequestExtraItems);
            OverrideVariable(result, "SatisfiedBy", SatisfiedBy);
            OverrideVariable(result, "NeedsIngredients", NeedsIngredients);
            OverrideVariable(result, "SplitSubItem", SplitSubItem);
            OverrideVariable(result, "SplitDepletedItems", SplitDepletedItems);
            OverrideVariable(result, "SplitByComponentsHolder", SplitByComponentsHolder);
            OverrideVariable(result, "SplitByComponentsWrapper", SplitByComponentsWrapper);
            OverrideVariable(result, "RefuseSplitWith", RefuseSplitWith);
            OverrideVariable(result, "DisposesTo", DisposesTo);
            OverrideVariable(result, "DedicatedProvider", DedicatedProvider);
            OverrideVariable(result, "CreditSourceDish", CreditSourceDish);
            OverrideVariable(result, "ExtendedDirtItem", ExtendedDirtItem);
            OverrideVariable(result, "Processes", Processes);

            FieldInfo sets = ReflectionUtils.GetField<ItemGroup>("Sets");

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
                OverrideVariable(result, "Sets", Sets);
            }

            
            //Setup ItemGroupView for this ItemGroup
            if (AutoSetupItemGroupView)
			{
				Main.LogDebug($"Setting up ItemGroupView as {typeof(T).FullName}");
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
				Main.LogDebug($"Setting up Colour Blind Labels");
				GameObject ColorBlind = GameObject.Instantiate(steak.Prefab.transform.Find("Colour Blind").gameObject);
				ColorBlind.name = "Colour Blind";
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
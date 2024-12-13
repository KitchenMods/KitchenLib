using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomItem : CustomItem<Item> { }
    public abstract class CustomItem<T> : CustomGameDataObject<T>, ICustomHasPrefab where T : Item
    {
	    // Base-Game Variables
        public virtual GameObject Prefab { get; protected set; }
        public virtual List<Item.ItemProcess> Processes { get; protected set; } = new List<Item.ItemProcess>();
        public virtual Item.ItemProcess AutomaticItemProcess { get; protected set; }
        public virtual List<IItemProperty> Properties { get; protected set; } = new List<IItemProperty>();
        public virtual float ExtraTimeGranted { get; protected set; }
        public virtual Factor EatingTime { get; protected set; }
        public virtual ItemValue ItemValue { get; protected set; } = ItemValue.Small;

        [Obsolete("Please use ItemValue instead.")]
        public virtual int Reward { get; protected set; } = 1;
        public virtual Item DirtiesTo { get; protected set; }
        public virtual bool IsConsumedByCustomer { get; protected set; }
        public virtual List<Item> MayRequestExtraItems { get; protected set; } = new List<Item>();
        public virtual int MaxOrderSharers { get; protected set; }
        public virtual int AlwaysOrderAdditionalItem { get; protected set; }
        public virtual bool AutoSatisfied { get; protected set; }
        public virtual List<Item> SatisfiedBy { get; protected set; } = new List<Item>();
        public virtual List<Item> NeedsIngredients { get; protected set; } = new List<Item>();
        public virtual Item SplitSubItem { get; protected set; }
        public virtual int SplitCount { get; protected set; } = 0;
        public virtual float SplitSpeed { get; protected set; } = 1f;
        public virtual List<Item> SplitDepletedItems { get; protected set; } = new List<Item>();
        public virtual bool AllowSplitMerging { get; protected set; }
        public virtual bool PreventExplicitSplit { get; protected set; }
        public virtual bool SplitByComponents { get; protected set; }
        public virtual Item SplitByComponentsHolder { get; protected set; }
        public virtual Item SplitByComponentsWrapper { get; protected set; }
        public virtual bool SplitByCopying { get; protected set; }
        public virtual Item RefuseSplitWith { get; protected set; }
        public virtual Item DisposesTo { get; protected set; }
        public virtual bool IsIndisposable { get; protected set; }
        public virtual ItemCategory ItemCategory { get; protected set; }
        public virtual ItemStorage ItemStorageFlags { get; protected set; }
        public virtual Appliance DedicatedProvider { get; protected set; }
        public virtual ToolAttachPoint HoldPose { get; protected set; } = ToolAttachPoint.Generic;
        public virtual bool IsMergeableSide { get; protected set; }
        public virtual Dish CreditSourceDish { get; protected set; }
        public virtual Item ExtendedDirtItem { get; protected set; }
        public virtual int RepeatOrderMin { get; protected set; }
        public virtual int RepeatOrderMax { get; protected set; }
        
        // KitchenLib Variables
        public virtual GameObject SidePrefab { get; protected set; }
        public virtual string ColourBlindTag { get; protected set; }
        public virtual int RewardOverride { get; protected set; } = -1;

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Item result = ScriptableObject.CreateInstance<Item>();
            
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

			if (!string.IsNullOrEmpty(ColourBlindTag))
			{
				Main.LogError($"Adding ColourBlindTag '{ColourBlindTag}'");
				Item steak = (Item)GDOUtils.GetExistingGDO(ItemReferences.SteakMedium);
				if (steak != null)
				{
					GameObject ColorBlind = GameObject.Instantiate(steak.Prefab.transform.Find("Colour Blind").gameObject);
					ColorBlind.name = "Colour Blind";
					ColorBlind.transform.SetParent(result.Prefab.transform);
					ColorBlind.transform.Find("Title").GetComponent<TMP_Text>().text = ColourBlindTag;
				}
			}

			if (RewardOverride != -1)
			{
				Main.LogDebug($"Assigning : {RewardOverride} >> RewardOverride");
				ItemOverrides.AddRewardOverride(result.ID, RewardOverride);
			}

			gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Item result = (Item)gameDataObject;
            
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
            
            if (SidePrefab == null)
            {
	            Main.LogError($"Assigning fallback side prefab");
	            SidePrefab = result.Prefab ?? Main.bundle.LoadAsset<GameObject>("Error_Item");
            }
            if (result.Prefab == null)
            {
	            Main.LogError($"Assigning fallback prefab");
	            result.Prefab = Main.bundle.LoadAsset<GameObject>("Error_Item");
            }
		}

        public override void OnRegister(GameDataObject gameDataObject)
        {
            IHasPrefab gdo = gameDataObject as IHasPrefab;
            if (gdo?.Prefab != null)
            {
                SetupPrefab(gdo.Prefab);
            }

            base.OnRegister(gameDataObject);
        }
        [Obsolete("Please use OnRegister")]
        public virtual void SetupPrefab(GameObject prefab) { }
    }
}
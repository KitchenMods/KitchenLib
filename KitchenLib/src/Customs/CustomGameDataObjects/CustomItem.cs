using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
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
        public virtual int Reward { get { return 1; } }
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
        
        // KitchenLib Variables
        public virtual GameObject SidePrefab { get; protected set; }
        public virtual string ColourBlindTag { get; protected set; }
        public virtual int RewardOverride { get; protected set; } = -1;

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Item result = ScriptableObject.CreateInstance<Item>();

			Main.LogDebug($"[CustomItem.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Prefab != Prefab) result.Prefab = Prefab;
			if (!AutomaticItemProcess.Equals(result.AutomaticItemProcess)) result.AutomaticItemProcess = AutomaticItemProcess;
            if (result.ExtraTimeGranted != ExtraTimeGranted) result.ExtraTimeGranted = ExtraTimeGranted;
            if (!result.EatingTime.Equals(EatingTime)) result.EatingTime = EatingTime;
            if (result.ItemValue != ItemValue) result.ItemValue = ItemValue;
			if (result.IsConsumedByCustomer != IsConsumedByCustomer) result.IsConsumedByCustomer = IsConsumedByCustomer;
            if (result.MaxOrderSharers != MaxOrderSharers) result.MaxOrderSharers = MaxOrderSharers;
            if (result.AlwaysOrderAdditionalItem != AlwaysOrderAdditionalItem) result.AlwaysOrderAdditionalItem = AlwaysOrderAdditionalItem;
            if (result.AutoSatisfied != AutoSatisfied) result.AutoSatisfied = AutoSatisfied;
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

			Main.LogDebug($"[CustomItem.Convert] [1.2] Converting Overrides");

			if (!string.IsNullOrEmpty(ColourBlindTag))
			{
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
                ItemOverrides.AddRewardOverride(result.ID, RewardOverride);

			if (SidePrefab == null)
			{
				Main.LogDebug($"[CustomItem.Convert] [1.3] Assigning Error Prefab");
				SidePrefab = result.Prefab ?? Main.bundle.LoadAsset<GameObject>("Error_Item");
			}
			if (result.Prefab == null)
			{
				Main.LogDebug($"[CustomItem.Convert] [1.4] Assigning Error Side Prefab");
				result.Prefab = Main.bundle.LoadAsset<GameObject>("Error_Item");
			}

			gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Item result = (Item)gameDataObject;

			Main.LogDebug($"[CustomItem.AttachDependentProperties] [1.1] Converting Base");

			if (result.Properties != Properties) result.Properties = Properties;
            if (result.DirtiesTo != DirtiesTo) result.DirtiesTo = DirtiesTo;
            if (result.MayRequestExtraItems != MayRequestExtraItems) result.MayRequestExtraItems = MayRequestExtraItems;
            if (result.SatisfiedBy != SatisfiedBy) result.SatisfiedBy = SatisfiedBy;
            if (result.NeedsIngredients != NeedsIngredients) result.NeedsIngredients = NeedsIngredients;
            if (result.SplitSubItem != SplitSubItem) result.SplitSubItem = SplitSubItem;
            if (result.SplitDepletedItems != SplitDepletedItems) result.SplitDepletedItems = SplitDepletedItems;
            if (result.SplitByComponentsHolder != SplitByComponentsHolder) result.SplitByComponentsHolder = SplitByComponentsHolder;
            if (result.SplitByComponentsWrapper != SplitByComponentsWrapper) result.SplitByComponentsWrapper = SplitByComponentsWrapper;
            if (result.RefuseSplitWith != RefuseSplitWith) result.RefuseSplitWith = RefuseSplitWith;
            if (result.DisposesTo != DisposesTo) result.DisposesTo = DisposesTo;
            if (result.DedicatedProvider != DedicatedProvider) result.DedicatedProvider = DedicatedProvider;
            if (result.CreditSourceDish != CreditSourceDish) result.CreditSourceDish = CreditSourceDish;
            if (result.ExtendedDirtItem != ExtendedDirtItem) result.ExtendedDirtItem = ExtendedDirtItem;

            FieldInfo processes = ReflectionUtils.GetField<Item>("Processes");

            if (processes.GetValue(result) != Processes) processes.SetValue(result, Processes);
		}

        public override void OnRegister(GameDataObject gameDataObject)
        {
            IHasPrefab gdo = gameDataObject as IHasPrefab;
            if (gdo?.Prefab != null)
            {
                SetupPrefab(gdo.Prefab);
            }
            else
            {
                Main.LogWarning($"Item/ItemGroup with ID '{UniqueNameID}' does not have a prefab set.");
            }

            base.OnRegister(gameDataObject);
        }

        public virtual void SetupPrefab(GameObject prefab) { }
    }
}
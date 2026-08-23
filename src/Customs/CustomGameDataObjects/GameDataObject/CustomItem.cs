using System;
using System.Collections.Generic;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KitchenLib.Customs
{
	public abstract class CustomItem : CustomItem<Item>
	{
	}

	public abstract class CustomItem<T> : CustomGameDataObject<T>, ICustomHasPrefab where T : GameDataObject
	{

		#region Base Game Variables

		public virtual GameObject Prefab { get; protected set; }
		public virtual List<Item.ItemProcess> Processes { get; protected set; } = new();
		public virtual Item.ItemProcess AutomaticItemProcess { get; protected set; }
		public virtual List<IItemProperty> Properties { get; protected set; } = new();
		public virtual bool RequiresCleaning { get; protected set; }
		public virtual float ExtraTimeGranted { get; protected set; }
		public virtual Factor EatingTime { get; protected set; }
		public virtual ItemValue ItemValue { get; protected set; } = ItemValue.Small;
		public virtual int Reward { get; protected set; } = 1;
		public virtual Item DirtiesTo { get; protected set; }
		public virtual bool IsConsumedByCustomer { get; protected set; }
		public virtual List<Item> MayRequestExtraItems { get; protected set; } = new();
		public virtual int MaxOrderSharers { get; protected set; }
		public virtual int AlwaysOrderAdditionalItem { get; protected set; }
		public virtual bool AutoSatisfied { get; protected set; }
		public virtual int RepeatOrderMin { get; protected set; }
		public virtual int RepeatOrderMax { get; protected set; }
		public virtual bool CanBeOrderedPiecemeal { get; protected set; }
		public virtual List<Item> SatisfiedBy { get; protected set; } = new();
		public virtual List<Item> NeedsIngredients { get; protected set; } = new();
		public virtual Item SplitSubItem { get; protected set; }
		public virtual int SplitCount { get; protected set; }
		public virtual float SplitSpeed { get; protected set; } = 1f;
		public virtual List<Item> SplitDepletedItems { get; protected set; } = new();
		public virtual bool AllowSplitMerging { get; protected set; }
		public virtual bool PreventExplicitSplit { get; protected set; }
		public virtual bool SplitByComponents { get; protected set; }
		public virtual Item SplitByComponentsHolder { get; protected set; }
		public virtual Item SplitByComponentsWrapper { get; protected set; }
		public virtual bool SplitByCopying { get; protected set; }
		public virtual Item RefuseSplitWith { get; protected set; }
		public virtual bool HasImplicitlyModifiedComponents { get; protected set; }
		public virtual Item DisposesTo { get; protected set; }
		public virtual bool IsIndisposable { get; protected set; }
		public virtual bool IsSinkDisposable { get; protected set; }
		public virtual ItemCategory ItemCategory { get; protected set; }
		public virtual ItemStorage ItemStorageFlags { get; protected set; }
		public virtual Appliance DedicatedProvider { get; protected set; }
		public virtual ToolAttachPoint HoldPose { get; protected set; }
		public virtual bool IsMergeableSide { get; protected set; }
		public virtual Dish CreditSourceDish { get; protected set; }
		public virtual Item ExtendedDirtItem { get; protected set; }
		public virtual bool IsOverchoppable { get; protected set; }

		#endregion

		#region KitchenLib Variables

		public virtual GameObject SidePrefab { get; protected set; }
		public virtual string ColourBlindTag { get; protected set; }
		public virtual int RewardOverride { get; protected set; } = -1;

		#endregion
		
		[Obsolete("Please use OnRegister")]
		public virtual void SetupPrefab(GameObject prefab)
		{
		}

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is Item item)
			{
				#region Apply Properties

				OverrideVariable(item, "Prefab", Prefab);
				OverrideVariable(item, "Properties", Properties);
				OverrideVariable(item, "RequiresCleaning", RequiresCleaning);
				OverrideVariable(item, "ExtraTimeGranted", ExtraTimeGranted);
				OverrideVariable(item, "EatingTime", EatingTime);
				OverrideVariable(item, "ItemValue", ItemValue);
				OverrideVariable(item, "IsConsumedByCustomer", IsConsumedByCustomer);
				OverrideVariable(item, "MaxOrderSharers", MaxOrderSharers);
				OverrideVariable(item, "AlwaysOrderAdditionalItem", AlwaysOrderAdditionalItem);
				OverrideVariable(item, "AutoSatisfied", AutoSatisfied);
				OverrideVariable(item, "RepeatOrderMin", RepeatOrderMin);
				OverrideVariable(item, "RepeatOrderMax", RepeatOrderMax);
				OverrideVariable(item, "CanBeOrderedPiecemeal", CanBeOrderedPiecemeal);
				OverrideVariable(item, "SplitCount", SplitCount);
				OverrideVariable(item, "SplitSpeed", SplitSpeed);
				OverrideVariable(item, "AllowSplitMerging", AllowSplitMerging);
				OverrideVariable(item, "PreventExplicitSplit", PreventExplicitSplit);
				OverrideVariable(item, "SplitByComponents", SplitByComponents);
				OverrideVariable(item, "SplitByCopying", SplitByCopying);
				OverrideVariable(item, "HasImplicitlyModifiedComponents", HasImplicitlyModifiedComponents);
				OverrideVariable(item, "IsIndisposable", IsIndisposable);
				OverrideVariable(item, "IsSinkDisposable", IsSinkDisposable);
				OverrideVariable(item, "ItemCategory", ItemCategory);
				OverrideVariable(item, "ItemStorageFlags", ItemStorageFlags);
				OverrideVariable(item, "HoldPose", HoldPose);
				OverrideVariable(item, "IsMergeableSide", IsMergeableSide);
				OverrideVariable(item, "IsOverchoppable", IsOverchoppable);

				#endregion

				if (!string.IsNullOrEmpty(ColourBlindTag))
				{
					BaseMod.InternalLogger.LogDebug($"Adding ColourBlindTag '{ColourBlindTag}'");
					var steak = (Item)GDOUtils.GetExistingGDO(ItemReferences.SteakMedium);
					if (steak != null)
					{
						var colorBlind = Object.Instantiate(steak.Prefab.transform.Find("Colour Blind").gameObject);
						colorBlind.name = "Colour Blind";
						colorBlind.transform.SetParent(item.Prefab.transform);
						colorBlind.transform.Find("Title").GetComponent<TMP_Text>().text = ColourBlindTag;
					}
				}

				if (RewardOverride != -1)
				{
					BaseMod.InternalLogger.LogDebug($"Assigning : {RewardOverride} >> RewardOverride");
					ItemOverrides.AddRewardOverride(item.ID, RewardOverride);
				}
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is Item item)
			{
				#region Apply Properties

				OverrideVariable(item, "Processes", Processes);
				OverrideVariable(item, "AutomaticItemProcess", AutomaticItemProcess);
				OverrideVariable(item, "DirtiesTo", DirtiesTo);
				OverrideVariable(item, "MayRequestExtraItems", MayRequestExtraItems);
				OverrideVariable(item, "SatisfiedBy", SatisfiedBy);
				OverrideVariable(item, "NeedsIngredients", NeedsIngredients);
				OverrideVariable(item, "SplitSubItem", SplitSubItem);
				OverrideVariable(item, "SplitDepletedItems", SplitDepletedItems);
				OverrideVariable(item, "SplitByComponentsHolder", SplitByComponentsHolder);
				OverrideVariable(item, "SplitByComponentsWrapper", SplitByComponentsWrapper);
				OverrideVariable(item, "RefuseSplitWith", RefuseSplitWith);
				OverrideVariable(item, "DisposesTo", DisposesTo);
				OverrideVariable(item, "DedicatedProvider", DedicatedProvider);
				OverrideVariable(item, "CreditSourceDish", CreditSourceDish);
				OverrideVariable(item, "ExtendedDirtItem", ExtendedDirtItem);

				#endregion

				if (SidePrefab == null)
				{
					BaseMod.InternalLogger.LogError("Assigning fallback side prefab");
					SidePrefab = item.Prefab ?? Main.bundle.LoadAsset<GameObject>("Error_Item");
				}
			}
		}

		public override void OnRegister(GameDataObject gameDataObject)
		{
			var gdo = gameDataObject as IHasPrefab;
			if (gdo?.Prefab != null)
			{
				SetupPrefab(gdo.Prefab);
			}
			
			base.OnRegister(gameDataObject);
		}
	}
}
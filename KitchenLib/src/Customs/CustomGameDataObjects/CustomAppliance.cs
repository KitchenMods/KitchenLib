using Kitchen;
using KitchenData;
using KitchenLib.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomAppliance : CustomLocalisedGameDataObject<ApplianceInfo>
	{
		public virtual GameObject Prefab { get; protected set; }
		public virtual GameObject HeldAppliancePrefab { get; protected set; }
		public virtual List<Appliance.ApplianceProcesses> Processes { get; protected set; } = new List<Appliance.ApplianceProcesses>();
		public virtual List<IApplianceProperty> Properties { get; protected set; } = new List<IApplianceProperty>();
		public virtual IEffectRange EffectRange { get; protected set; }
		public virtual IEffectCondition EffectCondition { get; protected set; }
		public virtual IEffectType EffectType { get; protected set; }
		public virtual EffectRepresentation EffectRepresentation { get; protected set; }
		public virtual bool IsNonInteractive { get; protected set; }
		public virtual OccupancyLayer Layer { get; protected set; }
		public virtual bool ForceHighInteractionPriority { get; protected set; }

		[Obsolete("Please use PriceTier to set your price")]
		public virtual int PurchaseCost { get; protected set; } = 0;
		public virtual EntryAnimation EntryAnimation { get; protected set; }
		public virtual ExitAnimation ExitAnimation { get; protected set; }
		public virtual bool SkipRotationAnimation { get; protected set; }
		public virtual bool IsPurchasable { get; protected set; } = false;
		public virtual bool IsPurchasableAsUpgrade { get; protected set; }
		public virtual DecorationType ThemeRequired { get; protected set; }
		public virtual ShoppingTags ShoppingTags { get; protected set; } = ShoppingTags.None;
		public virtual RarityTier RarityTier { get; protected set; } = RarityTier.Common;
		public virtual PriceTier PriceTier { get; protected set; } = PriceTier.Medium;
		public virtual ShopRequirementFilter ShopRequirementFilter { get; protected set; }
		public virtual List<Appliance> RequiresForShop { get; protected set; } = new List<Appliance>();
		public virtual List<Process> RequiresProcessForShop { get; protected set; } = new List<Process>();
		public virtual bool StapleWhenMissing { get; protected set; }
		public virtual bool SellOnlyAsDuplicate { get; protected set; }
		public virtual bool SellOnlyAsUnique { get; protected set; }
		public virtual bool PreventSale { get; protected set; }
		public virtual List<Appliance> Upgrades { get; protected set; } = new List<Appliance>();

		[Obsolete("Should not be used by the user")]
		public virtual bool IsAnUpgrade { get; protected set; }
		public virtual bool IsNonCrated { get; protected set; }
		public virtual Item CrateItem { get; protected set; }

		[Obsolete("Please set your Name in Info")]
		public virtual string Name { get; protected set; } = "Appliance";

		[Obsolete("Please set your Description in Info")]
		public virtual string Description { get; protected set; } = "A little something for your restaurant";

		[Obsolete("Please set your Sections in Info")]
		public virtual List<Appliance.Section> Sections { get; protected set; } = new List<Appliance.Section>();

		[Obsolete("Please set your Tags in Info")]
		public virtual List<string> Tags { get; protected set; } = new List<string>();

		[Obsolete("Please create a custom system for rotations")]
		public virtual bool ForceIsRotationPossible() { return false; }

		[Obsolete("Please create a custom system for rotations")]
		public virtual bool IsRotationPossible(InteractionData data) { return true; }

		[Obsolete("Please create a custom system for rotations")]
		public virtual bool PreRotate(InteractionData data, bool isSecondary = false) { return false; }

		[Obsolete("Please create a custom system for rotations")]
		public virtual void PostRotate(InteractionData data) { }

		[Obsolete("Please create a custom system for interactions")]
		public virtual bool ForceIsInteractionPossible() { return false; }

		[Obsolete("Please create a custom system for interactions")]
		public virtual bool IsInteractionPossible(InteractionData data) { return true; }

		[Obsolete("Please create a custom system for interactions")]
		public virtual bool PreInteract(InteractionData data, bool isSecondary = false) { return false; }

		[Obsolete("Please create a custom system for interactions")]
		public virtual void PostInteract(InteractionData data) { }

		public virtual int PurchaseCostOverride { get; protected set; } = -1;

		//private static readonly Appliance empty = ScriptableObject.CreateInstance<Appliance>();
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			Appliance result = ScriptableObject.CreateInstance<Appliance>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));
			else
				result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == AssetReference.Counter));

			if (result.ID != ID) result.ID = ID;
			if (result.Prefab != Prefab) result.Prefab = Prefab;
			if (result.HeldAppliancePrefab != HeldAppliancePrefab) result.HeldAppliancePrefab = HeldAppliancePrefab;
			if (result.EffectRange != EffectRange) result.EffectRange = EffectRange;
			if (result.EffectCondition != EffectCondition) result.EffectCondition = EffectCondition;
			if (result.EffectType != EffectType) result.EffectType = EffectType;
			if (result.IsNonInteractive != IsNonInteractive) result.IsNonInteractive = IsNonInteractive;
			if (result.Layer != Layer) result.Layer = Layer;
			if (result.ForceHighInteractionPriority != ForceHighInteractionPriority) result.ForceHighInteractionPriority = ForceHighInteractionPriority;
			if (result.EntryAnimation != EntryAnimation) result.EntryAnimation = EntryAnimation;
			if (result.ExitAnimation != ExitAnimation) result.ExitAnimation = ExitAnimation;
			if (result.SkipRotationAnimation != SkipRotationAnimation) result.SkipRotationAnimation = SkipRotationAnimation;
			if (result.IsPurchasable != IsPurchasable) result.IsPurchasable = IsPurchasable;
			if (result.IsPurchasableAsUpgrade != IsPurchasableAsUpgrade) result.IsPurchasableAsUpgrade = IsPurchasableAsUpgrade;
			if (result.ThemeRequired != ThemeRequired) result.ThemeRequired = ThemeRequired;
			if (result.ShoppingTags != ShoppingTags) result.ShoppingTags = ShoppingTags;
			if (result.RarityTier != RarityTier) result.RarityTier = RarityTier;
			if (result.PriceTier != PriceTier) result.PriceTier = PriceTier;
			if (result.ShopRequirementFilter != ShopRequirementFilter) result.ShopRequirementFilter = ShopRequirementFilter;
			if (result.StapleWhenMissing != StapleWhenMissing) result.StapleWhenMissing = StapleWhenMissing;
			if (result.SellOnlyAsDuplicate != SellOnlyAsDuplicate) result.SellOnlyAsDuplicate = SellOnlyAsDuplicate;
			if (result.SellOnlyAsUnique != SellOnlyAsUnique) result.SellOnlyAsUnique = SellOnlyAsUnique;
			if (result.PreventSale != PreventSale) result.PreventSale = PreventSale;
			if (result.IsNonCrated != IsNonCrated) result.IsNonCrated = IsNonCrated;
			if (result.Info != Info) result.Info = Info;

			if (PurchaseCostOverride != -1)
			{
				Appliance_Patch.AddPurchaseCostOverride(result.ID, PurchaseCostOverride);
			}

			if (InfoList.Count > 0)
			{
				result.Info = new LocalisationObject<ApplianceInfo>();
				foreach ((Locale, ApplianceInfo) info in InfoList)
					result.Info.Add(info.Item1, info.Item2);
			}

			if (result.Info == null)
			{
				result.Info = new LocalisationObject<ApplianceInfo>();
				if (!result.Info.Has(Locale.English))
				{
					result.Info.Add(Locale.English, new ApplianceInfo
					{
						Name = Name,
						Description = Description,
						Sections = Sections,
						Tags = Tags
					});
				}
			}


			gameDataObject = result;
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			Appliance result = (Appliance)gameDataObject;

			if (result.Processes != Processes) result.Processes = Processes;
			if (result.Properties != Properties) result.Properties = Properties;
			if (result.EffectRepresentation != EffectRepresentation) result.EffectRepresentation = EffectRepresentation;
			if (result.RequiresForShop != RequiresForShop) result.RequiresForShop = RequiresForShop;
			if (result.RequiresProcessForShop != RequiresProcessForShop) result.RequiresProcessForShop = RequiresProcessForShop;
			if (result.Upgrades != Upgrades) result.Upgrades = Upgrades;
			if (result.CrateItem != CrateItem) result.CrateItem = CrateItem;
		}
	}
}
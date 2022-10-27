using KitchenData;
using System.Collections.Generic;
using UnityEngine;
using Kitchen;
using System.Linq;

namespace KitchenLib.Customs
{
    public abstract class CustomAppliance : CustomGameDataObject
    {
		public virtual GameObject Prefab { get; internal set;}
		public virtual GameObject HeldAppliancePrefab { get; internal set;}
		public virtual List<Appliance.ApplianceProcesses> Processes { get { return new List<Appliance.ApplianceProcesses>(); } }
		public virtual List<IApplianceProperty> Properties { get { return new List<IApplianceProperty>(); } }
		public virtual IEffectRange EffectRange { get; internal set;}
		public virtual IEffectCondition EffectCondition { get; internal set;}
		public virtual IEffectType EffectType { get; internal set;}
		public virtual EffectRepresentation EffectRepresentation { get; internal set;}
		public virtual bool IsNonInteractive { get; internal set;}
		public virtual OccupancyLayer Layer { get; internal set;}
		public virtual bool ForceHighInteractionPriority { get; internal set;}
		public virtual int PurchaseCost { get { return 0; } }
		public virtual EntryAnimation EntryAnimation { get; internal set;}
		public virtual ExitAnimation ExitAnimation { get; internal set;}
		public virtual bool SkipRotationAnimation { get; internal set;}
		public virtual bool IsPurchasable { get { return false; } }
		public virtual bool IsPurchasableAsUpgrade { get; internal set;}
		public virtual DecorationType ThemeRequired { get; internal set;}
		public virtual ShoppingTags ShoppingTags { get { return ShoppingTags.None; } }
		public virtual RarityTier RarityTier { get { return RarityTier.Common; } }
		public virtual PriceTier PriceTier { get { return PriceTier.Medium; } }
		public virtual ShopRequirementFilter ShopRequirementFilter { get; internal set;}
		public virtual List<Appliance> RequiresForShop { get { return new List<Appliance>(); } }
		public virtual List<Process> RequiresProcessForShop { get { return new List<Process>(); } }
		public virtual bool StapleWhenMissing { get; internal set;}
		public virtual bool SellOnlyAsDuplicate { get; internal set;}
		public virtual bool PreventSale { get; internal set;}
		public virtual List<Appliance> Upgrades { get { return new List<Appliance>(); } }
		public virtual bool IsAnUpgrade { get; internal set;}
		public virtual bool IsNonCrated { get; internal set;}
		public virtual Item CrateItem { get; internal set;}
		public virtual string Name { get { return "Appliance"; } }
		public virtual string Description { get { return "A little something for your restaurant"; } }
		public virtual List<Appliance.Section> Sections { get { return new List<Appliance.Section>(); } }
		public virtual List<string> Tags { get { return new List<string>(); } }

		public virtual bool ForceIsRotationPossible() { return false; }
		public virtual bool IsRotationPossible(InteractionData data) { return true; }
		public virtual bool PreRotate(InteractionData data, bool isSecondary = false) { return false; }
		public virtual void PostRotate(InteractionData data) { }
		public virtual bool ForceIsInteractionPossible() { return false; }
		public virtual bool IsInteractionPossible(InteractionData data) { return true; }
		public virtual bool PreInteract(InteractionData data, bool isSecondary = false) { return false; }
		public virtual void PostInteract(InteractionData data) { }
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Appliance result = new Appliance();
            Appliance empty = new Appliance();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));
            else
                result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == AssetReference.Counter));

            if (empty.ID != ID) result.ID = ID;
            if (empty.Prefab != Prefab) result.Prefab = Prefab;
            if (empty.HeldAppliancePrefab != HeldAppliancePrefab) result.HeldAppliancePrefab = HeldAppliancePrefab;
            if (empty.Processes != Processes) result.Processes = Processes;
            if (empty.Properties != Properties) result.Properties = Properties;
            if (empty.EffectRange != EffectRange) result.EffectRange = EffectRange;
            if (empty.EffectCondition != EffectCondition) result.EffectCondition = EffectCondition;
            if (empty.EffectType != EffectType) result.EffectType = EffectType;
            if (empty.EffectRepresentation != EffectRepresentation) result.EffectRepresentation = EffectRepresentation;
            if (empty.IsNonInteractive != IsNonInteractive) result.IsNonInteractive = IsNonInteractive;
            if (empty.Layer != Layer) result.Layer = Layer;
            if (empty.ForceHighInteractionPriority != ForceHighInteractionPriority) result.ForceHighInteractionPriority = ForceHighInteractionPriority;
            if (empty.PurchaseCost != PurchaseCost) result.PurchaseCost = PurchaseCost;
            if (empty.EntryAnimation != EntryAnimation) result.EntryAnimation = EntryAnimation;
            if (empty.ExitAnimation != ExitAnimation) result.ExitAnimation = ExitAnimation;
            if (empty.SkipRotationAnimation != SkipRotationAnimation) result.SkipRotationAnimation = SkipRotationAnimation;
            if (empty.IsPurchasable != IsPurchasable) result.IsPurchasable = IsPurchasable;
            if (empty.IsPurchasableAsUpgrade != IsPurchasableAsUpgrade) result.IsPurchasableAsUpgrade = IsPurchasableAsUpgrade;
            if (empty.ThemeRequired != ThemeRequired) result.ThemeRequired = ThemeRequired;
            if (empty.ShoppingTags != ShoppingTags) result.ShoppingTags = ShoppingTags;
            if (empty.RarityTier != RarityTier) result.RarityTier = RarityTier;
            if (empty.PriceTier != PriceTier) result.PriceTier = PriceTier;
            if (empty.ShopRequirementFilter != ShopRequirementFilter) result.ShopRequirementFilter = ShopRequirementFilter;
            if (empty.RequiresForShop != RequiresForShop) result.RequiresForShop = RequiresForShop;
            if (empty.RequiresProcessForShop != RequiresProcessForShop) result.RequiresProcessForShop = RequiresProcessForShop;
            if (empty.StapleWhenMissing != StapleWhenMissing) result.StapleWhenMissing = StapleWhenMissing;
            if (empty.SellOnlyAsDuplicate != SellOnlyAsDuplicate) result.SellOnlyAsDuplicate = SellOnlyAsDuplicate;
            if (empty.PreventSale != PreventSale) result.PreventSale = PreventSale;
            if (empty.Upgrades != Upgrades) result.Upgrades = Upgrades;
            if (empty.IsAnUpgrade != IsAnUpgrade) result.IsAnUpgrade = IsAnUpgrade;
            if (empty.IsNonCrated != IsNonCrated) result.IsNonCrated = IsNonCrated;
            if (empty.CrateItem != CrateItem) result.CrateItem = CrateItem;
            if (empty.Name != Name) result.Name = Name;
            if (empty.Description != Description) result.Description = Description;
            if (empty.Sections != Sections) result.Sections = Sections;
            if (empty.Tags != Tags) result.Tags = Tags;

            result.Info = new LocalisationObject<ApplianceInfo>();
            gameDataObject = result;
        }
    }
}
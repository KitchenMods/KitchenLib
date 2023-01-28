using KitchenData;
using System.Collections.Generic;
using UnityEngine;
using Kitchen;
using System.Linq;

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
        public virtual bool PreventSale { get; protected set; }
        public virtual List<Appliance> Upgrades { get; protected set; } = new List<Appliance>();
        public virtual bool IsAnUpgrade { get; protected set; }
        public virtual bool IsNonCrated { get; protected set; }
        public virtual Item CrateItem { get; protected set; }
        public virtual string Name { get; protected set; } = "Appliance";
        public virtual string Description { get; protected set; } = "A little something for your restaurant";
        public virtual List<Appliance.Section> Sections { get; protected set; } = new List<Appliance.Section>();
        public virtual List<string> Tags { get; protected set; } = new List<string>();

        public virtual bool ForceIsRotationPossible() { return false; }
        public virtual bool IsRotationPossible(InteractionData data) { return true; }
        public virtual bool PreRotate(InteractionData data, bool isSecondary = false) { return false; }
        public virtual void PostRotate(InteractionData data) { }
        public virtual bool ForceIsInteractionPossible() { return false; }
        public virtual bool IsInteractionPossible(InteractionData data) { return true; }
        public virtual bool PreInteract(InteractionData data, bool isSecondary = false) { return false; }
        public virtual void PostInteract(InteractionData data) { }

        private static readonly Appliance empty = ScriptableObject.CreateInstance<Appliance>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Appliance result = ScriptableObject.CreateInstance<Appliance>();

            if (BaseGameDataObjectID != -1)
                result = Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));
            else
                result = Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == AssetReference.Counter));

            if (empty.ID != ID) result.ID = ID;
            if (empty.Prefab != Prefab) result.Prefab = Prefab;
            if (empty.HeldAppliancePrefab != HeldAppliancePrefab) result.HeldAppliancePrefab = HeldAppliancePrefab;
            if (empty.EffectRange != EffectRange) result.EffectRange = EffectRange;
            if (empty.EffectCondition != EffectCondition) result.EffectCondition = EffectCondition;
            if (empty.EffectType != EffectType) result.EffectType = EffectType;
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
            if (empty.StapleWhenMissing != StapleWhenMissing) result.StapleWhenMissing = StapleWhenMissing;
            if (empty.SellOnlyAsDuplicate != SellOnlyAsDuplicate) result.SellOnlyAsDuplicate = SellOnlyAsDuplicate;
            if (empty.PreventSale != PreventSale) result.PreventSale = PreventSale;
            if (empty.IsAnUpgrade != IsAnUpgrade) result.IsAnUpgrade = IsAnUpgrade;
            if (empty.IsNonCrated != IsNonCrated) result.IsNonCrated = IsNonCrated;
            if (empty.Name != Name) result.Name = Name;
            if (empty.Description != Description) result.Description = Description;
            if (empty.Sections != Sections) result.Sections = Sections;
            if (empty.Tags != Tags) result.Tags = Tags;
            if (empty.Info != Info) result.Info = Info;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Appliance result = (Appliance)gameDataObject;

            if (empty.Processes != Processes) result.Processes = Processes;
            if (empty.Properties != Properties) result.Properties = Properties;
            if (empty.EffectRepresentation != EffectRepresentation) result.EffectRepresentation = EffectRepresentation;
            if (empty.RequiresForShop != RequiresForShop) result.RequiresForShop = RequiresForShop;
            if (empty.RequiresProcessForShop != RequiresProcessForShop) result.RequiresProcessForShop = RequiresProcessForShop;
            if (empty.Upgrades != Upgrades) result.Upgrades = Upgrades;
            if (empty.CrateItem != CrateItem) result.CrateItem = CrateItem;
        }
    }
}
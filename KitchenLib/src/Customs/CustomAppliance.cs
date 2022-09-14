using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomAppliance
	{
        /*
         * Fields from Kitchen.Appliance
         */
        //Fields with default values
        public virtual string Description { get { return "A little something for your restaurant"; } } //Default: "A little something for your restaurant"
        public virtual EntryAnimation EntryAnimation { get { return EntryAnimation.Placement; } } //Default: EntryAnimation.Placement
        public virtual ExitAnimation ExitAnimation { get { return ExitAnimation.Destroy; } } //Default: ExitAnimation.Destroy
        public virtual bool ForceHighInteractionPriority { get { return false; } } //Default: false
        public virtual bool IsAnUpgrade { get { return false; } } //Default: false
        public virtual bool IsNonCrated { get { return false; } } //Default: false
        public virtual bool IsNonInteractive { get { return false; } } //Default: false
        public virtual bool IsPurchasable { get { return false; } } //Default: false
        public virtual bool IsPurchasableAsUpgrade { get { return false; } } //Default: false
        public virtual OccupancyLayer Layer { get; internal set; } //Default: Default
        public virtual string Name { get { return "Appliance"; } } //Default: "Appliance"
        public virtual bool PreventSale { get { return false; } } //Default: false
        public virtual int PurchaseCost { get; internal set; } //Default: 0
        public virtual RarityTier RarityTier { get { return RarityTier.Common; } } //Default: RarityTier.Common
        public virtual List<Appliance> RequiresForShop { get { return new List<Appliance>(); } } //Default: List<Appliance> (Empty)
        public virtual List<Process> RequiresProcessForShop { get { return new List<Process>(); } } //Default: List<Process> (Empty)
        public virtual bool SellOnlyAsDuplicate { get { return false; } } //Default: false
        public virtual ShoppingTags ShoppingTags { get { return ShoppingTags.None; } } //Default: ShoppingTags.None
        public virtual ShopRequirementFilter ShopRequirementFilter { get { return ShopRequirementFilter.None; } } //Default: ShopRequirementFilter.None
        public virtual bool SkipRotationAnimation { get { return false; } } //Default: false
        public virtual DecorationType ThemeRequired { get { return DecorationType.Null; } } //Default: DecorationType.Null
        public virtual List<Appliance> Upgrades { get { return new List<Appliance>(); } }  //Default: List<Appliance> (Empty)
        public virtual bool StapleWhenMissing { get { return false; } } //Default: false
        public virtual PriceTier PriceTier { get { return PriceTier.Medium; } } //Default: PriceTier.Medium

        //Fields without default values
        public virtual Item CrateItem { get; internal set; }
        public virtual IEffectCondition EffectCondition { get; internal set; }
        public virtual IEffectRange EffectRange { get; internal set; }
        public virtual EffectRepresentation EffectRepresentation { get; internal set; }
        public virtual IEffectType EffectType { get; internal set; }
        public virtual GameObject HeldAppliancePrefab { get; internal set; }
        public virtual int ID { get; internal set; }
        public virtual GameObject Prefab { get; internal set; }
        public virtual List<Appliance.ApplianceProcesses> Processes { get; internal set; }
        public virtual List<IApplianceProperty> Properties { get; internal set; }
        public virtual List<Appliance.Section> Sections { get; internal set; }
        public virtual List<string> Tags { get; internal set; }

        /*
         * Custom Fields
         */

        public string ModName { get; internal set; }
        public virtual int BaseApplianceId { get { return -1; } }
		public virtual int BasePrefabId { get { return BaseApplianceId; } }

		public Appliance Appliance { get; internal set; }

		//Overridable methods
        
		public virtual void OnRegister(Appliance appliance) { }


		public virtual bool ForceIsRotationPossible() { return false; }
		public virtual bool IsRotationPossible(InteractionData data) { return true; }
		public virtual bool PreRotate(InteractionData data, bool isSecondary = false) { return false; }
		public virtual void PostRotate(InteractionData data) { }
		public virtual bool ForceIsInteractionPossible() { return false; }
		public virtual bool IsInteractionPossible(InteractionData data) { return true; }
		public virtual bool PreInteract(InteractionData data, bool isSecondary = false) { return false; }
		public virtual void PostInteract(InteractionData data) { }
		public int GetHash() {
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}

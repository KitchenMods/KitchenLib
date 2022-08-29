using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Appliances
{
	public abstract class CustomAppliance
	{
		//All appliance fields that can be overriden (Other than Info)
		public virtual Item CrateItem { get { return null; } }
		public virtual string Description { get { return "The real unsung hero of any kitchen"; } }
		public virtual IEffectCondition EffectCondition { get { return null; } }
		public virtual IEffectRange EffectRange { get { return null; } }
		public virtual EffectRepresentation EffectRepresentation { get { return null; } }
		public virtual IEffectType EffectType { get { return null; } }
		public virtual EntryAnimation EntryAnimation { get { return EntryAnimation.Placement; } }
		public virtual ExitAnimation ExitAnimation { get { return ExitAnimation.Destroy; } }
		public virtual bool ForceHighInteractionPriority { get { return false; } }
		public virtual GameObject HeldAppliancePrefab { get { return null; } }
		public virtual int ID { get; internal set; }
		public virtual bool IsAnUpgrade { get { return false; } }
        public virtual bool IsNonCrated { get { return false; } }
        public virtual bool IsNonInteractive { get { return false; } }
        public virtual bool IsPurchasable { get { return true; } }
		public virtual bool IsPurchasableAsUpgrade { get { return false; } }
		public virtual OccupancyLayer Layer { get { return OccupancyLayer.Default; } }
		public virtual string Name { get { return "Counter"; } }
		public virtual GameObject Prefab { get { return null; } }
		public virtual bool PreventSale { get { return false; } }
		public virtual List<Appliance.ApplianceProcesses> Processes { get { return null; } }
		public virtual List<IApplianceProperty> Properties { get { return null; } }
		public virtual int PurchaseCost { get { return 20; } }
		public virtual RarityTier RarityTier { get { return RarityTier.Common; } }
        public virtual List<Appliance> RequiresForShop { get { return null; } }
        public virtual List<Process> RequiresProcessForShop { get { return null; } }
        public virtual List<Appliance.Section> Sections { get { return null; } }
		public virtual bool SellOnlyAsDuplicate { get { return false; } }
        public virtual ShoppingTags ShoppingTags { get { return ShoppingTags.Basic; } }
		public virtual ShopRequirementFilter ShopRequirementFilter { get { return ShopRequirementFilter.None; } }
		public virtual bool SkipRotationAnimation { get { return false; } }
        public virtual List<string> Tags { get { return null; } }
		public virtual DecorationType ThemeRequired { get { return DecorationType.Null; } }
        public virtual List<Appliance> Upgrades { get { return null; } }

        //Custom appliance fields

        public string ModName { get; internal set; }
        public virtual int BaseApplianceId { get { return -1; } }
		public virtual int BasePrefabId { get { return -1; } }

		public Appliance Appliance { get; internal set; }

		//Overridable methods
        
		public virtual void OnRegister(Appliance appliance) { }

		public virtual void OnInteract(InteractionData data) { }

		public virtual bool OnCheckInteractPossible(InteractionData data) {
			return false;
		}

		public int GetHash() {
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}

using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace KitchenLib.Customs
{
	public abstract class CustomAppliance : CustomLocalisedGameDataObject<Appliance, ApplianceInfo>, ICustomHasPrefab
    {
	    // Base-Game Variables
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
        public virtual List<MenuPhase> RequiresPhaseForShop { get; protected set; } = new List<MenuPhase>();
        public virtual bool StapleWhenMissing { get; protected set; }
        public virtual bool SellOnlyAsDuplicate { get; protected set; }
        public virtual bool SellOnlyAsUnique { get; protected set; }
        public virtual bool PreventSale { get; protected set; }
        public virtual List<Appliance> Upgrades { get; protected set; } = new List<Appliance>();
        public virtual List<Appliance> Enchantments { get; protected set; } = new List<Appliance>();

        [Obsolete("Should not be used by the user")]
        public virtual bool IsAnUpgrade { get; protected set; }
        public virtual bool IsNonCrated { get; protected set; }
        public virtual Item CrateItem { get; protected set; }
        
        // KitchenLib Variables
		public virtual bool AutoGenerateNavMeshObject { get; protected set; } = true;

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

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
			Appliance result = ScriptableObject.CreateInstance<Appliance>();

            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Prefab", Prefab);
            OverrideVariable(result, "HeldAppliancePrefab", HeldAppliancePrefab);
            OverrideVariable(result, "EffectRange", EffectRange);
            OverrideVariable(result, "EffectCondition", EffectCondition);
            OverrideVariable(result, "EffectType", EffectType);
            OverrideVariable(result, "IsNonInteractive", IsNonInteractive);
            OverrideVariable(result, "Layer", Layer);
            OverrideVariable(result, "ForceHighInteractionPriority", ForceHighInteractionPriority);
            OverrideVariable(result, "EntryAnimation", EntryAnimation);
            OverrideVariable(result, "ExitAnimation", ExitAnimation);
            OverrideVariable(result, "SkipRotationAnimation", SkipRotationAnimation);
            OverrideVariable(result, "IsPurchasable", IsPurchasable);
            OverrideVariable(result, "IsPurchasableAsUpgrade", IsPurchasableAsUpgrade);
            OverrideVariable(result, "ThemeRequired", ThemeRequired);
            OverrideVariable(result, "ShoppingTags", ShoppingTags);
            OverrideVariable(result, "RarityTier", RarityTier);
            OverrideVariable(result, "PriceTier", PriceTier);
            OverrideVariable(result, "ShopRequirementFilter", ShopRequirementFilter);
            OverrideVariable(result, "RequiresPhaseForShop", RequiresPhaseForShop);
            OverrideVariable(result, "StapleWhenMissing", StapleWhenMissing);
            OverrideVariable(result, "SellOnlyAsDuplicate", SellOnlyAsDuplicate);
            OverrideVariable(result, "SellOnlyAsUnique", SellOnlyAsUnique);
            OverrideVariable(result, "PreventSale", PreventSale);
            OverrideVariable(result, "IsNonCrated", IsNonCrated);
            OverrideVariable(result, "Info", Info);

            if (PurchaseCostOverride != -1)
            {
	            Main.LogDebug($"Assigning : {PurchaseCostOverride} >> PurchaseCostOverride");
	            ApplianceOverrides.AddPurchaseCostOverride(result.ID, PurchaseCostOverride);
            }

            if (InfoList.Count > 0)
            {
	            Main.LogDebug($"Setting up localisation");
	            SetupLocalisation<ApplianceInfo>(InfoList, ref result.Info);
            }
            else
            {
	            if (result.Info == null)
	            {
		            Main.LogDebug($"Setting up fallback localisation");
		            result.Info = new LocalisationObject<ApplianceInfo>();
		            if (!result.Info.Has(Locale.English))
		            {
			            ApplianceInfo applianceInfo = ScriptableObject.CreateInstance<ApplianceInfo>();
			            applianceInfo.Name = Name;
			            applianceInfo.Description = Description;
			            applianceInfo.Sections = Sections;
			            applianceInfo.Tags = Tags;
			            result.Info.Add(Locale.English, applianceInfo);
		            }
	            }
            }

            if (AutoGenerateNavMeshObject && result.Prefab != null)
            {
	            Main.LogDebug($"Setting up NavMeshObstacle");
	            if (result.Prefab.GetComponentsInChildren<NavMeshObstacle>().Length == 0)
	            {
		            Appliance counter = gameData.Get<Appliance>().FirstOrDefault(a => a.ID == ApplianceReferences.Countertop);
		            foreach (Transform t in counter.Prefab.GetComponentInChildren<Transform>())
		            {
			            if (t.gameObject.HasComponent<NavMeshObstacle>())
			            {
				            GameObjectUtils.CopyComponent(t.gameObject.GetComponent<NavMeshObstacle>(), result.Prefab);
				            break;
			            }
		            }
	            }
            }

			gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Appliance result = (Appliance)gameDataObject;
            
            OverrideVariable(result, "Processes", Processes);
            OverrideVariable(result, "Properties", Properties);
            OverrideVariable(result, "EffectRepresentation", EffectRepresentation);
            OverrideVariable(result, "RequiresForShop", RequiresForShop);
            OverrideVariable(result, "RequiresProcessForShop", RequiresProcessForShop);
            OverrideVariable(result, "Upgrades", Upgrades);
            OverrideVariable(result, "Enchantments", Enchantments);
            OverrideVariable(result, "CrateItem", CrateItem);

            if (result.Prefab == null)
            {
	            Main.LogError($"Assigning fallback prefab");
	            result.Prefab = Main.bundle.LoadAsset<GameObject>("Error_Appliance");
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
using System;
using System.Linq;
using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenLib.Appliances
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	class GameDataConstructor_Patch
	{
		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex(__result);

			var prefabHostObject = new UnityEngine.GameObject();
			prefabHostObject.name = "Custom Appliance Prefab Host";
			prefabHostObject.SetActive(false);

			List<GameDataObject> gameDataObjects = new List<GameDataObject>();

			foreach (CustomAppliance appliance in CustomAppliances.Appliances.Values)
			{
				Appliance newAppliance = createApp(__result, appliance);
				appliance.OnRegister(newAppliance);
				appliance.Appliance = newAppliance;
				gameDataObjects.Add(newAppliance);
			}

			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				try
				{
					gameDataObject.SetupForGame();
					gameDataObject.Localise(Localisation.CurrentLocale, __result.Substitutions);
					GlobalLocalisation globalLocalisation = gameDataObject as GlobalLocalisation;
					if (globalLocalisation != null)
					{
						__result.GlobalLocalisation = globalLocalisation;
					}
				}
				catch (Exception e) { }
			}

			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				__result.Objects.Add(gameDataObject.ID, gameDataObject);
				IHasPrefab hasPrefab = gameDataObject as IHasPrefab;
				if (hasPrefab != null)
				{
					__result.Prefabs.Add(gameDataObject.ID, hasPrefab.Prefab);
				}
			}
			
			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				gameDataObject.SetupFinal();
			}
			
			__result.Dispose();
			__result.InitialiseViews();
		}

		private static Appliance createApp(GameData gameData, CustomAppliance customAppliance)
        {
            Appliance result;
            Appliance empty = new Appliance();
			if (customAppliance.BaseApplianceId != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == customAppliance.BaseApplianceId));
            else
                result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == AssetReference.Counter));
            /*
			 * If value is overriden and changed from default, apply override value
			 */
            if (customAppliance.Description != empty.Description) result.Description = customAppliance.Description;
            if (customAppliance.EntryAnimation != empty.EntryAnimation) result.EntryAnimation = customAppliance.EntryAnimation;
            if (customAppliance.ExitAnimation != empty.ExitAnimation) result.ExitAnimation = customAppliance.ExitAnimation;
            if (customAppliance.ForceHighInteractionPriority != empty.ForceHighInteractionPriority) result.ForceHighInteractionPriority = customAppliance.ForceHighInteractionPriority;
            if (customAppliance.IsAnUpgrade != empty.IsAnUpgrade) result.IsAnUpgrade = customAppliance.IsAnUpgrade;
            if (customAppliance.IsNonCrated != empty.IsNonCrated) result.IsNonCrated = customAppliance.IsNonCrated;
            if (customAppliance.IsNonInteractive != empty.IsNonInteractive) result.IsNonInteractive = customAppliance.IsNonInteractive;
            if (customAppliance.IsPurchasable != empty.IsPurchasable) result.IsPurchasable = customAppliance.IsPurchasable;
            if (customAppliance.IsPurchasableAsUpgrade != empty.IsPurchasableAsUpgrade) result.IsPurchasableAsUpgrade = customAppliance.IsPurchasableAsUpgrade;
            if (customAppliance.Layer != empty.Layer) result.Layer = customAppliance.Layer;
            if (customAppliance.Name != empty.Name) result.Name = customAppliance.Name;
            if (customAppliance.PreventSale != empty.PreventSale) result.PreventSale = customAppliance.PreventSale;
            if (customAppliance.PurchaseCost != empty.PurchaseCost) result.PurchaseCost = customAppliance.PurchaseCost;
            if (customAppliance.RarityTier != empty.RarityTier) result.RarityTier = customAppliance.RarityTier;
            if (customAppliance.RequiresForShop != empty.RequiresForShop) result.RequiresForShop = customAppliance.RequiresForShop;
            if (customAppliance.RequiresProcessForShop != empty.RequiresProcessForShop) result.RequiresProcessForShop = customAppliance.RequiresProcessForShop;
            if (customAppliance.SellOnlyAsDuplicate != empty.SellOnlyAsDuplicate) result.SellOnlyAsDuplicate = customAppliance.SellOnlyAsDuplicate;
            if (customAppliance.ShoppingTags != empty.ShoppingTags) result.ShoppingTags = customAppliance.ShoppingTags;
            if (customAppliance.ShopRequirementFilter != empty.ShopRequirementFilter) result.ShopRequirementFilter = customAppliance.ShopRequirementFilter;
            if (customAppliance.SkipRotationAnimation != empty.SkipRotationAnimation) result.SkipRotationAnimation = customAppliance.SkipRotationAnimation;
            if (customAppliance.ThemeRequired != empty.ThemeRequired) result.ThemeRequired = customAppliance.ThemeRequired;
            if (customAppliance.Upgrades != empty.Upgrades) result.Upgrades = customAppliance.Upgrades;
            if (customAppliance.StapleWhenMissing != empty.StapleWhenMissing) result.StapleWhenMissing = customAppliance.StapleWhenMissing;
            if (customAppliance.PriceTier != empty.PriceTier) result.PriceTier = customAppliance.PriceTier;
            /*
			 * If null value is overriden and changed from default, apply override value
			 */
            if (customAppliance.CrateItem != null) result.CrateItem = customAppliance.CrateItem;
            if (customAppliance.EffectCondition != null) result.EffectCondition = customAppliance.EffectCondition;
            if (customAppliance.EffectRange != null) result.EffectRange = customAppliance.EffectRange;
            if (customAppliance.EffectRepresentation != null) result.EffectRepresentation = customAppliance.EffectRepresentation;
            if (customAppliance.EffectType != null) result.EffectType = customAppliance.EffectType;
            if (customAppliance.HeldAppliancePrefab != null) result.HeldAppliancePrefab = customAppliance.HeldAppliancePrefab;
            if (customAppliance.Prefab != null) result.Prefab = customAppliance.Prefab;
            if (customAppliance.Processes != null) result.Processes = customAppliance.Processes;
            if (customAppliance.Properties != null) result.Properties = customAppliance.Properties;
            if (customAppliance.Sections != null) result.Sections = customAppliance.Sections;
            if (customAppliance.Tags != null) result.Tags = customAppliance.Tags;

            result.ID = customAppliance.ID;
            result.Info = new LocalisationObject<ApplianceInfo>();
            result.name = $"{result.Name}(Clone)";

            if (result.Prefab == null)
                result.Prefab = gameData.Get<Appliance>().FirstOrDefault(a => a.ID == customAppliance.BasePrefabId).Prefab;

            return result;
		}
	}
}

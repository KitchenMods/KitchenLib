using Kitchen;
using KitchenData;
using System;
using System.Linq;
using HarmonyLib;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using KitchenLib.Event;
using KitchenLib.Reference;


namespace KitchenLib.Customs
{
    public static class CreateCustomGDOs
    {
        public static Appliance CreateAppliance(GameData gameData, CustomAppliance customAppliance)
        {
            Appliance result = default(Appliance);
            Appliance empty = default(Appliance);

            if (customAppliance.BaseApplianceId != -1) //Setting the base appliance
                result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == customAppliance.BaseApplianceId));
            else
                result = UnityEngine.Object.Instantiate(gameData.Get<Appliance>().FirstOrDefault(a => a.ID == ApplianceReference.Counter));
            
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

		public static Item CreateItem(GameData gameData, CustomItem customItem)
		{
			Item result = default(Item);
			Item empty = default(Item);
			if (customItem.BaseItemId != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == customItem.BaseItemId));
			else
				result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == 681117884));

			if (customItem.Prefab != empty.Prefab) result.Prefab = customItem.Prefab;
			//if (customItem.Processes != empty.Processes) result.Processes = customItem.Processes;
			if (customItem.DerivedProcesses != empty.DerivedProcesses) result.DerivedProcesses = customItem.DerivedProcesses;
			if (customItem.Properties != empty.Properties) result.Properties = customItem.Properties;
			if (customItem.ExtraTimeGranted != empty.ExtraTimeGranted) result.ExtraTimeGranted = customItem.ExtraTimeGranted;
			if (customItem.ItemValue != empty.ItemValue) result.ItemValue = customItem.ItemValue;
			if (customItem.Reward != empty.Reward) result.Reward = customItem.Reward;
			if (customItem.DirtiesTo != empty.DirtiesTo) result.DirtiesTo = customItem.DirtiesTo;
			if (customItem.MayRequestExtraItems != empty.MayRequestExtraItems) result.MayRequestExtraItems = customItem.MayRequestExtraItems;
			if (customItem.MaxOrderSharers != empty.MaxOrderSharers) result.MaxOrderSharers = customItem.MaxOrderSharers;
			if (customItem.SplitSubItem != empty.SplitSubItem) result.SplitSubItem = customItem.SplitSubItem;
			if (customItem.SplitCount != empty.SplitCount) result.SplitCount = customItem.SplitCount;
			if (customItem.SplitSpeed != empty.SplitSpeed) result.SplitSpeed = customItem.SplitSpeed;
			if (customItem.SplitDepletedItems != empty.SplitDepletedItems) result.SplitDepletedItems = customItem.SplitDepletedItems;
			if (customItem.AllowSplitMerging != empty.AllowSplitMerging) result.AllowSplitMerging = customItem.AllowSplitMerging;
			if (customItem.PreventExplicitSplit != empty.PreventExplicitSplit) result.PreventExplicitSplit = customItem.PreventExplicitSplit;
			if (customItem.SplitByComponents != empty.SplitByComponents) result.SplitByComponents = customItem.SplitByComponents;
			if (customItem.SplitByComponentsHolder != empty.SplitByComponentsHolder) result.SplitByComponentsHolder = customItem.SplitByComponentsHolder;
			if (customItem.SplitByCopying != empty.SplitByCopying) result.SplitByCopying = customItem.SplitByCopying;
			if (customItem.RefuseSplitWith != empty.RefuseSplitWith) result.RefuseSplitWith = customItem.RefuseSplitWith;
			if (customItem.DisposesTo != empty.DisposesTo) result.DisposesTo = customItem.DisposesTo;
			if (customItem.IsIndisposable != empty.IsIndisposable) result.IsIndisposable = customItem.IsIndisposable;
			if (customItem.ItemCategory != empty.ItemCategory) result.ItemCategory = customItem.ItemCategory;
			if (customItem.ItemStorageFlags != empty.ItemStorageFlags) result.ItemStorageFlags = customItem.ItemStorageFlags;
			if (customItem.DedicatedProvider != empty.DedicatedProvider) result.DedicatedProvider = customItem.DedicatedProvider;
			if (customItem.HoldPose != empty.HoldPose) result.HoldPose = customItem.HoldPose;
			if (customItem.IsMergeableSide != empty.IsMergeableSide) result.IsMergeableSide = customItem.IsMergeableSide;

			FieldInfo fi = typeof(Item).GetField("Processes", BindingFlags.NonPublic | BindingFlags.Instance);
			fi.SetValue(result, customItem.DerivedProcesses);

			result.ID = customItem.ID;
			result.name = $"{result.Prefab.name}(Clone)";

			if (result.Prefab == null)
				result.Prefab = gameData.Get<Appliance>().FirstOrDefault(a => a.ID == customItem.BasePrefabId).Prefab;
			
			return result;
		}

		public static Process CreateProcess(GameData gameData, CustomProcess customProcess)
		{
			Process result = default(Process);
			if (customProcess.BaseProcessId != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Process>().FirstOrDefault(a => a.ID == customProcess.BaseProcessId));
			
			if (customProcess.BasicEnablingAppliance != null) result.BasicEnablingAppliance = customProcess.BasicEnablingAppliance;
			if (customProcess.IsPseudoprocessFor != null) result.IsPseudoprocessFor = customProcess.IsPseudoprocessFor;
			result.EnablingApplianceCount = customProcess.EnablingApplianceCount;
			
			result.CanObfuscateProgress = customProcess.CanObfuscateProgress;
			result.Icon = customProcess.Icon;

			result.ID = customProcess.ID;
            result.Info = new LocalisationObject<ProcessInfo>();
			
			return result;
		}

        public static Contract CreateContract(GameData gameData, CustomContract customContract)
        {
			Contract result = default(Contract);
			Contract empty = default(Contract);

            if (customContract.BaseContractId != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Contract>().FirstOrDefault(a => a.ID == customContract.BaseContractId));

            if (customContract.Name != empty.Name) result.Name = customContract.Name;
            if (customContract.Description != empty.Description) result.Description = customContract.Description;
            if (customContract.Status != empty.Status) result.Status = customContract.Status;
            if (customContract.ExperienceMultiplier != empty.ExperienceMultiplier) result.ExperienceMultiplier = customContract.ExperienceMultiplier;

            result.ID = customContract.ID;
            result.Info = new LocalisationObject<ContractInfo>();
			
			return result;
        }

        public static Decor CreateDecor(GameData gameData, CustomDecor customDecor)
        {
			Decor result = default(Decor);
			Decor empty = default(Decor);

            if (customDecor.BaseDecorId != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Decor>().FirstOrDefault(a => a.ID == customDecor.BaseDecorId));

            if (customDecor.Material != empty.Material) result.Material = customDecor.Material;
            if (customDecor.ApplicatorAppliance != empty.ApplicatorAppliance) result.ApplicatorAppliance = customDecor.ApplicatorAppliance;
            if (customDecor.Type != empty.Type) result.Type = customDecor.Type;
            if (customDecor.IsAvailable != empty.IsAvailable) result.IsAvailable = customDecor.IsAvailable;

            result.ID = customDecor.ID;
			
			return result;
        }

        public static Dish CreateDish(GameData gameData, CustomDish customDish)
        {
            Dish result = default(Dish);
            Dish empty = default(Dish);

            if (customDish.BaseDishId != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Dish>().FirstOrDefault(a => a.ID == customDish.BaseDishId));

            if (customDish.Type != empty.Type) result.Type = customDish.Type;
            if (customDish.AchievementName != empty.AchievementName) result.AchievementName = customDish.AchievementName;
            if (customDish.UnlocksMenuItems != empty.UnlocksMenuItems) result.UnlocksMenuItems = customDish.UnlocksMenuItems;
            if (customDish.UnlocksIngredients != empty.UnlocksIngredients) result.UnlocksIngredients = customDish.UnlocksIngredients;
            if (customDish.ExtraOrderUnlocks != empty.ExtraOrderUnlocks) result.ExtraOrderUnlocks = customDish.ExtraOrderUnlocks;
            if (customDish.StartingNameSet != empty.StartingNameSet) result.StartingNameSet = customDish.StartingNameSet;
            if (customDish.MinimumIngredients != empty.MinimumIngredients) result.MinimumIngredients = customDish.MinimumIngredients;
            if (customDish.RequiredProcesses != empty.RequiredProcesses) result.RequiredProcesses = customDish.RequiredProcesses;
            if (customDish.BlockProviders != empty.BlockProviders) result.BlockProviders = customDish.BlockProviders;
            if (customDish.PrerequisiteDishes != empty.PrerequisiteDishes) result.PrerequisiteDishes = customDish.PrerequisiteDishes;
            if (customDish.IconPrefab != empty.IconPrefab) result.IconPrefab = customDish.IconPrefab;
            if (customDish.DisplayPrefab != empty.DisplayPrefab) result.DisplayPrefab = customDish.DisplayPrefab;

            return result;   
        }

        public static Effect CreateEffect(GameData gameData, CustomEffect customEffect)
        {
            Effect result = default(Effect);
            Effect empty = default(Effect);

            if (customEffect.BaseEffectId != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Effect>().FirstOrDefault(a => a.ID == customEffect.BaseEffectId));

            if (customEffect.Properties != empty.Properties) result.Properties = customEffect.Properties;
            if (customEffect.EffectRange != empty.EffectRange) result.EffectRange = customEffect.EffectRange;
            if (customEffect.EffectCondition != empty.EffectCondition) result.EffectCondition = customEffect.EffectCondition;
            if (customEffect.EffectType != empty.EffectType) result.EffectType = customEffect.EffectType;
            if (customEffect.EffectInformation != empty.EffectInformation) result.EffectInformation = customEffect.EffectInformation;

            return result;
        }

        public static EffectRepresentation CreateEffectRepresentation(GameData gameData, CustomEffectRepresentation custom)
        {
            EffectRepresentation result = default(EffectRepresentation);
            EffectRepresentation empty = default(EffectRepresentation);

            if (custom.BaseEffectRepresentationId != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<EffectRepresentation>().FirstOrDefault(a => a.ID == custom.BaseEffectRepresentationId));

            if (custom.Name != empty.Name) result.Name = custom.Name;
            if (custom.Description != empty.Description) result.Description = custom.Description;
            if (custom.Icon != empty.Icon) result.Icon = custom.Icon;

            return result;
        }

        public static FranchiseUpgrade CreateFranchiseUpgrade(GameData gameData, CustomFranchiseUpgrade custom)
        {
            FranchiseUpgrade result = default(FranchiseUpgrade);
            FranchiseUpgrade empty = default(FranchiseUpgrade);

            if (custom.BaseFranchiseUpgradeId != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<FranchiseUpgrade>().FirstOrDefault(a => a.ID == custom.BaseFranchiseUpgradeId));

            if (custom.Name != empty.Name) result.Name = custom.Name;
            if (custom.MaximumUpgradeCount != empty.MaximumUpgradeCount) result.MaximumUpgradeCount = custom.MaximumUpgradeCount;
            if (custom.Upgrades != empty.Upgrades) result.Upgrades = custom.Upgrades;

            return result;
        }

        public static GameDifficultySettings CreateGameDifficultySettings(GameData gameData, CustomGameDifficultySettings custom)
        {
            GameDifficultySettings result = default(GameDifficultySettings);
            GameDifficultySettings empty = default(GameDifficultySettings);

            if (custom.BaseGameDifficultySettingsId != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<GameDifficultySettings>().FirstOrDefault(a => a.ID == custom.BaseGameDifficultySettingsId));

            if (custom.IsActive != empty.IsActive) result.IsActive = custom.IsActive;
            if (custom.CustomersPerHourBase != empty.CustomersPerHourBase) result.CustomersPerHourBase = custom.CustomersPerHourBase;
            if (custom.CustomersPerHourIncreasePerDay != empty.CustomersPerHourIncreasePerDay) result.CustomersPerHourIncreasePerDay = custom.CustomersPerHourIncreasePerDay;
            if (custom.CustomerSideChance != empty.CustomerSideChance) result.CustomerSideChance = custom.CustomerSideChance;
            if (custom.QueuePatienceTime != empty.QueuePatienceTime) result.QueuePatienceTime = custom.QueuePatienceTime;
            if (custom.QueuePatienceBoost != empty.QueuePatienceBoost) result.QueuePatienceBoost = custom.QueuePatienceBoost;
            if (custom.CustomerStarterChance != empty.CustomerStarterChance) result.CustomerStarterChance = custom.CustomerStarterChance;
            if (custom.GroupDessertChance != empty.GroupDessertChance) result.GroupDessertChance = custom.GroupDessertChance;

            return result;
        }
    }
}
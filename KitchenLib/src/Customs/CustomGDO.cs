using System;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public class CustomGDO
	{
		public static Dictionary<int, CustomAppliance> Appliances = new Dictionary<int, CustomAppliance>();
		private static Dictionary<Type, CustomAppliance> appliancesByType = new Dictionary<Type, CustomAppliance>();

		public static Dictionary<int, CustomItem> Items = new Dictionary<int, CustomItem>();
		private static Dictionary<Type, CustomItem> itemsByType = new Dictionary<Type, CustomItem>();

		public static Dictionary<string, CustomItemProcess> ItemProcesses = new Dictionary<string, CustomItemProcess>();
		public static Dictionary<string, CustomApplianceProcess> ApplianceProcesses = new Dictionary<string, CustomApplianceProcess>();

		public static Dictionary<int, CustomProcess> Processes = new Dictionary<int, CustomProcess>();
		public static Dictionary<Type, CustomProcess> processesByType = new Dictionary<Type, CustomProcess>();

		public static Dictionary<int, CustomContract> Contracts = new Dictionary<int, CustomContract>();
		public static Dictionary<Type, CustomContract> contractsByType = new Dictionary<Type, CustomContract>();

		public static Dictionary<int, CustomDecor> Decors = new Dictionary<int, CustomDecor>();
		public static Dictionary<Type, CustomDecor> decorsByType = new Dictionary<Type, CustomDecor>();

		public static Dictionary<int, CustomDish> Dishes = new Dictionary<int, CustomDish>();
		public static Dictionary<Type, CustomDish> dishesByType = new Dictionary<Type, CustomDish>();

		public static Dictionary<int, CustomEffect> Effects = new Dictionary<int, CustomEffect>();
		public static Dictionary<Type, CustomEffect> effectsByType = new Dictionary<Type, CustomEffect>();

		public static Dictionary<int, CustomEffectRepresentation> EffectRepresentations = new Dictionary<int, CustomEffectRepresentation>();
		public static Dictionary<Type, CustomEffectRepresentation> effectRepresentationsByType = new Dictionary<Type, CustomEffectRepresentation>();

		public static Dictionary<int, CustomFranchiseUpgrade> FranchiseUpgrades = new Dictionary<int, CustomFranchiseUpgrade>();
		public static Dictionary<Type, CustomFranchiseUpgrade> franchiseUpgradesByType = new Dictionary<Type, CustomFranchiseUpgrade>();

		public static Dictionary<int, CustomGameDifficultySettings> GameDifficultySettings = new Dictionary<int, CustomGameDifficultySettings>();
		public static Dictionary<Type, CustomGameDifficultySettings> gameDifficultySettingsByType = new Dictionary<Type, CustomGameDifficultySettings>();

		public static Dictionary<int, CustomGardenProfile> GardenProfiles = new Dictionary<int, CustomGardenProfile>();
		public static Dictionary<Type, CustomGardenProfile> gardenProfilesByType = new Dictionary<Type, CustomGardenProfile>();

		public static Dictionary<int, CustomItemGroup> ItemGroups = new Dictionary<int, CustomItemGroup>();
		public static Dictionary<Type, CustomItemGroup> itemGroupsByType = new Dictionary<Type, CustomItemGroup>();

		public static Dictionary<int, CustomLayoutProfile> LayoutProfiles = new Dictionary<int, CustomLayoutProfile>();
		public static Dictionary<Type, CustomLayoutProfile> layoutProfilesByType = new Dictionary<Type, CustomLayoutProfile>();

		public static Dictionary<int, CustomLevelUpgradeSet> LevelUpgradeSets = new Dictionary<int, CustomLevelUpgradeSet>();
		public static Dictionary<Type, CustomLevelUpgradeSet> levelUpgradeSetsByType = new Dictionary<Type, CustomLevelUpgradeSet>();


		public static T RegisterItemProcess<T>(T process) where T : CustomItemProcess
		{
			if (ItemProcesses.ContainsKey(process.ProcessName)) {
				return null;
			}

			ItemProcesses.Add(process.ProcessName, process);
			return process;
		}

		public static T RegisterApplianceProcess<T>(T process) where T : CustomApplianceProcess
		{
			if (ApplianceProcesses.ContainsKey(process.ProcessName)) {
				return null;
			}

			ApplianceProcesses.Add(process.ProcessName, process);
			return process;
		}

		public static T RegisterProcess<T>(T process) where T : CustomProcess
		{
			if (process.ID == 0)
				process.ID = process.GetHash();
			Processes.Add(process.ID, process);
			processesByType.Add(process.GetType(), process);
			return process;
		}

		public static T RegisterAppliance<T>(T appliance) where T : CustomAppliance
		{
			if (appliance.ID == 0)
				appliance.ID = appliance.GetHash();

			if (Appliances.ContainsKey(appliance.ID))
			{
				Mod.Error("Appliance: " + appliance.Name + " failed to register - key:" + appliance.ID + " already in use. Generating custom key. " + appliance.GetHash());
				return null;
			}

			Appliances.Add(appliance.ID, appliance);
			appliancesByType.Add(appliance.GetType(), appliance);
			Mod.Log($"Registered appliance '{appliance.ModName}:{appliance.Name}' as {appliance.ID}");
			return appliance;
		}
		public static T RegisterItem<T>(T item) where T : CustomItem
		{
			if (item.ID == 0)
				item.ID = item.GetHash();

			if (Items.ContainsKey(item.ID))
			{
				Mod.Error("Item: " + item.Name + " failed to register - key:" + item.ID + " already in use. Generating custom key. " + item.GetHash());
				return null;
			}
			
			Items.Add(item.ID, item);
			itemsByType.Add(item.GetType(), item);
			Mod.Log($"Registered item '{item.ModName}:{item.Name}' as {item.ID}");
			return item;
		}

		public static T RegisterContract<T>(T contract) where T : CustomContract
		{
			if (contract.ID == 0)
				contract.ID = contract.GetHash();
			
			if (Contracts.ContainsKey(contract.ID))
			{
				Mod.Error("Contract: " + contract.Name + " failed to register - key:" + contract.ID + " already in use. Generating custom key. " + contract.GetHash());
				return null;
			}

			Contracts.Add(contract.ID, contract);
			contractsByType.Add(contract.GetType(), contract);
			Mod.Log($"Registered contract '{contract.ModName}:{contract.Name}' as {contract.ID}");
			return contract;
		}

		public static T RegisterDecor<T>(T decor) where T : CustomDecor
		{
			if (decor.ID == 0)
				decor.ID = decor.GetHash();
			
			if (Decors.ContainsKey(decor.ID))
			{
				Mod.Error("Decor: " + decor.Name + " failed to register - key:" + decor.ID + " already in use. Generating custom key. " + decor.GetHash());
				return null;
			}

			Decors.Add(decor.ID, decor);
			decorsByType.Add(decor.GetType(), decor);
			Mod.Log($"Registered contract '{decor.ModName}:{decor.Name}' as {decor.ID}");
			return decor;
		}

		public static T RegisterDish<T>(T dish) where T : CustomDish
		{
			if (dish.ID == 0)
				dish.ID = dish.GetHash();
			
			if (Dishes.ContainsKey(dish.ID))
			{
				Mod.Error("Dish: " + dish.Name + " failed to register - key:" + dish.ID + " already in use. Generating custom key. " + dish.GetHash());
				return null;
			}

			Dishes.Add(dish.ID, dish);
			dishesByType.Add(dish.GetType(), dish);
			Mod.Log($"Registered dish '{dish.ModName}:{dish.Name}' as {dish.ID}");
			return dish;
		}

		public static T RegisterEffect<T>(T effect) where T : CustomEffect
		{
			if (effect.ID == 0)
				effect.ID = effect.GetHash();
			
			if (Effects.ContainsKey(effect.ID))
			{
				Mod.Error("Effect: " + effect.Name + " failed to register - key:" + effect.ID + " already in use. Generating custom key. " + effect.GetHash());
				return null;
			}

			Effects.Add(effect.ID, effect);
			effectsByType.Add(effect.GetType(), effect);
			Mod.Log($"Registered effect '{effect.ModName}:{effect.Name}' as {effect.ID}");
			return effect;
		}

		public static T RegisterEffectRepresentation<T>(T effectRepresentation) where T : CustomEffectRepresentation
		{
			if (effectRepresentation.ID == 0)
				effectRepresentation.ID = effectRepresentation.GetHash();
			
			if (EffectRepresentations.ContainsKey(effectRepresentation.ID))
			{
				Mod.Error("EffectRepresentation: " + effectRepresentation.Name + " failed to register - key:" + effectRepresentation.ID + " already in use. Generating custom key. " + effectRepresentation.GetHash());
				return null;
			}

			EffectRepresentations.Add(effectRepresentation.ID, effectRepresentation);
			effectRepresentationsByType.Add(effectRepresentation.GetType(), effectRepresentation);
			Mod.Log($"Registered effect representation '{effectRepresentation.ModName}:{effectRepresentation.Name}' as {effectRepresentation.ID}");
			return effectRepresentation;
		}

		public static T RegisterFranchiseUpgrade<T>(T franchiseUpgrade) where T : CustomFranchiseUpgrade
		{
			if (franchiseUpgrade.ID == 0)
				franchiseUpgrade.ID = franchiseUpgrade.GetHash();
			
			if (FranchiseUpgrades.ContainsKey(franchiseUpgrade.ID))
			{
				Mod.Error("FranchiseUpgrade: " + franchiseUpgrade.Name + " failed to register - key:" + franchiseUpgrade.ID + " already in use. Generating custom key. " + franchiseUpgrade.GetHash());
				return null;
			}

			FranchiseUpgrades.Add(franchiseUpgrade.ID, franchiseUpgrade);
			franchiseUpgradesByType.Add(franchiseUpgrade.GetType(), franchiseUpgrade);
			Mod.Log($"Registered franchise upgrade '{franchiseUpgrade.ModName}:{franchiseUpgrade.Name}' as {franchiseUpgrade.ID}");
			return franchiseUpgrade;
		}

		public static T RegisterGameDifficultySettings <T>(T gameDifficultySettings) where T : CustomGameDifficultySettings
		{
			if (gameDifficultySettings.ID == 0)
				gameDifficultySettings.ID = gameDifficultySettings.GetHash();
			
			if (GameDifficultySettings.ContainsKey(gameDifficultySettings.ID))
			{
				Mod.Error("GameDifficultySettings: " + gameDifficultySettings.Name + " failed to register - key:" + gameDifficultySettings.ID + " already in use. Generating custom key. " + gameDifficultySettings.GetHash());
				return null;
			}

			GameDifficultySettings.Add(gameDifficultySettings.ID, gameDifficultySettings);
			gameDifficultySettingsByType.Add(gameDifficultySettings.GetType(), gameDifficultySettings);
			Mod.Log($"Registered game difficulty settings '{gameDifficultySettings.ModName}:{gameDifficultySettings.Name}' as {gameDifficultySettings.ID}");
			return gameDifficultySettings;
		}

		public static T RegisterGardenProfile<T>(T gardenProfile) where T : CustomGardenProfile
		{
			if (gardenProfile.ID == 0)
				gardenProfile.ID = gardenProfile.GetHash();
			
			if (GardenProfiles.ContainsKey(gardenProfile.ID))
			{
				Mod.Error("GardenProfile: " + gardenProfile.Name + " failed to register - key:" + gardenProfile.ID + " already in use. Generating custom key. " + gardenProfile.GetHash());
				return null;
			}

			GardenProfiles.Add(gardenProfile.ID, gardenProfile);
			gardenProfilesByType.Add(gardenProfile.GetType(), gardenProfile);
			Mod.Log($"Registered garden profile '{gardenProfile.ModName}:{gardenProfile.Name}' as {gardenProfile.ID}");
			return gardenProfile;
		}

		public static T RegisterItemGroup<T>(T itemGroup) where T : CustomItemGroup
		{
			if (itemGroup.ID == 0)
				itemGroup.ID = itemGroup.GetHash();
			
			if (ItemGroups.ContainsKey(itemGroup.ID))
			{
				Mod.Error("ItemGroup: " + itemGroup.Name + " failed to register - key:" + itemGroup.ID + " already in use. Generating custom key. " + itemGroup.GetHash());
				return null;
			}

			ItemGroups.Add(itemGroup.ID, itemGroup);
			itemGroupsByType.Add(itemGroup.GetType(), itemGroup);
			Mod.Log($"Registered item group '{itemGroup.ModName}:{itemGroup.Name}' as {itemGroup.ID}");
			return itemGroup;
		}

		public static T RegisterLayoutProfile<T>(T layoutProfile) where T : CustomLayoutProfile
		{
			if (layoutProfile.ID == 0)
				layoutProfile.ID = layoutProfile.GetHash();
			
			if (LayoutProfiles.ContainsKey(layoutProfile.ID))
			{
				Mod.Error("LayoutProfile: " + layoutProfile.Name + " failed to register - key:" + layoutProfile.ID + " already in use. Generating custom key. " + layoutProfile.GetHash());
				return null;
			}

			LayoutProfiles.Add(layoutProfile.ID, layoutProfile);
			layoutProfilesByType.Add(layoutProfile.GetType(), layoutProfile);
			Mod.Log($"Registered layout profile '{layoutProfile.ModName}:{layoutProfile.Name}' as {layoutProfile.ID}");
			return layoutProfile;
		}

		public static T RegisterLevelUpgradeSet<T>(T levelUpgradeSet) where T : CustomLevelUpgradeSet
		{
			if (levelUpgradeSet.ID == 0)
				levelUpgradeSet.ID = levelUpgradeSet.GetHash();
			
			if (LevelUpgradeSets.ContainsKey(levelUpgradeSet.ID))
			{
				Mod.Error("LevelUpgradeSet: " + levelUpgradeSet.Name + " failed to register - key:" + levelUpgradeSet.ID + " already in use. Generating custom key. " + levelUpgradeSet.GetHash());
				return null;
			}

			LevelUpgradeSets.Add(levelUpgradeSet.ID, levelUpgradeSet);
			levelUpgradeSetsByType.Add(levelUpgradeSet.GetType(), levelUpgradeSet);
			Mod.Log($"Registered level upgrade set '{levelUpgradeSet.ModName}:{levelUpgradeSet.Name}' as {levelUpgradeSet.ID}");
			return levelUpgradeSet;
		}

		//Get Custom Appliance
		public static CustomAppliance GetCustomAppliance(int id)
		{
			Appliances.TryGetValue(id, out var result);
			return result;
		}

		public static CustomAppliance GetCustomAppliance<T>()
		{
			appliancesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Item
		public static CustomItem GetCustomItem(int id)
		{
			Items.TryGetValue(id, out var result);
			return result;
		}

		public static CustomItem GetCustomItem<T>()
		{
			itemsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Process
		public static CustomProcess GetCustomProcess(int id)
		{
			Processes.TryGetValue(id, out var result);
			return result;
		}
		public static CustomProcess GetCustomProcess<T>()
		{
			processesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Item Process

		public static CustomItemProcess GetCustomItemProcess(string name)
		{
			ItemProcesses.TryGetValue(name, out var result);
			return result;
		}

		public static CustomItemProcess GetCustomItemProcess<T>()
		{
			ItemProcesses.TryGetValue(typeof(T).Name, out var result);
			return result;
		}

		//Get Custom Appliance Process

		public static CustomApplianceProcess GetCustomApplianceProcess(string name)
		{
			ApplianceProcesses.TryGetValue(name, out var result);
			return result;
		}

		public static CustomApplianceProcess GetCustomApplianceProcess<T>()
		{
			ApplianceProcesses.TryGetValue(typeof(T).Name, out var result);
			return result;
		}

		//Get Custom Contract
		public static CustomContract GetCustomContract(int id)
		{
			Contracts.TryGetValue(id, out var result);
			return result;
		}
		public static CustomContract GetCustomContract<T>()
		{
			contractsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Decor
		public static CustomDecor GetCustomDecor(int id)
		{
			Decors.TryGetValue(id, out var result);
			return result;
		}
		public static CustomDecor GetCustomDecor<T>()
		{
			decorsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Dish
		public static CustomDish GetCustomDish(int id)
		{
			Dishes.TryGetValue(id, out var result);
			return result;
		}
		public static CustomDish GetCustomDish<T>()
		{
			dishesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Effect
		public static CustomEffect GetCustomEffect(int id)
		{
			Effects.TryGetValue(id, out var result);
			return result;
		}
		public static CustomEffect GetCustomEffect<T>()
		{
			effectsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom EffectRepresentation
		public static CustomEffectRepresentation GetCustomEffectRepresentation(int id)
		{
			EffectRepresentations.TryGetValue(id, out var result);
			return result;
		}
		public static CustomEffectRepresentation GetCustomEffectRepresentation<T>()
		{
			effectRepresentationsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom FranchiseUpgrade
		public static CustomFranchiseUpgrade GetCustomFranchiseUpgrade(int id)
		{
			FranchiseUpgrades.TryGetValue(id, out var result);
			return result;
		}
		public static CustomFranchiseUpgrade GetCustomFranchiseUpgrade<T>()
		{
			franchiseUpgradesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom GameDifficultySettings
		public static CustomGameDifficultySettings GetCustomGameDifficultySettings(int id)
		{
			GameDifficultySettings.TryGetValue(id, out var result);
			return result;
		}
		public static CustomGameDifficultySettings GetCustomGameDifficultySettings<T>()
		{
			gameDifficultySettingsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom GardenProfile
		public static CustomGardenProfile GetCustomGardenProfile(int id)
		{
			GardenProfiles.TryGetValue(id, out var result);
			return result;
		}
		public static CustomGardenProfile GetCustomGardenProfile<T>()
		{
			gardenProfilesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom ItemGroup
		public static CustomItemGroup GetCustomItemGroup(int id)
		{
			ItemGroups.TryGetValue(id, out var result);
			return result;
		}
		public static CustomItemGroup GetCustomItemGroup<T>()
		{
			itemGroupsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom LayoutProfile
		public static CustomLayoutProfile GetCustomLayoutProfile(int id)
		{
			LayoutProfiles.TryGetValue(id, out var result);
			return result;
		}
		public static CustomLayoutProfile GetCustomLayoutProfile<T>()
		{
			layoutProfilesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom LevelUpgradeSet
		public static CustomLevelUpgradeSet GetCustomLevelUpgradeSet(int id)
		{
			LevelUpgradeSets.TryGetValue(id, out var result);
			return result;
		}
		public static CustomLevelUpgradeSet GetCustomLevelUpgradeSet<T>()
		{
			levelUpgradeSetsByType.TryGetValue(typeof(T), out var result);
			return result;
		}
	}
}

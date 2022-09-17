using System;
using System.Linq;
using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	public class GameDataConstructor_Patch
	{

		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex(__result);

			GDOUtils.SetupGDOIndex(__result);			

			var prefabHostObject = new UnityEngine.GameObject();
			prefabHostObject.name = "Custom Appliance Prefab Host";
			prefabHostObject.SetActive(false);

			List<GameDataObject> gameDataObjects = new List<GameDataObject>();


			foreach (CustomProcess process in CustomGDO.Processes.Values) //Adds Custom Process to GDOs
			{
				Process newProcess = createProcess(__result, process);
				process.Process = newProcess;
				process.OnRegister(newProcess);
				if (process.ProcessAudioClip != null)
					AudioUtils.AddProcessAudioClip(newProcess.ID, process.ProcessAudioClip);
				gameDataObjects.Add(newProcess);
			}

			foreach (CustomApplianceProcess applianceProcess in CustomGDO.ApplianceProcesses.Values) //Adds Custom Appliance Process to GDOUtils
			{
				Appliance.ApplianceProcesses newApplianceProcess = createApplianceProcess(__result, applianceProcess);
				applianceProcess.OnRegister(newApplianceProcess);
				GDOUtils.AddCustomApplianceProcess(applianceProcess.ProcessName, newApplianceProcess);
			}

			foreach (CustomItemProcess itemProcess in CustomGDO.ItemProcesses.Values) //Adds Custom Item Process to GDOUtils
			{
				Item.ItemProcess newItemProcess = createItemProcess(__result, itemProcess);
				itemProcess.OnRegister(newItemProcess);
				GDOUtils.AddCustomItemProcess(itemProcess.ProcessName, newItemProcess);
			}

			foreach (CustomAppliance appliance in CustomGDO.Appliances.Values) //Adds Custom Appliances to GDOs
			{
				Appliance newAppliance = createApp(__result, appliance);
				appliance.OnRegister(newAppliance);
				appliance.Appliance = newAppliance;
				gameDataObjects.Add(newAppliance);
			}

			foreach (CustomItem item in CustomGDO.Items.Values) //Adds Custom Items to GDOs
			{
				Item newItem= createItem(__result, item);
				item.OnRegister(newItem);
				item.Item = newItem;
				gameDataObjects.Add(newItem);
			}
			
			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				try
				{
					gameDataObject.SetupForGame();
					GlobalLocalisation globalLocalisation = gameDataObject as GlobalLocalisation;
					if (globalLocalisation != null)
					{
						__result.GlobalLocalisation = globalLocalisation;
					}
				}
				catch (Exception e)
				{
					Mod.Log(e.Message);
				}
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

		private static Item.ItemProcess createItemProcess(GameData gameData, CustomItemProcess customItemProcess)
		{
			Item.ItemProcess result = new Item.ItemProcess();
			result.Process = customItemProcess.Process;
			result.Result = customItemProcess.Result;
			result.Duration = customItemProcess.Duration;
			result.IsBad = customItemProcess.IsBad;
			result.RequiresWrapper = customItemProcess.RequiresWrapper;
			return result;
		}

		private static Appliance.ApplianceProcesses createApplianceProcess(GameData gameData, CustomApplianceProcess customApplianceProcess)
		{
			Appliance.ApplianceProcesses result = new Appliance.ApplianceProcesses();
			result.Process = customApplianceProcess.Process;
			result.IsAutomatic = customApplianceProcess.IsAutomatic;
			result.Speed = customApplianceProcess.Speed;
			return result;
		}

		private static Process createProcess(GameData gameData, CustomProcess customProcess)
		{
			Process result = new Process();
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

		public static Item createItem(GameData gameData, CustomItem customItem)
		{
			Item result;
			Item empty = new Item();
			if (customItem.BaseItemId != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == customItem.BaseItemId));
			else
				result = UnityEngine.Object.Instantiate(gameData.Get<Item>().FirstOrDefault(a => a.ID == 681117884));

			if (customItem.Prefab != empty.Prefab) result.Prefab = customItem.Prefab;
			//if (customItem.Processes != empty.Processes) result.Processes = customItem.Processes;
			//if (customItem.DerivedProcesses != empty.DerivedProcesses) result.DerivedProcesses = customItem.DerivedProcesses;
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
	}
}

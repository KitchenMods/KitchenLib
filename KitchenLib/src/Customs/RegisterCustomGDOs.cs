using System;
using System.Linq;
using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using KitchenLib.Event;

namespace KitchenLib.Customs
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	public class GameDataConstructor_Patch
	{

		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex(__result);

			GDOUtils.SetupGDOIndex(__result);			

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataEvent), Events.BuildGameDataEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result));

			List<GameDataObject> gameDataObjects = new List<GameDataObject>();

			foreach (CustomProcess process in CustomGDO.Processes.Values) //Adds Custom Process to GDOs
			{
				Process newProcess = CreateCustomGDOs.CreateProcess(__result, process);
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
				Appliance newAppliance = CreateCustomGDOs.CreateAppliance(__result, appliance);
				appliance.OnRegister(newAppliance);
				appliance.Appliance = newAppliance;
				gameDataObjects.Add(newAppliance);
			}

			foreach (CustomItem item in CustomGDO.Items.Values) //Adds Custom Items to GDOs
			{
				Item newItem = CreateCustomGDOs.CreateItem(__result, item);
				item.OnRegister(newItem);
				item.Item = newItem;
				gameDataObjects.Add(newItem);
			}

			foreach (CustomContract contract in CustomGDO.Contracts.Values) //Adds Custom Contracts to GDOs
			{
				Contract newContract = CreateCustomGDOs.CreateContract(__result, contract);
				contract.OnRegister(newContract);
				contract.Contract = newContract;
				gameDataObjects.Add(newContract);
			}

			foreach (CustomDecor decor in CustomGDO.Decors.Values) //Adds Custom Decors to GDOs
			{
				Decor newDecor = CreateCustomGDOs.CreateDecor(__result, decor);
				decor.OnRegister(newDecor);
				decor.Decor = newDecor;
				gameDataObjects.Add(newDecor);
			}

			foreach (CustomDish dish in CustomGDO.Dishes.Values) //Adds Custom Dishes to GDOs
			{
				Dish newDish = CreateCustomGDOs.CreateDish(__result, dish);
				dish.OnRegister(newDish);
				dish.Dish = newDish;
				gameDataObjects.Add(newDish);
			}

			foreach (CustomEffect effect in CustomGDO.Effects.Values) //Adds Custom Effects to GDOs
			{
				Effect newEffect = CreateCustomGDOs.CreateEffect(__result, effect);
				effect.OnRegister(newEffect);
				effect.Effect = newEffect;
				gameDataObjects.Add(newEffect);
			}

			foreach (CustomEffectRepresentation effectRepresentation in CustomGDO.EffectRepresentations.Values) //Adds Custom Effect Representations to GDOs
			{
				EffectRepresentation newEffectRepresentation = CreateCustomGDOs.CreateEffectRepresentation(__result, effectRepresentation);
				effectRepresentation.OnRegister(newEffectRepresentation);
				effectRepresentation.EffectRepresentation = newEffectRepresentation;
				gameDataObjects.Add(newEffectRepresentation);
			}

			foreach (CustomFranchiseUpgrade franchiseUpgrade in CustomGDO.FranchiseUpgrades.Values) //Adds Custom Franchise Upgrades to GDOs
			{
				FranchiseUpgrade newFranchiseUpgrade = CreateCustomGDOs.CreateFranchiseUpgrade(__result, franchiseUpgrade);
				franchiseUpgrade.OnRegister(newFranchiseUpgrade);
				franchiseUpgrade.FranchiseUpgrade = newFranchiseUpgrade;
				gameDataObjects.Add(newFranchiseUpgrade);
			}

			foreach (CustomGameDifficultySettings gameDifficultySettings in CustomGDO.GameDifficultySettings.Values) //Adds Custom Game Difficulty Settings to GDOs
			{
				GameDifficultySettings newGameDifficultySettings = CreateCustomGDOs.CreateGameDifficultySettings(__result, gameDifficultySettings);
				gameDifficultySettings.OnRegister(newGameDifficultySettings);
				gameDifficultySettings.GameDifficultySettings = newGameDifficultySettings;
				gameDataObjects.Add(newGameDifficultySettings);
			}

			foreach (CustomGardenProfile gardenProfile in CustomGDO.GardenProfiles.Values) //Adds Custom Garden Profiles to GDOs
			{
				GardenProfile newGardenProfile = CreateCustomGDOs.CreateGardenProfile(__result, gardenProfile);
				gardenProfile.OnRegister(newGardenProfile);
				gardenProfile.GardenProfile = newGardenProfile;
				gameDataObjects.Add(newGardenProfile);
			}

			foreach (CustomItemGroup itemGroup in CustomGDO.ItemGroups.Values) //Adds Custom Item Groups to GDOs
			{
				ItemGroup newItemGroup = CreateCustomGDOs.CreateItemGroup(__result, itemGroup);
				itemGroup.OnRegister(newItemGroup);
				itemGroup.ItemGroup = newItemGroup;
				gameDataObjects.Add(newItemGroup);
			}

			foreach (CustomLayoutProfile layoutProfile in CustomGDO.LayoutProfiles.Values) //Adds Custom Layout Profiles to GDOs
			{
				LayoutProfile newLayoutProfile = CreateCustomGDOs.CreateLayoutProfile(__result, layoutProfile);
				layoutProfile.OnRegister(newLayoutProfile);
				layoutProfile.LayoutProfile = newLayoutProfile;
				gameDataObjects.Add(newLayoutProfile);
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
	}
}

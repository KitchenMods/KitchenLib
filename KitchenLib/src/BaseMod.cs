using System;
using System.Runtime.CompilerServices;
using MelonLoader;
using Kitchen;
using KitchenLib.Registry;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;
using Semver;

namespace KitchenLib
{
	public abstract class BaseMod : MelonMod
	{
		public string ModName { get { return Info.Name; } }
		public string ModVersion { get { return Info.Version; } }
        
		public string ModID;
		public string CompatibleVersions;
		public string[] ModDependencies;

		public static KitchenVersion version = new KitchenVersion(Application.version);
		public static SemVersion semVersion = new SemVersion(version.Major, version.Minor, version.Patch);
        
            
        public BaseMod(string modID, string compatibleVersions, string[] modDependencies = null) : base() {
            
            ModID = modID; CompatibleVersions = compatibleVersions; ModDependencies = modDependencies;
            ModRegistery.Register(this);
        }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Log(string message) {
			MelonLogger.Msg(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Error(string message) {
			MelonLogger.Error(message);
		}

		[Obsolete("Use the AddAppliance method instead")]
		public T RegisterCustomAppliance<T>() where T : CustomAppliance, new()  {
			return AddAppliance<T>();
		}

		public T AddAppliance<T>() where T : CustomAppliance, new()
		{
			T appliance = new T();
			appliance.ModName = Info.Name;
			return CustomGDO.RegisterAppliance(appliance);
		}

		public T AddItem<T>() where T : CustomItem, new()
		{
			T item = new T();
			item.ModName = Info.Name;
			return CustomGDO.RegisterItem(item);
		}

		public T AddContract<T>() where T : CustomContract, new()
		{
			T contract = new T();
			contract.ModName = Info.Name;
			return CustomGDO.RegisterContract(contract);
		}

		public T AddDecor<T>() where T : CustomDecor, new()
		{
			T decor = new T();
			decor.ModName = Info.Name;
			return CustomGDO.RegisterDecor(decor);
		}

		public T AddDish<T>() where T : CustomDish, new()
		{
			T dish = new T();
			dish.ModName = Info.Name;
			return CustomGDO.RegisterDish(dish);
		}

		public T AddEffect<T>() where T : CustomEffect, new()
		{
			T effect = new T();
			effect.ModName = Info.Name;
			return CustomGDO.RegisterEffect(effect);
		}

		public T AddEffectRepresentation<T>() where T : CustomEffectRepresentation, new()
		{
			T effectRepresentation = new T();
			effectRepresentation.ModName = Info.Name;
			return CustomGDO.RegisterEffectRepresentation(effectRepresentation);
		}

		public T AddFranchiseUpgrade<T>() where T : CustomFranchiseUpgrade, new()
		{
			T franchiseUpgrade = new T();
			franchiseUpgrade.ModName = Info.Name;
			return CustomGDO.RegisterFranchiseUpgrade(franchiseUpgrade);
		}

		public T AddGameDifficultySettings<T>() where T : CustomGameDifficultySettings, new()
		{
			T gameDifficultySettings = new T();
			gameDifficultySettings.ModName = Info.Name;
			return CustomGDO.RegisterGameDifficultySettings(gameDifficultySettings);
		}

		public T AddGardenProfile<T>() where T : CustomGardenProfile, new()
		{
			T gardenProfile = new T();
			gardenProfile.ModName = Info.Name;
			return CustomGDO.RegisterGardenProfile(gardenProfile);
		}

		public T AddItemGroup<T>() where T : CustomItemGroup, new()
		{
			T itemGroup = new T();
			itemGroup.ModName = Info.Name;
			return CustomGDO.RegisterItemGroup(itemGroup);
		}

		public T AddLayoutProfile<T>() where T : CustomLayoutProfile, new()
		{
			T layoutProfile = new T();
			layoutProfile.ModName = Info.Name;
			return CustomGDO.RegisterLayoutProfile(layoutProfile);
		}








		public T AddItemProcess<T>() where T : CustomItemProcess, new()
		{
			T itemProcess = new T();
			return CustomGDO.RegisterItemProcess(itemProcess);
		}

		public T AddApplianceProcess<T>() where T : CustomApplianceProcess, new()
		{
			T applianceProcess = new T();
			return CustomGDO.RegisterApplianceProcess(applianceProcess);
		}

		public T AddProcess<T>() where T : CustomProcess, new()
		{
			T process = new T();
			return CustomGDO.RegisterProcess(process);
		}

		public T AddPreference<T>(string modID, string key, string name) where T : BasePreference, new()
		{
			T preference = new T();
			return PreferenceUtils.Register<T>(modID, key, name);
		}

		[Obsolete("System registration is now automatic")]
		public T AddSystem<T>() where T : GenericSystemBase, new() {
			return SystemUtils.AddSystem<T>();
		}
	}
}

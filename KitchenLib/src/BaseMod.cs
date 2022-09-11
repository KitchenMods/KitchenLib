using System;
using System.Runtime.CompilerServices;
using MelonLoader;
using Kitchen;
using KitchenLib.Registry;
using KitchenLib.Appliances;
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

		public T AddItemProcess<T>() where T : CustomItemProcess, new()
		{
			T itemProcess = new T();
			return CustomGDO.RegisterItemProcess(itemProcess);
		}

		public T AddSystem<T>() where T : GenericSystemBase, new() {
			return SystemUtils.AddSystem<T>();
		}
	}
}

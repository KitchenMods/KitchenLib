using System;
using System.Runtime.CompilerServices;
using Kitchen;
using KitchenLib.Registry;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;
using Semver;
using System.Reflection;
using HarmonyLib;
#if MelonLoader
using MelonLoader;
#endif
#if BepInEx
using BepInEx;
using BepInEx.Logging;
#endif

#if MelonLoader
[assembly: MelonInfo(typeof(KitchenLib.Mod), "KitchenLib", "0.1.8", "KitchenMods")]
[assembly: MelonGame("It's Happening", "PlateUp")]
[assembly: MelonPriority(-1000000)]
[assembly: MelonColor(System.ConsoleColor.Green)]
#endif
namespace KitchenLib
{
	#if BepInEx
	[BepInProcess("PlateUp.exe")]
	#endif
	public abstract class BaseMod : LoaderMod
	{
		#if MelonLoader
		public string ModName { get { return Info.Name; } }
		public string ModVersion { get { return Info.Version; } }
		#endif
		#if BepInEx
		public string ModName { get { return this.Info.Metadata.Name; } }
		public string ModVersion { get { return this.Info.Metadata.Version.ToString(); } }
		private static ManualLogSource logger;
		#endif
		public string ModID;

		public string CompatibleVersions;
		public string[] ModDependencies;

		public static KitchenVersion version = new KitchenVersion(Application.version);
		public static SemVersion semVersion = new SemVersion(version.Major, version.Minor, version.Patch);
        
		#if MelonLoader
        public BaseMod(string modID, string compatibleVersions, string[] modDependencies = null) : base() {
		   ModID = modID; CompatibleVersions = compatibleVersions; ModDependencies = modDependencies;
		   ModRegistery.Register(this);
        }
		#endif
		#if BepInEx
        public BaseMod(string compatibleVersions, Assembly assem, string[] modDependencies = null) : base() {
			ModID = this.Info.Metadata.GUID;
			HarmonyLib.Harmony.CreateAndPatchAll(assem, ModID);
			CompatibleVersions = compatibleVersions;
            logger = Logger;
			Mod.Log("HELLO " + ModID + " " + assem.FullName);
            ModRegistery.Register(this);
        }
		#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Log(string message) {
			#if BepInEx
			logger.Log(LogLevel.All, message);
			#endif
			#if MelonLoader
			MelonLogger.Msg(message);
			#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Error(string message) {
			#if BepInEx
			logger.Log(LogLevel.Error, message);
			#endif
			#if MelonLoader
			MelonLogger.Error(message);
			#endif
		}

		public T AddGameDataObject<T>() where T : CustomGameDataObject, new()
		{
			T gdo = new T();
			gdo.ModName = ModName;
			return CustomGDO.RegisterGameDataObject(gdo);
		}

		public T AddSubProcess<T>() where T : CustomSubProcess, new()
		{
			T subProcess = new T();
			return CustomSubProcess.RegisterSubProcess(subProcess);
		}

		public T AddPreference<T>(string modID, string key, string name) where T : BasePreference, new()
		{
			T preference = new T();
			return PreferenceUtils.Register<T>(modID, key, name);
		}


		/*
		 * Obsolete Methods
		 */

		[Obsolete("Use the AddAppliance method instead")]
		public T RegisterCustomAppliance<T>() where T : CustomAppliance, new()  {
			return AddAppliance<T>();
		}

		[Obsolete("System registration is now automatic")]
		public T AddSystem<T>() where T : GenericSystemBase, new() {
			return SystemUtils.AddSystem<T>();
		}

		[Obsolete("Use the AddGameDataObject method instead")]
		public T AddAppliance<T>() where T : CustomAppliance, new()
		{
			return AddGameDataObject<T>();
		}

		[Obsolete("Use the AddGameDataObject method instead")]
		public T AddItem<T>() where T : CustomItem, new()
		{
			return AddGameDataObject<T>();
		}

		[Obsolete("Use the AddGameDataObject method instead")]
		public T AddProcess<T>() where T : CustomProcess, new()
		{
			return AddGameDataObject<T>();
		}
	}
}

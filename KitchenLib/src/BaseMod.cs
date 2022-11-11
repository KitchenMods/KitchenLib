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
#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
using BepInEx.Logging;
#endif

namespace KitchenLib
{
	public abstract class BaseMod : LoaderMod
	{
		#if MELONLOADER
		public string ModName { get { return Info.Name; } }
		public string ModVersion { get { return Info.Version; } }
		#endif
		#if BEPINEX
		public string ModName { get { return this.Info.Metadata.Name; } }
		public string ModVersion { get { return this.Info.Metadata.Version.ToString(); } }
		private static ManualLogSource logger;
		#endif
		public string ModID;

		public string CompatibleVersions;
		public string[] ModDependencies;

		public static KitchenVersion version = new KitchenVersion(Application.version);
		public static SemVersion semVersion = new SemVersion(version.Major, version.Minor, version.Patch);
        
        public BaseMod(string modID, string compatibleVersions, string[] modDependencies = null) : base() {
#if MELONLOADER
			ModID = modID; CompatibleVersions = compatibleVersions; ModDependencies = modDependencies;
			ModRegistery.Register(this);
#endif
        }
        public BaseMod(string compatibleVersions, Assembly assem, string[] modDependencies = null) : base() {
#if BEPINEX
            logger = Logger;
			ModID = this.Info.Metadata.GUID;
			HarmonyLib.Harmony.CreateAndPatchAll(assem, ModID);
			CompatibleVersions = compatibleVersions;
            ModRegistery.Register(this);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Log(string message) {
#if BEPINEX
			logger.Log(LogLevel.All, message);
#endif
#if MELONLOADER
			MelonLogger.Msg(message);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Error(string message) {
#if BEPINEX
			logger.Log(LogLevel.Error, message);
#endif
#if MELONLOADER
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

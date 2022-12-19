#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
using BepInEx.Logging;
#endif

using System.Reflection;
using Semver;
using UnityEngine;
using KitchenLib.Utils;
using System.Runtime.CompilerServices;
using HarmonyLib;
using KitchenLib.Registry;
using static MelonLoader.MelonLogger;
using KitchenLib.Customs;
using System;
using System.Collections.Generic;
using KitchenMods;
using Kitchen;

namespace KitchenLib
{

	public abstract class BaseMod : LoaderMod
	{
		public string ModID = "";
		public string ModName = "";
		public string ModVersion = "";
		public string CompatibleVersions = "";

		public static KitchenVersion version;
		public static SemVersion semVersion;

		public static BaseMod instance;
		private static List<Assembly> PatchedAssemblies = new List<Assembly>();
		private bool isRegistered = false;
		
#if BEPINEX || WORKSHOP
		public static HarmonyLib.Harmony harmonyInstance;
#endif
		public BaseMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly) : base()
		{
			SetupMod(modID, modName, author, modVersion, compatibleVersions, assembly);
		}

		[Obsolete("Please use BaseMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly)")]
		public BaseMod(string modID, string compatibleVersions, string[] modDependencies = null) : base()
		{
			SetupMod(modID, "Unsupported Name", "Unsupported Author", "0.0.0", compatibleVersions, null);
		}

		[Obsolete("Please use BaseMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly)")]
		public BaseMod(string compatibleVersions, Assembly assembly, string[] modDependencies = null) : base()
		{
			SetupMod("unsupportedmodid", "Unsupported Name", "Unsupported Author", "0.0.0", compatibleVersions, assembly);
		}

		[Obsolete("Please use BaseMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly)")]
		public BaseMod(string modID, string modVersion, string compatibleVersions, Assembly assembly) : base()
		{
			SetupMod(modID, "Unsupported Name", "Unsupported Author", modVersion, compatibleVersions, assembly);
		}

		private void SetupMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly)
		{
			
			instance = this;
			ModID = modID;
			ModName = modName;
			ModVersion = modVersion;
			CompatibleVersions = compatibleVersions;

			if (!Debug.isDebugBuild)
				version = new KitchenVersion(Application.version);
			else
				version = new KitchenVersion("");

			
#if BEPINEX || WORKSHOP
			if (harmonyInstance == null)
				harmonyInstance = new HarmonyLib.Harmony(modID);
			if (!PatchedAssemblies.Contains(assembly))
			{
				if (assembly != null)
				{
					harmonyInstance.PatchAll(assembly);
					PatchedAssemblies.Add(assembly);
				}
			}
#endif
			

			semVersion = new SemVersion(version.Major, version.Minor, version.Patch);
			isRegistered = ModRegistery.Register(this);
			
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(string message)
		{
#if BEPINEX
			Logger.Log(LogLevel.Info, message);
#endif
#if MELONLOADER
			MelonLogger.Msg(message);
#endif
#if WORKSHOP
			Debug.Log(message);
#endif
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(string message)
		{
#if BEPINEX
			Logger.Log(LogLevel.Error, message);
#endif
#if MELONLOADER
			MelonLogger.Error(message);
#endif
#if WORKSHOP
			Debug.LogError(message);
#endif
		}
		protected virtual void OnInitialise() { }
		protected virtual void OnFrameUpdate() { }

#if BEPINEX
		void Update()
		{
			if (isRegistered)
				OnFrameUpdate();
		}

		void Awake()
		{
			if (isRegistered)
				OnInitialise();
		}
#endif

#if MELONLOADER
		public override void OnUpdate()
		{
			if (isRegistered)
				OnFrameUpdate();
		}

		public override void OnInitializeMelon()
		{
			if (isRegistered)
				OnInitialise();
		}
#endif

#if WORKSHOP

		protected virtual void OnPostActivate(Mod mod) { }
		protected virtual void OnPostInject() { }
		protected virtual void OnPreInject() { }

		public override void PostActivate(Mod mod) //IModInitializer
		{
			OnPostActivate(mod);
		}

		public override void PostInject() //IModInitializer
		{
			OnPostInject();
		}

		public override void PreInject() //IModInitializer
		{
			OnPreInject();
		}

		protected override void OnUpdate() //IModSystem
		{
			OnFrameUpdate();
		}

		protected override void Initialise() //IModSystem
		{
			OnInitialise();
		}
#endif

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
	}
}
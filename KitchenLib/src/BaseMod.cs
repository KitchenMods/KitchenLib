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
		
#if BEPINEX || WORKSHOP
		public HarmonyLib.Harmony harmonyInstance;
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
			harmonyInstance = new HarmonyLib.Harmony(modID);
			if (assembly != null)
				harmonyInstance.PatchAll(assembly);
#endif

			semVersion = new SemVersion(version.Major, version.Minor, version.Patch);
			ModRegistery.Register(this);
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

		protected virtual void OnFrameUpdate() { }
		protected virtual void OnInitialise() { }

#if BEPINEX
		void Update()
		{
			OnFrameUpdate();
		}

		void Awake()
		{
			OnInitialise();
		}
#endif

#if MELONLOADER
		public override void OnUpdate()
		{
			OnFrameUpdate();
		}

		public override void OnInitializeMelon()
		{
			OnInitialise();
		}
#endif

#if WORKSHOP
		protected override void OnUpdate()
		{
			OnFrameUpdate();
		}

		protected override void Initialise()
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
using System.Reflection;
using Semver;
using UnityEngine;
using KitchenLib.Utils;
using System.Runtime.CompilerServices;
using KitchenLib.Registry;
using KitchenLib.Customs;
using KitchenLib.DevUI;
using System;
using System.Collections.Generic;
using KitchenMods;

namespace KitchenLib
{
	public abstract class BaseMod : LoaderMod
	{
		public string ModID = "";
		public string ModName = "";
		public string ModAuthor = "";
		public string ModVersion = "";
		public string CompatibleVersions = "";
		public string BetaVersion = "";

		public static KitchenVersion version;
		public static SemVersion semVersion;

		public static BaseMod instance;
		private static List<Assembly> PatchedAssemblies = new List<Assembly>();
		private bool isRegistered = false;
		private bool canRegisterGDO = false;
		
		public static HarmonyLib.Harmony harmonyInstance;
		
		public BaseMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly) : base()
		{
			SetupMod(modID, modName, author, modVersion, "", compatibleVersions, assembly);
		}

		[Obsolete("Please use BaseMod(string modID, string modName, string author, string modVersion, string betaVersion, string compatibleVersions, Assembly assembly)")]
		public BaseMod(string modID, string compatibleVersions, string[] modDependencies = null) : base()
		{
			SetupMod(modID, "Unsupported Name", "Unsupported Author", "0.0.0", "", compatibleVersions, null);
		}
		[Obsolete("Please use BaseMod(string modID, string modName, string author, string modVersion, string betaVersion, string compatibleVersions, Assembly assembly)")]
		public BaseMod(string compatibleVersions, Assembly assembly, string[] modDependencies = null) : base()
		{
			SetupMod("unsupportedmodid", "Unsupported Name", "Unsupported Author", "0.0.0", "", compatibleVersions, assembly);
		}

		[Obsolete("Please use BaseMod(string modID, string modName, string author, string modVersion, string betaVersion, string compatibleVersions, Assembly assembly)")]
		public BaseMod(string modID, string modVersion, string compatibleVersions, Assembly assembly) : base()
		{
			SetupMod(modID, "Unsupported Name", "Unsupported Author", modVersion, "", compatibleVersions, assembly);
		}

		public BaseMod(string modID, string modName, string author, string modVersion, string betaVersion, string compatibleVersions, Assembly assembly) : base()
		{
			SetupMod(modID, modName, author, modVersion, betaVersion, compatibleVersions, assembly);
		}

		private void SetupMod(string modID, string modName, string author, string modVersion, string betaVersion, string compatibleVersions, Assembly assembly)
		{
			instance = this;
			ModID = modID;
			ModName = modName;
			ModAuthor = author;
			ModVersion = modVersion;
			if (!string.IsNullOrEmpty(betaVersion))
				BetaVersion = " b" + betaVersion;
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
			canRegisterGDO = true;

		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(string message)
		{
			Debug.Log($"[{ModName}] " + message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Warning(string message)
		{
			Debug.LogWarning($"[{ModName}] " + message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(string message)
		{
			Debug.LogError($"[{ModName}] " + message);
		}

		protected virtual void OnInitialise() { }
		protected virtual void OnFrameUpdate() { }

		protected virtual void OnPostActivate(Mod mod) { }
		protected virtual void OnPostInject() { }
		protected virtual void OnPreInject() { }

		public sealed override void PostActivate(Mod mod) //IModInitializer
		{
			foreach (AssetBundleModPack pack in mod.GetPacks<AssetBundleModPack>())
			{
				foreach (AssetBundle bundle in pack.AssetBundles)
				{
					JSONManager.LoadAllJsons(bundle);
				}
			}

			foreach (CustomBaseMaterial material in JSONManager.LoadedJsons)
			{
				Material mat;
				material.ConvertMaterial(out mat);
				AddMaterial(mat);
			}
			OnPostActivate(mod);
			canRegisterGDO = false;
		}

		public sealed override void PostInject() //IModInitializer
		{
			OnPostInject();
		}

		public sealed override void PreInject() //IModInitializer
		{
			OnPreInject();
		}

		protected override void OnUpdate() //IModSystem
		{
			OnFrameUpdate();
		}

		protected sealed override void Initialise() //IModSystem
		{
			if (!ModRegistery.InitialisedMods.Contains(ModAuthor + ModID))
			{
				OnInitialise();
				ModRegistery.InitialisedMods.Add(ModAuthor + ModID);
			}
		}

		public T AddGameDataObject<T>() where T : CustomGameDataObject, new()
		{
			T gdo = new T();
			gdo.ModName = ModName;
			if (canRegisterGDO)
			{
				return CustomGDO.RegisterGameDataObject(gdo);
			}
			else
			{
				Main.instance.Warning("Please Register GDOs in OnPostActivate(Mod mod) " + gdo.GetType().FullName);
				return null;
			}
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

		public Material AddCustomMaterial<T>() where T : CustomBaseMaterial, new()
		{
			T material = new T();
			material.ConvertMaterial(out Material newMaterial);
			return CustomMaterials.AddMaterial(newMaterial.name, newMaterial);
		}

		public Material AddMaterial<T>() where T : Material, new()
		{
			T material = new T();
			return CustomMaterials.AddMaterial(material.name, material);
		}

		public Material AddMaterial(Material material)
		{
			return CustomMaterials.AddMaterial(material.name, material);
		}

		public void RegisterMenu<T>() where T : BaseUI, new()
		{
			T menu = new T();
			DevUIController._uiList.Add(menu);
		}
	}
}
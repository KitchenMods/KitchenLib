using KitchenLib.Customs;
using KitchenLib.DevUI;
using KitchenLib.Logging;
using KitchenLib.Patches;
using KitchenLib.Registry;
using KitchenLib.Utils;
using KitchenLib.Views;
using KitchenMods;
using Semver;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

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

		private static List<Assembly> PatchedAssemblies = new List<Assembly>();
		private bool isRegistered = false;
		private bool canRegisterGDO = false;

		[Obsolete("This will point to different mods at different times, use your own singleton variable instead.")]
		public static BaseMod instance;

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
			DebugLogPatch.SetupCustomLogHandler();

			instance = this;
			ModID = modID;
			ModName = modName;
			ModAuthor = author;
			ModVersion = modVersion;
			if (!string.IsNullOrEmpty(betaVersion))
				BetaVersion = " b" + betaVersion;
			CompatibleVersions = compatibleVersions;

			if (!Debug.isDebugBuild)
				version = new KitchenVersion(Application.version, this);
			else
				version = new KitchenVersion("", this);

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

			foreach (BaseJson json in JSONManager.LoadedJsons)
			{
				if (json is CustomBaseMaterial)
				{
					CustomBaseMaterial customBaseMaterial = json as CustomBaseMaterial;
					Material mat;
					customBaseMaterial.ConvertMaterial(out mat);
					AddMaterial(mat);
				}
				else if (json is CustomMaterial)
				{
					CustomMaterial customBaseMaterial = json as CustomMaterial;
					Material mat;
					customBaseMaterial.Deserialise();
					customBaseMaterial.ConvertMaterial(out mat);
					AddMaterial(mat);
				}
			}
			
			try
			{
				OnPostActivate(mod);
			}
			catch (Exception e)
			{
				Main.LogWarning($"{ModID} has failed to PostActivate.");
				Main.LogWarning(e);
			}
			
			canRegisterGDO = false;
		}

		public sealed override void PostInject() //IModInitializer
		{
			try
			{
				OnPostInject();
			}
			catch (Exception e)
			{
				Main.LogWarning($"{ModID} has failed to PostInject.");
				Main.LogWarning(e);
			}
		}

		public sealed override void PreInject() //IModInitializer
		{
			try
			{
				OnPreInject();
			}
			catch (Exception e)
			{
				Main.LogWarning($"{ModID} has failed to PreInject.");
				Main.LogWarning(e);
			}
		}

		protected override void OnUpdate() //IModSystem
		{
			try
			{
				OnFrameUpdate();
			}
			catch (Exception e)
			{
				Main.LogWarning($"{ModID} has failed to OnUpdate.");
				Main.LogWarning(e);
			}
		}

		protected sealed override void Initialise() //IModSystem
		{
			if (!ModRegistery.InitialisedMods.Contains(ModAuthor + ModID))
			{
				try
				{
					OnInitialise();
				}
				catch (Exception e)
				{
					Main.LogWarning($"{ModID} has failed to Initialise.");
					Main.LogWarning(e);
				}
				ModRegistery.InitialisedMods.Add(ModAuthor + ModID);
			}
		}

		public T AddGameDataObject<T>() where T : CustomGameDataObject, new()
		{
			T gdo = new T();
			gdo.ModID = ModID;
			gdo.ModName = ModName;
			if (canRegisterGDO)
			{
				return CustomGDO.RegisterGameDataObject(gdo);
			}
			else
			{
				Main.LogWarning("Please Register GDOs in OnPostActivate(Mod mod) " + gdo.GetType().FullName);
				return null;
			}
		}

		public CustomGameDataObject AddGameDataObject(Type t)
		{
			if(t.IsCastableTo(typeof(CustomGameDataObject)))
			{
				CustomGameDataObject gdo = (CustomGameDataObject)Activator.CreateInstance(t);
				if (canRegisterGDO)
				{
					return CustomGDO.RegisterGameDataObject(gdo);
				}
				else
				{
					Main.Logger.LogWarning("Please Register GDOs in OnPostActivate(Mod mod) " + gdo.GetType().FullName);
					return null;
				}
			}
			else
			{
				Main.Logger.LogError("Please make sure " + t.FullName + " extends CustomGameDataObject");
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
			if (CustomMaterials.CustomMaterialsIndex.ContainsKey(material.name))
			{
				return material;
			}
			else
			{
				return CustomMaterials.AddMaterial(material.name, material);
			}
		}

		public Material AddMaterial(Material material)
		{
			if (CustomMaterials.CustomMaterialsIndex.ContainsKey(material.name))
			{
				return material;
			}
			else
			{
				return CustomMaterials.AddMaterial(material.name, material);
			}
		}

		/// <summary>
		/// Register a custom view type.
		/// </summary>
		/// <param name="id">The view ID.</param>
		/// <returns>The corresponding CustomViewType, for assigning to a field.</returns>
		public CustomViewType AddViewType(string id)
		{
			return AddViewType(id, (GameObject)null);
		}

		/// <summary>
		/// Register a custom view type with the specified prefab.
		/// </summary>
		/// <param name="id">The view ID.</param>
		/// <param name="prefab">The prefab.</param>
		/// <returns>The corresponding CustomViewType, for assigning to a field.</returns>
		public CustomViewType AddViewType(string id, GameObject prefab)
		{
			return CustomViewType.Register(ModID, id, () => prefab);
		}

		/// <summary>
		/// Register a custom view type with the specified prefab builder. The prefab builder is lazily-evaluated only once.
		/// </summary>
		/// <param name="id">The view ID.</param>
		/// <param name="prefab">The prefab builder.</param>
		/// <returns>The corresponding CustomViewType, for assigning to a field.</returns>
		public CustomViewType AddViewType(string id, Func<GameObject> prefab)
		{
			return CustomViewType.Register(ModID, id, prefab);
		}

		public void RegisterMenu<T>() where T : BaseUI, new()
		{
			T menu = new T();
			DevUIController._uiList.Add(menu);
		}

		public KitchenLogger InitLogger()
		{
			return new KitchenLogger(ModName);
		}

		public Dictionary<string, CustomGameDataObject> RegisterGameDataObjects(Type t)
		{
			Dictionary<string, CustomGameDataObject> keyValuePairs = new Dictionary<string, CustomGameDataObject>();
			Assembly asm = ModRegistery.keyValuePairs[t];
			foreach (Type type in asm.GetTypes())
			{
				if(typeof(IAddGameData).IsAssignableFrom(type))
				{
					Main.Logger.LogWarning("THIS IS IMPORTANT: " + type.FullName);
					keyValuePairs.Add(type.Name, AddGameDataObject(type));
				}
			}
			return keyValuePairs;
		}
	}
}
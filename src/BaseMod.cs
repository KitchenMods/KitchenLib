using KitchenLib.Customs;
using KitchenLib.DevUI;
using KitchenLib.Logging;
using KitchenLib.Patches;
using KitchenLib.Registry;
using KitchenLib.Utils;
using KitchenLib.Views;
using KitchenMods;
using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using KitchenLib.Interfaces;
using KitchenLib.Materials;
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
		internal static KitchenLogger InternalLogger;

		private static Dictionary<string, Harmony> HarmonyInstances = new Dictionary<string, Harmony>();
		private static Dictionary<string, List<Assembly>> PatchedAssemblies = new Dictionary<string, List<Assembly>>();
		
		private bool isRegistered = false;
		private bool canRegisterGDO = false;
		private Mod mod;

		[Obsolete("This will point to different mods at different times, use your own singleton variable instead.")]
		public static BaseMod instance;
		
		#region BaseMod Definitions

		#region Old Definitions

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

		#endregion

		#region Current Definitions

		public BaseMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly) : base()
		{
			SetupMod(modID, modName, author, modVersion, "", compatibleVersions, assembly);
		}
		
		public BaseMod(string modID, string modName, string author, string modVersion, string betaVersion, string compatibleVersions, Assembly assembly) : base()
		{
			SetupMod(modID, modName, author, modVersion, betaVersion, compatibleVersions, assembly);
		}

		#endregion
		
		#endregion
		
		
		private void SetupMod(string modID, string modName, string author, string modVersion, string betaVersion, string compatibleVersions, Assembly assembly)
		{
			if (InternalLogger == null)
				InternalLogger = new KitchenLogger("KL Internal");
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

			#region HarmonyX Patching

			if (!HarmonyInstances.ContainsKey(modID))
				HarmonyInstances.Add(modID, new Harmony(modID));
			
			if (!PatchedAssemblies.ContainsKey(modID))
				PatchedAssemblies.Add(modID, new List<Assembly>());
			
			if (!PatchedAssemblies[modID].Contains(assembly))
			{
				if (assembly != null)
				{
					HarmonyInstances[modID].PatchAll(assembly);
					PatchedAssemblies[modID].Add(assembly);
				}
			}

			#endregion

			semVersion = new SemVersion(version.Major, version.Minor, version.Patch);
			isRegistered = ModRegistery.Register(this);
			canRegisterGDO = true;
		}

		protected virtual void OnInitialise() { }
		protected virtual void OnFrameUpdate() { }

		protected virtual void OnPostActivate(Mod mod) { }
		protected virtual void OnPostInject() { }
		protected virtual void OnPreInject() { }

		public sealed override void PostActivate(Mod mod) //IModInitializer
		{
			this.mod = mod;

			LoadJSONAssets();
			LoadCustomMaterials();

			AutoRegisterGameDataObjects(typeof(IAutoRegisterAll), typeof(IDontRegister), typeof(IRegisterGDO));
			
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
			try
			{
				OnFrameUpdate();
			}
			catch (Exception e)
			{
				InternalLogger.LogWarning($"{ModID} has failed to OnUpdate.");
				InternalLogger.LogWarning(e);
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
					InternalLogger.LogWarning($"{ModID} has failed to Initialise.");
					InternalLogger.LogWarning(e);
				}
				ModRegistery.InitialisedMods.Add(ModAuthor + ModID);
			}
		}

		#region Internal Code

		private void LoadJSONAssets()
		{
			foreach (AssetBundleModPack pack in mod.GetPacks<AssetBundleModPack>())
			{
				foreach (AssetBundle bundle in pack.AssetBundles)
				{
					JSONManager.LoadAllJsons(bundle);
				}
			}
		}

		private void LoadCustomMaterials()
		{
			foreach (BaseJson json in JSONManager.LoadedJsons)
			{
				if (json is CustomBaseMaterial customBaseMaterial) // Legacy
				{
					customBaseMaterial.ConvertMaterial(out Material mat);
					MaterialManager.RegisterCustomMaterial(mat.name, mat);
				}
				else if (json is CustomMaterial customMaterial)
				{
					customMaterial.Deserialise();
					customMaterial.ConvertMaterial(out Material mat);
					MaterialManager.RegisterCustomMaterial(mat.name, mat);
				}
			}
		}

		private void AutoRegisterGameDataObjects(Type registerAll, Type dontRegister, Type singleRegister)
		{
			if (GetType().GetInterfaces().Contains(registerAll))
			{
				foreach (Type type in this.GetType().Assembly.GetTypes())
				{
					if (type.IsAbstract || !typeof(CustomGameDataObject).IsAssignableFrom(type) || dontRegister.IsAssignableFrom(type))
						continue;

					AddGameDataObjectAutomatically(type);
				}
			}

			foreach (AssemblyModPack pack in mod.GetPacks<AssemblyModPack>())
			{
				foreach (Type type in pack.Asm.GetTypes())
				{
					if (type.GetInterfaces().Contains(singleRegister))
					{
						AddGameDataObjectByInterface(type);
					}
				}
			}
		}

		internal static void ObsoleteCodeWarning(string className, string methodName)
		{
			InternalLogger.LogWarning($"[Obsolete Warning] {className}.{methodName} is marked as Obsolete, but is still being used.");
		}

		#endregion

		#region External Code
		
		public object AddGameDataObjectByInterface(Type gdo)
		{
			InternalLogger.LogDebug($"Registering {gdo.FullName} by interface.");
			return AddGameDataObjectType(gdo);
		}

		public object AddGameDataObjectAutomatically(Type gdo)
		{
			InternalLogger.LogDebug($"Registering {gdo.FullName} automatically.");
			return AddGameDataObjectType(gdo);
		}

		public object AddGameDataObjectType(Type gdo)
		{
			MethodInfo method = ReflectionUtils.GetMethod<BaseMod>("AddGameDataObject");
			MethodInfo generic = method.MakeGenericMethod(gdo);
			return generic.Invoke(this, new object[]{});
		}
		
		public T AddGameDataObject<T>() where T : CustomGameDataObject, new()
		{
			T gdo = new T();
			gdo.ModID = ModID;
			gdo.ModName = ModName;
			gdo.mod = mod;
			if (canRegisterGDO)
			{
				return CustomGDO.RegisterGameDataObject(gdo);
			}
			else
			{
				InternalLogger.LogWarning("Please Register GDOs in OnPostActivate(Mod mod) " + gdo.GetType().FullName);
				return null;
			}
		}

		public KitchenLogger InitLogger()
		{
			return new KitchenLogger(ModName);
		}
		
		public void RegisterMenu<T>() where T : BaseUI, new()
		{
			T menu = new T();
			DevUIController._uiList.Add(menu);
		}

		#endregion

		#region Obsolete Code

		[Obsolete("Please use Appliance.ApplianceProcesses and Item.ItemProcess")]
		public T AddSubProcess<T>() where T : CustomSubProcess, new()
		{
			T subProcess = new T();
			return CustomSubProcess.RegisterSubProcess(subProcess);
		}

		[Obsolete("Please use PreferenceManager")]
		public T AddPreference<T>(string modID, string key, string name) where T : BasePreference, new()
		{
			T preference = new T();
			return PreferenceUtils.Register<T>(modID, key, name);
		}
		
		/// <summary>
		/// Register a custom view type.
		/// </summary>
		/// <param name="id">The view ID.</param>
		/// <returns>The corresponding CustomViewType, for assigning to a field.</returns>
		[Obsolete("Please use ViewUtils.RegisterView")]
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
		[Obsolete("Please use ViewUtils.RegisterView")]
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
		[Obsolete("Please use ViewUtils.RegisterView")]
		public CustomViewType AddViewType(string id, Func<GameObject> prefab)
		{
			return CustomViewType.Register(ModID, id, prefab);
		}
		[Obsolete("Please use MaterialManager")]
		public Material AddCustomMaterial<T>() where T : CustomBaseMaterial, new()
		{
			T material = new T();
			material.ConvertMaterial(out Material newMaterial);
			return CustomMaterials.AddMaterial(newMaterial.name, newMaterial);
		}

		[Obsolete("Please use MaterialManager")]
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

		[Obsolete("Please use MaterialManager")]
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Please use a KitchenLogger instance")]
		public void Log(string message)
		{
			Debug.Log($"[{ModName}] " + message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Please use a KitchenLogger instance")]
		public void Warning(string message)
		{
			Debug.LogWarning($"[{ModName}] " + message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Please use a KitchenLogger instance")]
		public void Error(string message)
		{
			Debug.LogError($"[{ModName}] " + message);
		}

		#endregion
	}
}
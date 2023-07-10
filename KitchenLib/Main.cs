using Kitchen;
using KitchenLib.Colorblind;
using KitchenLib.Customs;
using KitchenLib.DevUI;
using KitchenLib.Event;
using KitchenLib.Preferences;
using KitchenLib.UI;
using KitchenMods;
using System.Linq;
using System.Reflection;
using UnityEngine;
using KitchenLib.UI.PlateUp;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using System.Runtime.CompilerServices;
using System;
using KitchenData;
using System.IO;
using KitchenLib.References;

namespace KitchenLib
{
	/// <summary>
	/// The main class of the KitchenLib mod.
	/// </summary>
	public class Main : BaseMod
	{
		/// <summary>
		/// The ID of the mod.
		/// </summary>
		internal const string MOD_ID = "kitchenlib";

		/// <summary>
		/// The name of the mod.
		/// </summary>
		internal const string MOD_NAME = "KitchenLib";

		/// <summary>
		/// The author of the mod.
		/// </summary>
		internal const string MOD_AUTHOR = "KitchenMods";

		/// <summary>
		/// The version of the mod.
		/// </summary>
		internal const string MOD_VERSION = "0.7.9";

		/// <summary>
		/// The beta version of the mod.
		/// </summary>
		internal const string MOD_BETA_VERSION = "3";

		/// <summary>
		/// The compatible versions of the mod.
		/// </summary>
		internal const string MOD_COMPATIBLE_VERSIONS = ">=1.1.6";

		/// <summary>
		/// The holder for synchronizing views.
		/// </summary>
		internal static CustomAppliance SyncModsViewHolder;

		/// <summary>
		/// The asset bundle for the mod.
		/// </summary>
		internal static AssetBundle bundle;

		/// <summary>
		/// The preference manager for the mod.
		/// </summary>
		internal static PreferenceManager manager;

		/// <summary>
		/// The logger for the mod.
		/// </summary>
		internal static KitchenLogger Logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="Main"/> class.
		/// </summary>
		public Main() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_BETA_VERSION, MOD_COMPATIBLE_VERSIONS, Assembly.GetExecutingAssembly()) { }

		/// <summary>
		/// Called after the mod is activated.
		/// </summary>
		/// <param name="mod">The mod instance.</param>
		protected override void OnPostActivate(Mod mod)
		{
			Logger = InitLogger();
			manager = new PreferenceManager(MOD_ID);
			manager.RegisterPreference(new PreferenceBool("enableChangingMenu", true));
			manager.Load();
			bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_ID);
			SyncModsViewHolder = AddGameDataObject<SyncModsViewHolder>();
			SetupMenus();
			RegisterMenu<NewMaterialUI>();
			RegisterMenu<DebugMenu>();
			FeatureFlags.Init();

			/*
			// View types
			AddViewType("imms", () =>
			{
				var res = new GameObject
				{
					name = "IMMS"
				};
				res.AddComponent<IMMSView>();

				return res;
			});

			// IMMS logger
			IMMSManager.RegisterAll((string key, IMMSContext ctx, object[] args) =>
			{
				LogInfo($"[IMMS] id={ctx.Id} channel={ctx.Channel} key={key} source={ctx.Source} target={ctx.Target} type={ctx.Type} args={string.Join(",", args.Select(Convert.ToString))}");
				return null;
			});
			*/
		}

		/// <summary>
		/// Called during the initialization phase.
		/// </summary>
		protected override void OnInitialise()
		{
			GameObject go = new GameObject();
			go.AddComponent<DevUIController>();

			ColorblindUtils.AddSingleItemLabels(ColorblindUtils.itemLabels.ToArray());
		}

		/// <summary>
		/// Sets up the menus for the mod.
		/// </summary>
		private void SetupMenus()
		{

			ModsPreferencesMenu<PauseMenuAction>.RegisterMenu("KitchenLib", typeof(PreferenceMenu<PauseMenuAction>), typeof(PauseMenuAction));
			ModsPreferencesMenu<MainMenuAction>.RegisterMenu("KitchenLib", typeof(PreferenceMenu<MainMenuAction>), typeof(MainMenuAction));

			//Setting Up For Main Menu
			Events.StartMainMenu_SetupEvent += (s, args) =>
			{
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mods", typeof(ModsMenu<MainMenuAction>), false });
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mod Preferences", typeof(ModsPreferencesMenu<MainMenuAction>), false });
			};
			Events.MainMenuView_SetupMenusEvent += (s, args) =>
			{
				args.addMenu.Invoke(args.instance, new object[] { typeof(RevisedMainMenu), new RevisedMainMenu(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsMenu<MainMenuAction>), new ModsMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsPreferencesMenu<MainMenuAction>), new ModsPreferencesMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
			};

			//Setting Up For Pause Menu
			Events.MainMenu_SetupEvent += (s, args) =>
			{
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mods", typeof(ModsMenu<PauseMenuAction>), false });
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mod Preferences", typeof(ModsPreferencesMenu<PauseMenuAction>), false });
			};
			Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
			{
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsMenu<PauseMenuAction>), new ModsMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsPreferencesMenu<PauseMenuAction>), new ModsPreferencesMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModSyncMenu), new ModSyncMenu(args.instance.ButtonContainer, args.module_list) });
			};

			Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent += (s, args) =>
			{
				args.Menus.Add(typeof(PreferenceMenu<PauseMenuAction>), new PreferenceMenu<PauseMenuAction>(args.Container, args.Module_list));
			};

			Events.PreferenceMenu_MainMenu_CreateSubmenusEvent += (s, args) =>
			{
				args.Menus.Add(typeof(PreferenceMenu<MainMenuAction>), new PreferenceMenu<MainMenuAction>(args.Container, args.Module_list));
			};
		}
		private void ExtractAssets(GameDataObject gameDataObject)
		{
			FieldInfo[] fields = gameDataObject.GetType().GetFields();
			foreach (FieldInfo field in fields)
			{
				if (field.FieldType == typeof(GameObject))
				{
					Texture2D texture = null;
					GameObject prefab = (GameObject)field.GetValue(gameDataObject);
					if (prefab != null)
					{
						texture = PrefabSnapshot.GetApplianceSnapshot(prefab);
						byte[] bytes = null;
						if (texture != null)
							bytes = texture.EncodeToPNG();

						var dirPath = Application.dataPath + "/../SaveImages/";
						if (!Directory.Exists(dirPath))
						{
							Directory.CreateDirectory(dirPath);
						}
						if (bytes != null)
							File.WriteAllBytes(dirPath + gameDataObject.ID + "-" + gameDataObject.GetType().ToString() + "-" + field.Name + ".png", bytes);
					}
				}
			}
		}

		private void ExtractAllAssets()
		{
			foreach (GameDataObject gameDataObject in GameData.Main.Get<GameDataObject>())
			{
				ExtractAssets(gameDataObject);
			}
		}

		/// <summary>
		/// Registers a new cape.
		/// </summary>
		/// <typeparam name="T">The type of the cape.</typeparam>
		/// <param name="id">The ID of the cape.</param>
		/// <param name="display">The display name of the cape.</param>
		private void RegisterNewCape<T>(string id, string display) where T : CustomPlayerCosmetic, new()
		{
			AddGameDataObject<T>();
		}

		[Obsolete]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void LogInfo(string message)
		{
			Debug.Log($"[{MOD_NAME}] " + message);
		}

		[Obsolete]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void LogWarning(string message)
		{
			Debug.LogWarning($"[{MOD_NAME}] " + message);
		}

		[Obsolete]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void LogError(string message)
		{
			Debug.LogError($"[{MOD_NAME}] " + message);
		}
	}
}
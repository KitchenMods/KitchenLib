using Kitchen;
using KitchenData;
using KitchenLib.Colorblind;
using KitchenLib.Customs;
using KitchenLib.DevUI;
using KitchenLib.Event;
using KitchenLib.Preferences;
using KitchenLib.UI;
using KitchenMods;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using KitchenLib.UI.PlateUp;
using KitchenLib.JSON;

namespace KitchenLib
{
	public class Main : BaseMod
	{
		public const string MOD_ID = "kitchenlib";
		public const string MOD_NAME = "KitchenLib";
		public const string MOD_AUTHOR = "KitchenMods";
		public const string MOD_VERSION = "0.7.8";
		public const string MOD_BETA_VERSION = "0";
		public const string MOD_COMPATIBLE_VERSIONS = ">=1.1.4";

		public static CustomAppliance CommandViewHolder;
		public static CustomAppliance InfoViewHolder;
		public static CustomAppliance SendToClientViewHolder;
		public static CustomAppliance TileHighlighterViewHolder;
		public static CustomAppliance ClientEquipCapeViewHolder;
		public static CustomAppliance SyncModsViewHolder;
		public static AssetBundle bundle;

		public static PreferenceManager manager;
		public static PreferenceManager cosmeticManager;

		public Main() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_BETA_VERSION, MOD_COMPATIBLE_VERSIONS, Assembly.GetExecutingAssembly()) { }
		protected override void OnPostActivate(Mod mod)
		{
			ContentPackManager.Initialise();
			ContentPackManager.ApplyPatches();
			ContentPackManager.InjectGDOs();

			manager = new PreferenceManager(MOD_ID);
			cosmeticManager = new PreferenceManager(MOD_ID + ".cosmetics");
			manager.RegisterPreference(new PreferenceBool("enableChangingMenu", true));
			manager.Load();
			
			bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).ToList()[0];
			
			SyncModsViewHolder = AddGameDataObject<SyncModsViewHolder>();

			RegisterNewCape<ItsHappeningCape>("itsHappening", "Its Happening! Cape");
			RegisterNewCape<StaffCape>("staff", "Staff Cape");
			RegisterNewCape<KitchenLibCape>("support", "KitchenLib Cape");
			RegisterNewCape<SupportCape>("kitchenlib", "Support Cape");
			RegisterNewCape<TwitchCape>("twitch", "Twitch Cape");
			RegisterNewCape<EasterCape>("easter2023", "Easter Champion Cape");
			RegisterNewCape<GearsCape>("gears2023", "Gears Champion Cape");
			RegisterNewCape<Discord_BoostCape>("discordboost", "Booster Cape");

			SetupMenus();
			RegisterMenu<NewMaterialUI>();
			RegisterMenu<DebugMenu>();
			
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

			// Init feature flags
			FeatureFlags.Init();
		}
		protected override void OnInitialise()
		{
			GameObject go = new GameObject();
			go.AddComponent<DevUIController>();
			ColorblindUtils.AddSingleItemLabels(ColorblindUtils.itemLabels.ToArray());
		}

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

		private void ExtractAssets()
		{
			foreach (GameDataObject gameDataObject in GameData.Main.Get<GameDataObject>())
			{
				Texture2D texture = null;
				if (gameDataObject.GetType() == typeof(Appliance))
					if (((Appliance)gameDataObject).Prefab != null)
						texture = PrefabSnapshot.GetApplianceSnapshot(((Appliance)gameDataObject).Prefab);
				if (gameDataObject.GetType() == typeof(Item))
					if (((Item)gameDataObject).Prefab != null)
						texture = PrefabSnapshot.GetApplianceSnapshot(((Item)gameDataObject).Prefab);
				if (gameDataObject.GetType() == typeof(PlayerCosmetic))
					if (((PlayerCosmetic)gameDataObject).Visual != null)
						texture = PrefabSnapshot.GetApplianceSnapshot(((PlayerCosmetic)gameDataObject).Visual);

				byte[] bytes = null;
				if (texture != null)
					bytes = texture.EncodeToPNG();

				var dirPath = Application.dataPath + "/../SaveImages/";
				if (!Directory.Exists(dirPath))
				{
					Directory.CreateDirectory(dirPath);
				}
				if (bytes != null)
					File.WriteAllBytes(dirPath + gameDataObject.ID + "-" + gameDataObject.name + ".png", bytes);
			}
		}

		public void RegisterNewCape<T>(string id, string display) where T : CustomPlayerCosmetic, new()
		{
			AddGameDataObject<T>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void LogInfo(string message)
		{
			Debug.Log($"[{MOD_NAME}] " + message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void LogWarning(string message)
		{
			Debug.LogWarning($"[{MOD_NAME}] " + message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void LogError(string message)
		{
			Debug.LogError($"[{MOD_NAME}] " + message);
		}
	}
}
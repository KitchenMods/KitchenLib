using System.Reflection;
using KitchenLib.Event;
using Kitchen;
using KitchenData;
using KitchenMods;
using UnityEngine;
using System.IO;
using KitchenLib.DevUI;
using KitchenLib.UI;
using KitchenLib.Colorblind;
using KitchenLib.Preferences;
using System.Collections.Generic;

namespace KitchenLib
{
	public class Main : BaseMod
	{
		public const string MOD_ID = "kitchenlib";
		public const string MOD_NAME = "KitchenLib";
		public const string MOD_AUTHOR = "KitchenMods";
		public const string MOD_VERSION = "0.5.2";
		public const string MOD_BETA_VERSION = "RC-5";
		public const string MOD_COMPATIBLE_VERSIONS = "1.1.3";

		public static AssetBundle bundle;
		public Main() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_BETA_VERSION, MOD_COMPATIBLE_VERSIONS, Assembly.GetExecutingAssembly()) { }

		protected override void OnPostActivate(Mod mod)
		{
			SetupMenus();
			RegisterMenu<MaterialsUI>();
			RegisterMenu<DebugMenu>();

			/*
			PreferenceManager manager = new PreferenceManager(MOD_ID); // Setting up the preference manager
			
			manager.RegisterPreference(new PreferenceBool("isEnabled")).Set(true); // Registering a preference and setting it's default value
			manager.RegisterPreference(new PreferenceInt("CustomerCount")).Set(5);
			manager.RegisterPreference(new PreferenceBool("CustomerCats")).Set(false);

			manager.Load(); // Loading the preferences from the save file (if any exists)

			Log("Are cats going to spawn? " + manager.Get<PreferenceBool>("CustomerCats")); // Getting the value of the preference
			*/

		}
		protected override void OnInitialise()
		{
			GameObject go = new GameObject();
			go.AddComponent<DevUIController>();

			ColorblindUtils.AddSingleItemLabels(ColorblindUtils.itemLabels.ToArray());
		}

		private void SetupMenus()
		{
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
	}
}
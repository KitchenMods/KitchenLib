using UnityEngine;
using Kitchen.Modules;
using KitchenLib.Event;
using KitchenLib.Utils;
using Kitchen;
using System.Collections.Generic;
using System;


namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib", "1.1.0") { }
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
		}

		public override void OnInitializeMelon()
		{

			PreferenceUtils.Register<BoolPreference>("kitchenlib","enabled", "Is Enabled");

			//Setting Up For Main Menu
			Events.StartMainMenu_SetupEvent += (s, args) =>
			{
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mods", typeof(ModsMenu<MainMenuAction>), false });
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mod Preferences", typeof(ModsPreferencesMenu<MainMenuAction>), false });
			};
			Events.MainMenuView_SetupMenusEvent += (s, args) =>
			{
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

			//Client Mod Setup
			Events.PreferenceMenu_SetupEvent += (s, args) =>
			{
				Type type = args.instance.GetType().GetGenericArguments()[0];
				args.mInfo.Invoke(args.instance, new object[] { "KitchenLib", typeof(KLSettingsMenu<>).MakeGenericType(type), false });
			};
			Events.PreferenceMenu_CreateSubmenusEvent += (s, args) =>
			{
				Type type = args.instance.GetType().GetGenericArguments()[0];
				//args.menus.Add(typeof(KLSettingsMenu<>).MakeGenericType(type), new KLSettingsMenu<MainMenuAction>(args.Container, args.Module_list));
				((Dictionary<Type, object>)args.Menus).Add(typeof(KLSettingsMenu<>).MakeGenericType(type), new KLSettingsMenu<MainMenuAction>(args.Container, args.Module_list));
			};
			
		}
  }

    

    public class KLSettingsMenu<T> : KLMenu<T>
    {
	    public KLSettingsMenu(Transform container, ModuleList module_list) : base(container, module_list)
	    {
	    }

	    public override void Setup(int player_id)
	    {
			/*
			* KitchenLib doesn't have any preferences at the moment, this is just a template for developers to follow.
			*/
			//AddInfo("KitchenLib doesn't actually have any preferences... yet.");

            AddLabel("Enable Mod");
            BoolOption(PreferenceUtils.Get<BoolPreference>("kitchenlib","enabled"));


            New<SpacerElement>();
		    New<SpacerElement>();
			
		    AddButton("Apply", delegate
		    {
                PreferenceUtils.Save();
		    });
			
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate
		    {
			    RequestPreviousMenu();
		    });
	    }
    }
}

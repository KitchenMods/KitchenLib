using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using KitchenLib.Event;
using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using KitchenLib.Utils;
using System.Reflection;


namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib", "1.0.5") { }
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
		}

		public override void OnInitializeMelon()
		{
			BoolPreference x = PreferencesRegistry.Register<BoolPreference>("kitchenlib", "enabled", "KL Settings");
			BoolPreference y = PreferencesRegistry.Register<BoolPreference>("kitchenlib", "disabled", "KL Settings");

            PreferencesRegistry.Load();
			Events.SetupEvent += (s, args) =>
			{
                args.Menu.AddNewButton(typeof(testmenu<MainMenuAction>), PreferencesRegistry.Get<BoolPreference>("kitchenlib","enabled").DisplayName);
                args.Menu.AddNewButton(typeof(testmenu<MainMenuAction>), PreferencesRegistry.Get<BoolPreference>("kitchenlib","disabled").DisplayName);
			};
			Events.CreateSubMenusEvent += (s, args) =>
			{
                args.Menus.Add(typeof(testmenu<MainMenuAction>), new testmenu<MainMenuAction>(args.Container, args.Module_list));
			};
		}
  }

    

    public class testmenu<T> : Menu<T>
    {
	    public testmenu(Transform container, ModuleList module_list) : base(container, module_list)
	    {
	    }

	    public override void Setup(int player_id)
	    {
            AddLabel("Enable Mod");
            BoolOption(PreferencesRegistry.Get<BoolPreference>("kitchenlib","enabled"));


            New<SpacerElement>();
		    New<SpacerElement>();
		    AddButton("Apply", delegate
		    {
                PreferencesRegistry.Save();
		    });
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate
		    {
			    RequestPreviousMenu();
		    });
	    }

        private void BoolOption(BoolPreference pref)
        {
			this.Add<bool>(new Option<bool>(new List<bool>
			{
				false,
				true
			}, (bool)pref.Value, new List<string>
			{
				this.Localisation["SETTING_DISABLED"],
				this.Localisation["SETTING_ENABLED"]
			}, null)).OnChanged += delegate(object _, bool f)
			{
				pref.Value = f;
			};
        }
    }
}

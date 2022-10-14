using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using KitchenLib.Event;
using KitchenLib.Utils;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace KitchenLib
{
    public partial class ModsPreferencesMenu : StartGameMainMenu
    {
        public ModsPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id) {
            AddLabel("Mod Preferences");
            
            New<SpacerElement>(true);
            
            Mod.Log("TestSetup");
            EventUtils.InvokeEvent(nameof(Events.SetupEvent), Events.SetupEvent?.GetInvocationList(), null, new SetupEventArgs(player_id, this));

            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddActionButton("Back", MainMenuAction.Back, ElementStyle.MainMenuBack);
        }

        public void AddNewButton(Type type, string name)
        {
            AddButton(name, delegate
            {
                RequestSubMenu(type);
            });
        }

        public override void CreateSubmenus(ref Dictionary<Type, Menu<MainMenuAction>> menus)
        {
            Mod.Log("TestCreate");
            EventUtils.InvokeEvent(nameof(Events.CreateSubMenusEvent), Events.CreateSubMenusEvent?.GetInvocationList(), null, new CreateSubMenusEventArgs(menus, Container, ModuleList));
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
            BoolOption(PreferencesRegistry.Get<TestA>("kitchenlib:settings").myBool);


            New<SpacerElement>();
		    New<SpacerElement>();
		    AddButton("Apply", delegate
		    {
                PreferencesRegistry.Save();
                PreferencesRegistry.Load();
		    });
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate
		    {
			    RequestPreviousMenu();
		    });
	    }

        private void BoolOption(bool value)
        {
            
			this.Add<bool>(new Option<bool>(new List<bool>
			{
				false,
				true
			}, value, new List<string>
			{
				this.Localisation["SETTING_DISABLED"],
				this.Localisation["SETTING_ENABLED"]
			}, null)).OnChanged += delegate(object _, bool f)
			{
				value = f;
			};
        }
    }
}
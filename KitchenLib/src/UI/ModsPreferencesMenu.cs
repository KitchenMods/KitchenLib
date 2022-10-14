using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using System.Collections.Generic;
using System;

namespace KitchenLib
{
    public partial class ModsPreferencesMenu : StartGameMainMenu
    {
        public ModsPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id) {
            AddLabel("Mod Preferences");
            
            New<SpacerElement>(true);
            foreach (ModPreference preference in PreferencesRegistry.Preferences.Values)
            {
                AddButton(preference.Name, delegate
		        {
			        RequestSubMenu(typeof(testmenu<MainMenuAction>));
		        });
            }
            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddActionButton("Back", MainMenuAction.Back, ElementStyle.MainMenuBack);
        }

        public override void CreateSubmenus(ref Dictionary<Type, Menu<MainMenuAction>> menus)
        {
		    menus.Add(typeof(testmenu<MainMenuAction>), new testmenu<MainMenuAction>(Container, ModuleList));
        }
    }

    public class testmenu<T> : Menu<T>
    {
        
	public testmenu(Transform container, ModuleList module_list)
		: base(container, module_list)
	{
	}

	public override void Setup(int player_id)
	{
        /*
		AddLabel(base.Localisation["SETTING_LETTERS_INSIDE"]);
		AddBoolOption(Pref.LettersSpawnInside);
		AddLabel(base.Localisation["SETTING_DESK_AS_PARCEL"]);
		AddBoolOption(Pref.ProvideStartingEnvelopesAsParcels);
        */
		New<SpacerElement>();
		New<SpacerElement>();
		AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate
		{
			RequestPreviousMenu();
		});
	}
    }
}
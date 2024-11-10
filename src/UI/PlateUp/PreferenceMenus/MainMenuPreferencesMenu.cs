using System;
using System.Collections.Generic;
using Kitchen.Modules;
using UnityEngine;

namespace KitchenLib.UI.PlateUp.PreferenceMenus
{
	public class MainMenuPreferencesMenu : BasePreferenceMenu
	{
		internal static List<(Type, string)> MenusToRegister = new List<(Type, string)>();

		public MainMenuPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		public static void RegisterMenu(string name, Type type)
		{
			foreach ((Type, string) menu in MenusToRegister)
			{
				if (menu.Item1 == type) return;
			}
			MenusToRegister.Add((type, name));
		}

		public override void Setup(int player_id)
		{
			_menusToRegister = MenusToRegister;
			base.Setup(player_id);
		}
	}
}
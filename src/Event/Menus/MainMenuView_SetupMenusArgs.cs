using Kitchen;
using Kitchen.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Event
{
	public class MainMenuView_SetupMenusArgs : EventArgs
	{
		public readonly MainMenuView instance;
		public readonly MethodInfo addMenu;
		public readonly ModuleList module_list;
		public readonly Dictionary<Type, Menu<MenuAction>> Menus;

		internal MainMenuView_SetupMenusArgs(MainMenuView instance, MethodInfo addMenu, ModuleList module_list, Dictionary<Type, Menu<MenuAction>> Menus)
		{
			this.instance = instance;
			this.addMenu = addMenu;
			this.module_list = module_list;
			this.Menus = Menus;
		}

		public void AddMenu(object[] parameters)
		{
			addMenu.Invoke(instance, parameters);
		}
	}
}
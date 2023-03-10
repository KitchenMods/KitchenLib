using Kitchen;
using Kitchen.Modules;
using System;
using System.Reflection;

namespace KitchenLib.Event
{
	public class PlayerPauseView_SetupMenusArgs : EventArgs
	{
		public readonly PlayerPauseView instance;
		public readonly MethodInfo addMenu;
		public readonly ModuleList module_list;

		internal PlayerPauseView_SetupMenusArgs(PlayerPauseView instance, MethodInfo addMenu, ModuleList module_list)
		{
			this.instance = instance;
			this.addMenu = addMenu;
			this.module_list = module_list;
		}

		public void AddMenu(object[] parameters)
		{
			addMenu.Invoke(instance, parameters);
		}
	}
}
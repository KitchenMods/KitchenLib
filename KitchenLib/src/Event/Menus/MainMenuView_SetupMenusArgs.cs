using Kitchen;
using System;
using System.Reflection;
using Kitchen.Modules;

namespace KitchenLib.Event
{
    public class MainMenuView_SetupMenusArgs : EventArgs
    {
        internal MainMenuView instance;
        internal MethodInfo addMenu;
        internal ModuleList module_list;

        internal MainMenuView_SetupMenusArgs(MainMenuView instance, MethodInfo addMenu, ModuleList module_list)
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
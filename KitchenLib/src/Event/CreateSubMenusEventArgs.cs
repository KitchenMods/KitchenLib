using KitchenData;
using System;
using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Event
{
    public class CreateSubMenusEventArgs : EventArgs
    {
        public readonly Dictionary<Type, Menu<MainMenuAction>> Menus;
        public readonly Transform Container;
        public readonly ModuleList Module_list;
        internal CreateSubMenusEventArgs(Dictionary<Type, Menu<MainMenuAction>> menus, Transform container, ModuleList module_list)
        {
            Menus = menus;
            Container = container;
            Module_list = module_list;
        }
    }

    public class SetupEventArgs : EventArgs
    {
        public readonly int PlayerId;
        public readonly ModsPreferencesMenu Menu;
        internal SetupEventArgs(int player_id, ModsPreferencesMenu menu)
        {
            PlayerId = player_id;
            Menu = menu;
        }
    }
}
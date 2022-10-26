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
    public class PreferenceMenu_CreateSubmenusArgs : EventArgs
    {
        public readonly Dictionary<Type, object> Menus;
        public readonly Transform Container;
        public readonly ModuleList Module_list;
        internal object instance;
        internal PreferenceMenu_CreateSubmenusArgs(object instance, object menus, Transform container, ModuleList module_list)
        {
            this.instance = instance;
            Menus = (Dictionary<Type, object>)menus;
            Container = container;
            Module_list = module_list;
        }
    }

    /*
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
    */
}
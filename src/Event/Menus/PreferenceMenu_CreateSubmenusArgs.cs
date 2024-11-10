using Kitchen;
using Kitchen.Modules;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Event
{
	public class PreferenceMenu_CreateSubmenusArgs<T> : EventArgs
	{
		public readonly Dictionary<Type, Menu<T>> Menus;
		public readonly Transform Container;
		public readonly ModuleList Module_list;
		public readonly object instance;
		internal PreferenceMenu_CreateSubmenusArgs(object instance, Dictionary<Type, Menu<T>> menus, Transform container, ModuleList module_list)
		{
			this.instance = instance;
			Menus = menus;
			Container = container;
			Module_list = module_list;
		}
	}
}
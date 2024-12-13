using Kitchen.Modules;
using System;
using System.Collections.Generic;
using Kitchen;
using KitchenLib.UI.PlateUp;
using UnityEngine;

namespace KitchenLib
{
	public class ModsPreferencesMenu<T> : KLMenu<T>
	{
		public ModsPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		internal static Dictionary<(string, Type), Type> MenusToRegister = new Dictionary<(string, Type), Type>();
		public static void RegisterMenu(string name, Type type, Type generic)
		{
			if (!MenusToRegister.ContainsKey((name, type))) 
				MenusToRegister.Add((name, type), generic);
		}
	}
}
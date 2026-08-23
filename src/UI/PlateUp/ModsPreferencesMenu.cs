using Kitchen.Modules;
using System;
using System.Collections.Generic;
using Kitchen;
using KitchenLib.UI.PlateUp.PreferenceMenus;
using UnityEngine;

namespace KitchenLib
{
	public class ModsPreferencesMenu<T> : KLMenu<T>
	{
		public ModsPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		internal static Dictionary<(string, Type), Type> MenusToRegister = new Dictionary<(string, Type), Type>();
		public static void RegisterMenu(string name, Type type, Type generic)
		{
			if (!TryCloseOverMenuAction(type, out Type menuType))
				return;

			if (typeof(T) == typeof(PauseMenuAction))
			{
				PauseMenuPreferencesesMenu.RegisterUsableMenu(menuType);
			}
			
			if (typeof(T) == typeof(MainMenuAction))
			{
				MainMenuPreferencesesMenu.RegisterUsableMenu(menuType);
			}

			if (!MenusToRegister.ContainsKey((name, menuType))) 
				MenusToRegister.Add((name, menuType), generic);
		}

		/*
		 * Menus are instantiated with Activator.CreateInstance, which cannot handle an open generic type
		 * definition, and the preference menus they get added to only ever run on MenuAction. Force any
		 * generic menu type to be closed over MenuAction, whatever it was registered as.
		 */
		private static bool TryCloseOverMenuAction(Type type, out Type menuType)
		{
			menuType = type;

			if (type == null)
			{
				BaseMod.InternalLogger.LogError("Failed to register menu: type was null.");
				return false;
			}

			if (!type.IsGenericType && !type.IsGenericTypeDefinition)
				return true;

			Type definition = type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition();
			if (definition.GetGenericArguments().Length != 1)
			{
				BaseMod.InternalLogger.LogError($"Failed to register menu {type.FullName}: expected a single generic argument.");
				return false;
			}

			try
			{
				menuType = definition.MakeGenericType(typeof(MenuAction));
			}
			catch (Exception e)
			{
				BaseMod.InternalLogger.LogError($"Failed to register menu {type.FullName}: cannot be used with MenuAction: {e}");
				return false;
			}

			return true;
		}
	}
}

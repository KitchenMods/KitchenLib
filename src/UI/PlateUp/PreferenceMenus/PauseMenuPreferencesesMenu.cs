using System;
using System.Collections.Generic;
using Kitchen.Modules;
using KitchenLib.Event;
using UnityEngine;

namespace KitchenLib.UI.PlateUp.PreferenceMenus
{
	public class PauseMenuPreferencesesMenu : BasePreferencesMenu
	{
		internal static List<(Type, string)> MenusToRegister = new List<(Type, string)>();
		
		public PauseMenuPreferencesesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }
		
		public static void RegisterMenu(string name, Type type, bool autoRegisterTyping = true)
		{
			foreach ((Type, string) menu in MenusToRegister)
			{
				if (menu.Item1 == type) return;
			}

			if (autoRegisterTyping)
			{
				Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
				{
					var menu = Activator.CreateInstance(type, new object[]{args.instance.ButtonContainer, args.module_list});
					args.addMenu.Invoke(args.instance, new object[] { type, menu });
				};
			}
			
			MenusToRegister.Add((type, name));
		}

		public override void Setup(int player_id)
		{
			_menusToRegister = MenusToRegister;
			base.Setup(player_id);
		}

		public static void RegisterUsableMenu(Type type)
		{
			Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
			{
				var menu = Activator.CreateInstance(type, new object[]{args.instance.ButtonContainer, args.module_list});
				args.addMenu.Invoke(args.instance, new object[] { type, menu });
			};
		}
	}
}
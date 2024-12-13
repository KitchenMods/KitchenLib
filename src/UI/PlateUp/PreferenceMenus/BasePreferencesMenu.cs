using System;
using System.Collections.Generic;
using System.Linq;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Event;
using UnityEngine;

namespace KitchenLib.UI.PlateUp.PreferenceMenus
{
	public class BasePreferencesMenu : KLMenu
	{
		public BasePreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		private int entriesPerPage = 5;
		private Dictionary<int, List<(Type, string)>> registeredMenus = new Dictionary<int, List<(Type, string)>>();
		protected List<(Type, string)> _menusToRegister = new List<(Type, string)>();
		private Option<int> PageSelector = null;
		private int CurrentPage = 0;
		
		public override void Setup(int player_id)
		{
			base.Setup(player_id);
			
			registeredMenus.Clear();
			int counter = 0;
			int page = 0;
			for (int i = 0; i < _menusToRegister.Count; i++)
			{
				Type menuType = _menusToRegister[i].Item1;
				string menuName = _menusToRegister[i].Item2;

				if (counter > entriesPerPage)
				{
					counter = 0;
					page++;
				}

				if (!registeredMenus.ContainsKey(page))
				{
					registeredMenus.Add(page, new List<(Type, string)>());
				}
				
				registeredMenus[page].Add((menuType, menuName));
				counter++;
			}

			List<string> menuNames = new List<string>();
			foreach (int pagenumber in registeredMenus.Keys)
			{
				menuNames.Add("Page " + (pagenumber + 1));
			}

			PageSelector = new Option<int>(registeredMenus.Keys.ToList(), 0, menuNames);
			PageSelector.OnChanged += delegate (object _, int result)
			{
				CurrentPage = result;
				Redraw();
			};

			Redraw();
		}

		protected override void Redraw()
		{
			base.Redraw();
			ModuleList.Clear();

			AddLabel("Mod Preferences");
			
			if (registeredMenus.Count > 1)
			{
				AddSelect(PageSelector);
				New<SpacerElement>(true);
			}

			if (registeredMenus.ContainsKey(CurrentPage))
			{
				foreach ((Type, string) menu in registeredMenus[CurrentPage])
				{
					AddSubmenuButton(menu.Item2, menu.Item1);
				}
			}

			New<SpacerElement>(true);
			New<SpacerElement>(true);
			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

		public static void RegisterUsableMenu(Type type)
		{
			Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
			{
				var menu = Activator.CreateInstance(type, new object[]{args.instance.ButtonContainer, args.module_list});
				args.addMenu.Invoke(args.instance, new object[] { type, menu });
			};
			
			Events.MainMenuView_SetupMenusEvent += (s, args) =>
			{
				var menu = Activator.CreateInstance(type, new object[]{args.instance.ButtonContainer, args.module_list});
				args.addMenu.Invoke(args.instance, new object[] { type, menu });
			};
		}
	}
}
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Event;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using KitchenLib.Preferences;
using UnityEngine;

namespace KitchenLib
{
	public class ModsPreferencesMenu<T> : KLMenu<T>
	{
		public ModsPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		private static Dictionary<(Type, Type), string> RegisteredMenus = new Dictionary<(Type, Type), string>();
		private static Dictionary<(Type, Type), int> MenuPages = new Dictionary<(Type, Type), int>();

		private static int MenusPerPage = 5;

		private static Dictionary<(Type, int), int> MenuPageCounter = new Dictionary<(Type, int), int>();

		private static Dictionary<Type, List<int>> Pages = new Dictionary<Type, List<int>>();
		private static Dictionary<Type, List<string>> PageNames = new Dictionary<Type, List<string>>();

		internal static Dictionary<(string, Type), Type> MenusToRegister = new Dictionary<(string, Type), Type>();

		private Option<int> PageSelector = null;

		private static int CurrentPage = 0;

		private static Type preferenceSystemMenuType = null;

		internal static void Register(string name, Type type, Type generic)
		{
			if (Main.manager.GetPreference<PreferenceBool>("mergeWithPreferenceSystem") != null && Main.manager.GetPreference<PreferenceBool>("mergeWithPreferenceSystem").Value && Main.preferenceSystemMenuType != null)
			{
				MethodInfo info = ReflectionUtils.GetMethod(Main.preferenceSystemMenuType.MakeGenericType(generic), "RegisterMenu");
				if (info != null)
				{
					info.Invoke(null, new object[] { name, type, generic });
					return;
				}
			}
			if (!RegisteredMenus.ContainsKey((type, generic)))
			{
				if (!Pages.ContainsKey(generic))
					Pages.Add(generic, new List<int>());
				if (!PageNames.ContainsKey(generic))
					PageNames.Add(generic, new List<string>());

				bool foundValidPage = false;
				int page = 0;
				RegisteredMenus.Add((type, generic), name);

				while (!foundValidPage)
				{
					if (MenuPageCounter.ContainsKey((generic, page)))
					{
						int pageCount = MenuPageCounter[(generic, page)];
						if (pageCount < MenusPerPage)
						{
							MenuPageCounter[(generic, page)]++;
							foundValidPage = true;
						}
						else
						{
							page++;
						}
					}
					else
					{
						MenuPageCounter.Add((generic, page), 0);
						MenuPageCounter[(generic, page)]++;
						foundValidPage = true;
						break;
					}
				}

				if (foundValidPage)
				{

					if (!Pages[generic].Contains(page))
					{
						Pages[generic].Add(page);
						PageNames[generic].Add("Page: " + page);
					}
					MenuPages.Add((type, generic), page);
				}
			}
		}

		public static void RegisterMenu(string name, Type type, Type generic)
		{
			if (!MenusToRegister.ContainsKey((name, type))) 
				MenusToRegister.Add((name, type), generic);
		}
		private static void RegisterLegacyMenu(string name, Type type, bool skip_stack)
		{
			RegisterMenu(name, type, type.GetGenericArguments()[0]);
		}

		private bool HasBeenSetup = false;
		public override void Setup(int player_id)
		{
			if (!HasBeenSetup)
			{
				MethodInfo mInfo = ReflectionUtils.GetMethod<ModsPreferencesMenu<T>>("RegisterLegacyMenu");
				EventUtils.InvokeEvent(nameof(Events.PreferenceMenu_MainMenu_SetupEvent), Events.PreferenceMenu_MainMenu_SetupEvent?.GetInvocationList(), null, new PreferenceMenu_SetupArgs(this, mInfo));
				EventUtils.InvokeEvent(nameof(Events.PreferenceMenu_PauseMenu_SetupEvent), Events.PreferenceMenu_PauseMenu_SetupEvent?.GetInvocationList(), null, new PreferenceMenu_SetupArgs(this, mInfo));
				HasBeenSetup = true;
			}
			CurrentPage = 0;
			if (!Pages.ContainsKey(GetType().GetGenericArguments()[0]))
				Pages.Add(GetType().GetGenericArguments()[0], new List<int>());
			if (!PageNames.ContainsKey(GetType().GetGenericArguments()[0]))
				PageNames.Add(GetType().GetGenericArguments()[0], new List<string>());

			PageSelector = new Option<int>(Pages[GetType().GetGenericArguments()[0]], CurrentPage, PageNames[GetType().GetGenericArguments()[0]]);
			PageSelector.OnChanged += delegate (object _, int result)
			{
				CurrentPage = result;
				Redraw(CurrentPage);
			};

			Redraw(CurrentPage);
		}

		private void Redraw(int pageNumber = 0)
		{
			ModuleList.Clear();

			AddLabel("Mod Preferences");
			New<SpacerElement>(true);

			if (Pages[GetType().GetGenericArguments()[0]].Count >= 2)
			{
				AddSelect<int>(PageSelector);
			}

			New<SpacerElement>(true);

			foreach ((Type, Type) menu in RegisteredMenus.Keys)
			{
				if (menu.Item2 == this.GetType().GetGenericArguments()[0])
				{
					if (MenuPages[menu] == pageNumber)
					{
						AddSubmenuButton(RegisteredMenus[menu], menu.Item1, false);
					}
				}
			}

			New<SpacerElement>(true);
			New<SpacerElement>(true);
			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

		public override void CreateSubmenus(ref Dictionary<Type, Menu<T>> menus)
		{
			if (this.GetType().GetGenericArguments()[0] == typeof(MainMenuAction))
				EventUtils.InvokeEvent(nameof(Events.PreferenceMenu_MainMenu_CreateSubmenusEvent), Events.PreferenceMenu_MainMenu_CreateSubmenusEvent?.GetInvocationList(), null, new PreferenceMenu_CreateSubmenusArgs<T>(this, menus, this.Container, this.ModuleList));
			else if (this.GetType().GetGenericArguments()[0] == typeof(PauseMenuAction))
				EventUtils.InvokeEvent(nameof(Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent), Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent?.GetInvocationList(), null, new PreferenceMenu_CreateSubmenusArgs<T>(this, menus, this.Container, this.ModuleList));
		}
	}
}
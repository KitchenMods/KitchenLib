using System;
using System.Linq;
using Kitchen;
using HarmonyLib;
using System.Reflection;
using Kitchen.Modules;
using KitchenData;
using KitchenLib.Utils;
using KitchenLib.Event;
using UnityEngine;
using System.IO;

namespace KitchenLib
{
	[HarmonyPatch(typeof(MainMenuView), "SetupMenus")]
	class StartMainMenu_Patch
	{
		static void Postfix(MainMenuView __instance)
		{	
			MethodInfo setMenu = ReflectionUtils.GetMethod<MainMenuView>("SetMenu");
			setMenu.Invoke(__instance, new object[] { typeof(RevisedMainMenu), false });
		}
	}

	[HarmonyPatch(typeof(MainMenuView), "SetupMenus")]
    class MainMenuView_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(MainMenuView __instance)
        {
            //FieldInfo panel = __instance.GetType().GetField("Panel", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo panel = ReflectionUtils.GetField<LocalMenuView<MainMenuAction>>("Panel");
			PanelElement p = (PanelElement)panel.GetValue(__instance);
            //FieldInfo moduleList = __instance.GetType().GetField("ModuleList", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo moduleList = ReflectionUtils.GetField<LocalMenuView<MainMenuAction>>("ModuleList");
			ModuleList mList = (ModuleList)moduleList.GetValue(__instance);
            p.gameObject.SetActive(false);
            //MethodInfo mInfo = __instance.GetType().GetMethod("AddMenu", BindingFlags.NonPublic | BindingFlags.Instance);
			MethodInfo mInfo = ReflectionUtils.GetMethod<LocalMenuView<MainMenuAction>>("AddMenu");

            MainMenuView_SetupMenusArgs mainMenuViewEvent = new MainMenuView_SetupMenusArgs(__instance, mInfo, mList);
            EventUtils.InvokeEvent(nameof(Events.MainMenuView_SetupMenusEvent), Events.MainMenuView_SetupMenusEvent?.GetInvocationList(), null, mainMenuViewEvent);
            return true;
        }
    }
    
    [HarmonyPatch(typeof(MainMenu), "Setup")]
    class MainMenu_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(StartMainMenu __instance)
        {
            ProfileManager.Main.Load();

            MethodInfo addActionButton = __instance.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(m => m.Name == "AddActionButton" && m.GetParameters().Length == 3);
			
            MethodInfo addSubmenuButton = ReflectionUtils.GetMethod<MainMenu>("AddSubmenuButton");
            MethodInfo addSpacer = ReflectionUtils.GetMethod<MainMenu>("New").MakeGenericMethod(new Type[] { typeof(SpacerElement) });

            MainMenu_SetupArgs startMainMenuEvent = new MainMenu_SetupArgs(__instance, addActionButton, addSubmenuButton, addSpacer);
            EventUtils.InvokeEvent(nameof(Events.MainMenu_SetupEvent), Events.MainMenu_SetupEvent?.GetInvocationList(), null, startMainMenuEvent);
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerPauseView), "SetupMenus")]
    class PlayerPauseView_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(PlayerPauseView __instance)
        {
            //FieldInfo panel = __instance.GetType().GetField("Panel", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo panel = ReflectionUtils.GetField<LocalMenuView<PauseMenuAction>>("Panel");
            PanelElement p = (PanelElement)panel.GetValue(__instance);
            //FieldInfo moduleList = __instance.GetType().GetField("ModuleList", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo moduleList = ReflectionUtils.GetField<LocalMenuView<PauseMenuAction>>("ModuleList");
			ModuleList mList = (ModuleList)moduleList.GetValue(__instance);
            p.gameObject.SetActive(false);
            //MethodInfo mInfo = __instance.GetType().GetMethod("AddMenu", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo mInfo = ReflectionUtils.GetMethod<LocalMenuView<PauseMenuAction>>("AddMenu");

			PlayerPauseView_SetupMenusArgs mainMenuViewEvent = new PlayerPauseView_SetupMenusArgs(__instance, mInfo, mList);
            EventUtils.InvokeEvent(nameof(Events.PlayerPauseView_SetupMenusEvent), Events.PlayerPauseView_SetupMenusEvent?.GetInvocationList(), null, mainMenuViewEvent);
            return true;
        }
    }

	public class DataCollectionMenu : KLMenu<MainMenuAction>
	{
		public DataCollectionMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		public override void Setup(int player_id)
		{
			AddLabel("Are you over 13 years old?");

			AddLabel("Do you permit the collection of your data?");
		}
	}
	public class RevisedMainMenu : KLMenu<MainMenuAction>
	{
		public RevisedMainMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}

		public override void Setup(int player_id)
		{
			ProfileManager.Main.Load();

#if BEPINEX
			string[] newLines;
			if (File.Exists("BepInEx/config/BepInEx.cfg"))
			{
				string[] lines = File.ReadAllLines("BepInEx/config/BepInEx.cfg");

				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Contains("EnableAssemblyCache = true"))
					{
						lines[i] = "EnableAssemblyCache = false";
						newLines = lines;
						AddLabel("BEPINEX WARNING");
						AddInfo("We have detected that you have enabled the BepInEx assembly cache. This is known to cause issues with the game, and is not supported. Please disable it in the BepInEx config or by using the button below, and restart your game.");
						AddButton("Disable BepInEx Cache", delegate
						{
							File.WriteAllLines("BepInEx/config/BepInEx.cfg", lines);
							Main.instance.Log("Disabled BepInEx Caching");
						});
						New<SpacerElement>(true);
						New<SpacerElement>(true);
					}
				}
			}
#endif

			AddSubmenuButton(GameData.Main.GlobalLocalisation["MAIN_MENU_SINGLEPLAYER"], typeof(SingleplayerMainMenu), false);
			AddSubmenuButton(GameData.Main.GlobalLocalisation["MAIN_MENU_MULTIPLAYER"], typeof(MultiplayerMainMenu), false);
			
			AddSubmenuButton("Mods", typeof(ModsMenu<MainMenuAction>), false);
			AddSubmenuButton("Mod Preferences", typeof(ModsPreferencesMenu<MainMenuAction>), false);

			AddSubmenuButton(GameData.Main.GlobalLocalisation["MAIN_MENU_OPTIONS"], typeof(OptionsMenu<MainMenuAction>), false);
			New<SpacerElement>(true);
			New<SpacerElement>(true);
			AddActionButton(GameData.Main.GlobalLocalisation["MAIN_MENU_QUIT"], MainMenuAction.Quit, ElementStyle.MainMenuBack);
		}
	}
}
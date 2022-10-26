using System;
using System.Linq;
using Kitchen;
using HarmonyLib;
using System.Reflection;
using Kitchen.Modules;
using KitchenData;
using KitchenLib.Utils;
using KitchenLib.Event;

namespace KitchenLib
{
    [HarmonyPatch(typeof(StartMainMenu), "Setup")]
    class StartMainMenu_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(StartMainMenu __instance)
        {
            ProfileManager.Main.Load();

            MethodInfo addActionButton = __instance.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(m => m.Name == "AddActionButton" && m.GetParameters().Length == 3);
            MethodInfo addSubmenuButton = ReflectionUtils.GetMethod<StartMainMenu>("AddSubmenuButton");
            MethodInfo addSpacer = ReflectionUtils.GetMethod<StartMainMenu>("New").MakeGenericMethod(new Type[] { typeof(SpacerElement) });

            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_SINGLEPLAYER"], typeof(SingleplayerMainMenu), false });
            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_MULTIPLAYER"], typeof(MultiplayerMainMenu), false });

            StartMainMenu_SetupArgs startMainMenuEvent = new StartMainMenu_SetupArgs(__instance, addActionButton, addSubmenuButton, addSpacer);
            EventUtils.InvokeEvent(nameof(Events.StartMainMenu_SetupEvent), Events.StartMainMenu_SetupEvent?.GetInvocationList(), null, startMainMenuEvent);

            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_OPTIONS"], typeof(OptionsMenu<MainMenuAction>), false });
            addSpacer.Invoke(__instance, new object[] { true });
            addSpacer.Invoke(__instance, new object[] { true });
            addActionButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_QUIT"], MainMenuAction.Quit, ElementStyle.MainMenuBack });
            return false;
        }
    }

    [HarmonyPatch(typeof(MainMenuView), "SetupMenus")]
    class MainMenuView_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(MainMenuView __instance)
        {
            FieldInfo panel = __instance.GetType().GetField("Panel", BindingFlags.NonPublic | BindingFlags.Instance);
            PanelElement p = (PanelElement)panel.GetValue(__instance);
            FieldInfo moduleList = __instance.GetType().GetField("ModuleList", BindingFlags.NonPublic | BindingFlags.Instance);
            ModuleList mList = (ModuleList)moduleList.GetValue(__instance);
            p.gameObject.SetActive(false);
            MethodInfo mInfo = __instance.GetType().GetMethod("AddMenu", BindingFlags.NonPublic | BindingFlags.Instance);

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
        static bool Prefix(MainMenuView __instance)
        {
            FieldInfo panel = __instance.GetType().GetField("Panel", BindingFlags.NonPublic | BindingFlags.Instance);
            PanelElement p = (PanelElement)panel.GetValue(__instance);
            FieldInfo moduleList = __instance.GetType().GetField("ModuleList", BindingFlags.NonPublic | BindingFlags.Instance);
            ModuleList mList = (ModuleList)moduleList.GetValue(__instance);
            p.gameObject.SetActive(false);
            MethodInfo mInfo = __instance.GetType().GetMethod("AddMenu", BindingFlags.NonPublic | BindingFlags.Instance);

            PlayerPauseView_SetupMenusArgs mainMenuViewEvent = new PlayerPauseView_SetupMenusArgs(__instance, mInfo, mList);
            EventUtils.InvokeEvent(nameof(Events.PlayerPauseView_SetupMenusEvent), Events.PlayerPauseView_SetupMenusEvent?.GetInvocationList(), null, mainMenuViewEvent);
            return true;
        }
    }
}
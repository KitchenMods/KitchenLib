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

            StartMainMenuEvent startMainMenuEvent = new StartMainMenuEvent(__instance, addActionButton, addSubmenuButton, addSpacer);

            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_SINGLEPLAYER"], typeof(SingleplayerMainMenu), false });
            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_MULTIPLAYER"], typeof(MultiplayerMainMenu), false });
            addSubmenuButton.Invoke(__instance, new object[] { "Mods", typeof(ModsMenu), false });
            addSubmenuButton.Invoke(__instance, new object[] { "Mod Preferences", typeof(ModsPreferencesMenu), false });
            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_OPTIONS"], typeof(OptionsMenu<MainMenuAction>), false });
            addSpacer.Invoke(__instance, new object[] { true });
            addSpacer.Invoke(__instance, new object[] { true });
            addActionButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_QUIT"], MainMenuAction.Quit, ElementStyle.MainMenuBack });

            EventUtils.InvokeEvent(nameof(Events.StartMainMenuEvent), Events.StartMainMenuEvent?.GetInvocationList(), null, startMainMenuEvent);
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

            MainMenuViewEventArgs mainMenuViewEvent = new MainMenuViewEventArgs(__instance, mInfo);

            mInfo.Invoke(__instance, new object[] { typeof(ModsMenu), new ModsMenu(__instance.ButtonContainer, mList) });
            mInfo.Invoke(__instance, new object[] { typeof(ModsPreferencesMenu), new ModsPreferencesMenu(__instance.ButtonContainer, mList) });
            //mInfo.Invoke(__instance, new object[] { typeof(testmenu), new testmenu(__instance.ButtonContainer, mList) });

            EventUtils.InvokeEvent(nameof(Events.MainMenuViewEvent), Events.MainMenuViewEvent?.GetInvocationList(), null, mainMenuViewEvent);
            return true;
        }
    }
}
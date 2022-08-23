using System;
using System.Linq;
using UnityEngine;
using MelonLoader;
using Kitchen;
using HarmonyLib;
using System.Reflection;
using Kitchen.Modules;
using KitchenLib.Registry;
using System.Collections.Generic;
using KitchenData;

namespace KitchenLib
{
	[HarmonyPatch(typeof(StartMainMenu), "Setup")]
    class StartMainMenu_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(StartMainMenu __instance)
        {
            ProfileManager.Main.Load();
            MethodInfo addSubmenuButton = __instance.GetType().GetMethod("AddSubmenuButton", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo addActionButton = __instance.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(m => m.Name == "AddActionButton" && m.GetParameters().Length == 3);
            MethodInfo addSpacer = __instance.GetType().GetMethod("New", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new Type[] { typeof(SpacerElement) });
            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_SINGLEPLAYER"], typeof(SingleplayerMainMenu), false });
            addSubmenuButton.Invoke(__instance, new object[] { GameData.Main.GlobalLocalisation["MAIN_MENU_MULTIPLAYER"], typeof(MultiplayerMainMenu), false });
            addSubmenuButton.Invoke(__instance, new object[] { "Mods", typeof(ModsMenu), false });
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
            mInfo.Invoke(__instance, new object[] { typeof(ModsMenu), new ModsMenu(__instance.ButtonContainer, mList) });
            return true;
        }
    }
    
    public class ModsMenu : StartGameMainMenu
    {
        public ModsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id)
        {
            AddLabel("Loaded Mods");
            
            foreach (BaseMod mod in ModRegistery.Registered.Values)
            {
                if (mod.ModName != null && mod.ModVersion != null)
                {
                    AddInfo(mod.ModName + "     v" + mod.ModVersion);
                }
            }
            

            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddActionButton("Back", MainMenuAction.Back, ElementStyle.MainMenuBack);

        }

    }
}
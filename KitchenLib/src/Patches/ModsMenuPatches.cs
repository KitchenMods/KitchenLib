using HarmonyLib;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Event;
using KitchenLib.Utils;
using System;
using System.Linq;
using System.Reflection;
using KitchenLib.UI;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(MainMenuView), "SetupMenus")]
	internal class MainMenuViewPatch
	{
		static bool Prefix(MainMenuView __instance)
		{
			FieldInfo panel = ReflectionUtils.GetField<LocalMenuView<MainMenuAction>>("Panel");
			PanelElement p = (PanelElement)panel.GetValue(__instance);
			FieldInfo moduleList = ReflectionUtils.GetField<LocalMenuView<MainMenuAction>>("ModuleList");
			ModuleList mList = (ModuleList)moduleList.GetValue(__instance);
			p.gameObject.SetActive(false);
			MethodInfo mInfo = ReflectionUtils.GetMethod<LocalMenuView<MainMenuAction>>("AddMenu");

			MainMenuView_SetupMenusArgs mainMenuViewEvent = new MainMenuView_SetupMenusArgs(__instance, mInfo, mList);
			EventUtils.InvokeEvent(nameof(Events.MainMenuView_SetupMenusEvent), Events.MainMenuView_SetupMenusEvent?.GetInvocationList(), null, mainMenuViewEvent);
			return true;
		}
		static void Postfix(MainMenuView __instance)
		{
			MethodInfo setMenu = ReflectionUtils.GetMethod<MainMenuView>("SetMenu");
			setMenu.Invoke(__instance, new object[] { typeof(RevisedMainMenu), false });
		}
	}

	[HarmonyPatch(typeof(MainMenu), "Setup")]
	internal class MainMenuPatch
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
	internal class PlayerPauseViewPatch
	{
		[HarmonyPrefix]
		static bool Prefix(PlayerPauseView __instance)
		{
			FieldInfo panel = ReflectionUtils.GetField<LocalMenuView<PauseMenuAction>>("Panel");
			PanelElement p = (PanelElement)panel.GetValue(__instance);
			FieldInfo moduleList = ReflectionUtils.GetField<LocalMenuView<PauseMenuAction>>("ModuleList");
			ModuleList mList = (ModuleList)moduleList.GetValue(__instance);
			p.gameObject.SetActive(false);
			MethodInfo mInfo = ReflectionUtils.GetMethod<LocalMenuView<PauseMenuAction>>("AddMenu");

			PlayerPauseView_SetupMenusArgs mainMenuViewEvent = new PlayerPauseView_SetupMenusArgs(__instance, mInfo, mList);
			EventUtils.InvokeEvent(nameof(Events.PlayerPauseView_SetupMenusEvent), Events.PlayerPauseView_SetupMenusEvent?.GetInvocationList(), null, mainMenuViewEvent);
			return true;
		}
	}
}
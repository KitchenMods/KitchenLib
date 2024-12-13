using HarmonyLib;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Event;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(MainMenuView), "SetupMenus")]
	internal class MainMenuViewPatch
	{
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new(instructions);

			matcher.MatchForward(false, new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldfld), new CodeMatch(OpCodes.Callvirt), new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldtoken))
				.Advance(3)
				.Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MainMenuViewPatch), "CallSetupMenusEvent")))
				.Insert(new CodeInstruction(OpCodes.Ldarg_0)); 
				
			return matcher.InstructionEnumeration();
		}

		static void Postfix(MainMenuView __instance)
		{
			MethodInfo _SetMenu = ReflectionUtils.GetMethod<MainMenuView>("SetMenu");
			_SetMenu.Invoke(__instance, new object[] {ErrorHandling.GetNextMenu(null), false});
		}

		private static void CallSetupMenusEvent(MainMenuView instance)
		{
			FieldInfo moduleList = ReflectionUtils.GetField<LocalMenuView<MenuAction>>("ModuleList");
			ModuleList mList = (ModuleList)moduleList.GetValue(instance);
			MethodInfo mInfo = ReflectionUtils.GetMethod<LocalMenuView<MenuAction>>("AddMenu");
			MainMenuView_SetupMenusArgs mainMenuViewEvent = new MainMenuView_SetupMenusArgs(instance, mInfo, mList);
			EventUtils.InvokeEvent(nameof(Events.MainMenuView_SetupMenusEvent), Events.MainMenuView_SetupMenusEvent?.GetInvocationList(), null, mainMenuViewEvent);
		}
	}

	[HarmonyPatch(typeof(MainMenu), "Setup")]
	internal class MainMenuPatch
	{
		[HarmonyPrefix]
		static bool Prefix(StartMainMenu __instance)
		{
			//ProfileManager.Main.Load();

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
			FieldInfo moduleList = ReflectionUtils.GetField<LocalMenuView<MenuAction>>("ModuleList");
			ModuleList mList = (ModuleList)moduleList.GetValue(__instance);
			MethodInfo mInfo = ReflectionUtils.GetMethod<LocalMenuView<MenuAction>>("AddMenu");

			PlayerPauseView_SetupMenusArgs mainMenuViewEvent = new PlayerPauseView_SetupMenusArgs(__instance, mInfo, mList);
			EventUtils.InvokeEvent(nameof(Events.PlayerPauseView_SetupMenusEvent), Events.PlayerPauseView_SetupMenusEvent?.GetInvocationList(), null, mainMenuViewEvent);
			return true;
		}
	}
}
﻿using System.Reflection;
using Kitchen.Modules;
using Kitchen;
using KitchenLib.Achievements;
using KitchenLib.Preferences;
using KitchenLib.UI.PlateUp;
using KitchenLib.Utils;

namespace KitchenLib.UI
{
	public class RevisedMainMenu
	{
		private static void AddBetaInfo(StartMainMenu instance)
		{
			MethodInfo AddLabel = ReflectionUtils.GetMethod<StartMainMenu>("AddLabel");
			MethodInfo AddInfo = ReflectionUtils.GetMethod<StartMainMenu>("AddInfo");
			MethodInfo New = ReflectionUtils.GetMethod<StartMainMenu>("New").MakeGenericMethod(typeof(SpacerElement));

			if (Main.MOD_BETA_VERSION != "")
			{
				AddLabel.Invoke(instance, ["!!BETA WARNING!!"]);
				AddInfo.Invoke(instance, ["You are running a beta version of KitchenLib"]);
				AddInfo.Invoke(instance, ["(Please backup your save files.)"]);
				AddInfo.Invoke(instance, ["There will be bugs, this is for TESTING PURPOSES"]);
			}
		}

		private static void AddModMenus(StartMainMenu instance)
		{
			MethodInfo New = ReflectionUtils.GetMethod<StartMainMenu>("New").MakeGenericMethod(typeof(SpacerElement));
			MethodInfo AddSubmenuButton = ReflectionUtils.GetMethod<StartMainMenu>("AddSubmenuButton");
			
			New.Invoke(instance, [true]);
			AddSubmenuButton.Invoke(instance, ["Mods", typeof(ModsMenu<MenuAction>), false]);
			if (!Main.manager.GetPreference<PreferenceBool>("mergeWithPreferenceSystem").Value && Main.preferenceSystemMenuType != null || Main.preferenceSystemMenuType == null)
			{
				if (PreferenceManager.Managers.Count > 0)
				{
					AddSubmenuButton.Invoke(instance, ["Mod Preferences", typeof(ModsPreferencesMenu<MenuAction>), false]);
				}
			}

			if (AchievementsManager.Managers.Count > 0)
			{
				AddSubmenuButton.Invoke(instance, ["Mod Achievements", typeof(ModAchievementsMenu<MenuAction>), false]);
			}
		}
	}
}

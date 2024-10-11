using System.Reflection;
using Kitchen.Modules;
using Kitchen;
using KitchenLib.Achievements;
using KitchenLib.Preferences;
using KitchenLib.UI.PlateUp;
using KitchenLib.UI.PlateUp.PreferenceMenus;
using KitchenLib.Utils;

namespace KitchenLib.UI
{
	public class RevisedMainMenu
	{
		private static void AddBetaInfo(StartMainMenu instance)
		{
			MethodInfo AddLabel = ReflectionUtils.GetMethod<StartMainMenu>("AddLabel");

			if (Main.MOD_BETA_VERSION != "")
			{
				AddLabel.Invoke(instance, ["!! KITCHENLIB BETA !!"]);
			}
		}

		private static void AddModMenus(StartMainMenu instance)
		{
			MethodInfo New = ReflectionUtils.GetMethod<StartMainMenu>("New").MakeGenericMethod(typeof(SpacerElement));
			MethodInfo AddSubmenuButton = ReflectionUtils.GetMethod<StartMainMenu>("AddSubmenuButton");
			
			New.Invoke(instance, [true]);
			AddSubmenuButton.Invoke(instance, ["Mods", typeof(ModsMenu), false]);
			if (!Main.manager.GetPreference<PreferenceBool>("mergeWithPreferenceSystem").Value && Main.preferenceSystemMenuType != null || Main.preferenceSystemMenuType == null)
			{
				if (PreferenceManager.Managers.Count > 0)
				{
					//AddSubmenuButton.Invoke(instance, ["Mod Preferences", typeof(ModsPreferencesMenu<MenuAction>), false]);
					AddSubmenuButton.Invoke(instance, ["Mod Preferences", typeof(MainMenuPreferencesesMenu), false]);
				}
			}

			if (AchievementsManager.Managers.Count > 0)
			{
				AddSubmenuButton.Invoke(instance, ["Mod Achievements", typeof(ModAchievementsMenu), false]);
			}
		}
	}
}

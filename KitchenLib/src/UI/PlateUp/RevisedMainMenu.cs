using Kitchen.Modules;
using Kitchen;
using KitchenData;
using UnityEngine;
using KitchenLib.Preferences;
using KitchenLib.UI.PlateUp;

namespace KitchenLib.UI
{
	public class RevisedMainMenu : KLMenu<MainMenuAction>
	{
		public RevisedMainMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}

		public override void Setup(int player_id)
		{
			ProfileManager.Main.Load();
			if (Main.MOD_BETA_VERSION != "")
			{
				AddLabel("!!BETA WARNING!!");
				AddInfo("You are running a beta version of KitchenLib");
				AddInfo("(Please backup your save files.)");
				AddInfo("There will be bugs, this is for TESTING PURPOSES");
				New<SpacerElement>(true);
				New<SpacerElement>(true);
				New<SpacerElement>(true);
			}

			AddSubmenuButton(GameData.Main.GlobalLocalisation["MAIN_MENU_SINGLEPLAYER"], typeof(SingleplayerMainMenu), false);
			AddSubmenuButton(GameData.Main.GlobalLocalisation["MAIN_MENU_MULTIPLAYER"], typeof(MultiplayerMainMenu), false);

			AddSubmenuButton("Mods", typeof(ModsMenu<MainMenuAction>), false);
			if (!Main.manager.GetPreference<PreferenceBool>("mergeWithPreferenceSystem").Value && Main.preferenceSystemMenuType != null || Main.preferenceSystemMenuType == null)
			{
				AddSubmenuButton("Mod Preferences", typeof(ModsPreferencesMenu<MainMenuAction>), false);
			}
			AddSubmenuButton("Mod Achievements", typeof(ModAchievementsMenu<MainMenuAction>), false);

			AddSubmenuButton(GameData.Main.GlobalLocalisation["MAIN_MENU_OPTIONS"], typeof(OptionsMenu<MainMenuAction>), false);
			New<SpacerElement>(true);
			New<SpacerElement>(true);
			AddActionButton(GameData.Main.GlobalLocalisation["MAIN_MENU_QUIT"], MainMenuAction.Quit, ElementStyle.MainMenuBack);
		}
	}
}

using Kitchen.Modules;
using Kitchen;
using KitchenData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

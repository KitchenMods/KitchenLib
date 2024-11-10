using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	public class UserOptions : KLMenu
	{
		public UserOptions(Transform container, ModuleList module_list) : base(container, module_list) { }

		private Option<bool> scrollingMenu = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("enableChangingMenu").Value, new List<string> { "Enabled", "Disabled" });
		private Option<int> cosmeticWidth = new Option<int>(new List<int> { 3, 4, 5, 6, 7, 8, 9, 10 }, Main.manager.GetPreference<PreferenceInt>("cosmeticWidth").Value, new List<string> { "3", "4", "5", "6", "7", "8", "9", "10" });
		private Option<int> cosmeticHeight = new Option<int>(new List<int> { 3, 4, 5, 6, 7, 8, 9, 10 }, Main.manager.GetPreference<PreferenceInt>("cosmeticHeight").Value, new List<string> { "3", "4", "5", "6", "7", "8", "9", "10" });
		private Option<bool> mergeWithPreferenceSystem = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("mergeWithPreferenceSystem").Value, new List<string> { "Enabled", "Disabled" });
		// private Option<bool> steamCloudPreferences = new Option<bool>(new List<bool> { true, false }, BaseMod.globalPreferences.GetPreference<PreferenceBool>("steamCloudPreferences").Value, new List<string> { "Enabled", "Disabled" });
		private Option<int> achievementNotificatonDisplay = new Option<int>(new List<int> { 0, 1, 2 }, Main.manager.GetPreference<PreferenceInt>("achievementNotificatonDisplay").Value, new List<string> { "Disabled", "Order Ticket", "Steam" });

		public override void Setup(int player_id)
		{
			New<SpacerElement>(true);
			AddLabel("Merge with Preference System (Requires Restart)");
			AddSelect(mergeWithPreferenceSystem);
			mergeWithPreferenceSystem.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("mergeWithPreferenceSystem").Set(result);
			};

			New<SpacerElement>(true);

			AddLabel("Cosmetic Selector X");
			AddSelect(cosmeticWidth);
			cosmeticWidth.OnChanged += delegate (object _, int result)
			{
				Main.manager.GetPreference<PreferenceInt>("cosmeticWidth").Set(result);
			};

			New<SpacerElement>(true);

			AddLabel("Cosmetic Selector Y");
			AddSelect(cosmeticHeight);
			cosmeticHeight.OnChanged += delegate (object _, int result)
			{
				Main.manager.GetPreference<PreferenceInt>("cosmeticHeight").Set(result);
			}; 
			
			/*
		New<SpacerElement>(true);

		AddLabel("EXPERIMENTAL : Steam Cloud Preferences (Requires Restart)");
		AddSelect(steamCloudPreferences);
		steamCloudPreferences.OnChanged += delegate (object _, bool result)
		{
			BaseMod.globalPreferences.GetPreference<PreferenceBool>("steamCloudPreferences").Set(result);
			BaseMod.globalPreferences.Save();
		};
		*/
			
			New<SpacerElement>(true);

			AddLabel("Achievement Notification Theme (Requires Restart)");
			AddSelect(achievementNotificatonDisplay);
			achievementNotificatonDisplay.OnChanged += delegate (object _, int result)
			{
				Main.manager.GetPreference<PreferenceInt>("achievementNotificatonDisplay").Set(result);
			};
		

			New<SpacerElement>(true);

			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				Main.manager.Save();
				RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}
	}
}
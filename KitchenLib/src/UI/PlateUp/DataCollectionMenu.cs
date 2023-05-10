using Kitchen.Modules;
using Kitchen;
using System.Collections.Generic;
using UnityEngine;
using KitchenLib.Preferences;

namespace KitchenLib.UI
{
	internal class DataCollectionMenu : KLMenu<MainMenuAction>
	{
		public DataCollectionMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		public override void Setup(int player_id)
		{
			AddLabel("KitchenLib Data Collection");
			AddInfo("KitchenLib would like to collect a small amount of data from your game.");
			AddInfo("This data will be used to improve the mod and to help us understand how players use the mod.");
			AddInfo("This data will not be shared with any third parties.");
			AddInfo("You can opt out of this data collection at any time by disabling the mod in the mod menu.");
			AddInfo("We will be collecting the following data: Hashed Steam ID, Steam Name, Game Version, KitchenLib Version, Last Play, Last Lobby, Resolution");
			AddInfo("This data list is subject to change, and can be found on our GitHub page.");
			New<SpacerElement>(true);

			AddLabel("Are you over 13 years old?");
			AddSelect<bool>(_over_13);
			_over_13.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("over13").Set(result);
			};

			New<SpacerElement>(true);

			AddLabel("Do you permit the collection of this data?");
			AddSelect<bool>(_data_consent);
			_data_consent.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("datacollection").Set(result);
			};

			New<SpacerElement>(true);

			AddButton("Continue", delegate (int i)
			{
				Main.manager.GetPreference<PreferenceBool>("hasrequested").Set(true);
				Main.manager.Save();
				RequestSubMenu(typeof(RevisedMainMenu));
			}, 0, 1f, 0.2f);
		}

		private Option<bool> _over_13 = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("over13").Value, new List<string> { "Yes", "No" });
		private Option<bool> _data_consent = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("datacollection").Value, new List<string> { "Yes", "No" });
	}
}

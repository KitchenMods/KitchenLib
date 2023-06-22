using Kitchen.Modules;
using Kitchen;
using System.Collections.Generic;
using UnityEngine;
using KitchenLib.Preferences;
using static UnityEngine.Rendering.HableCurve;
using System;
using KitchenData;

namespace KitchenLib.UI
{
	internal class PrivacyPolicyUpdate : KLMenu<MainMenuAction>
	{
		public PrivacyPolicyUpdate(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		public override void Setup(int player_id)
		{
			AddLabel("KitchenLib Data Collection");
			AddInfo("As of June 23rd 2023, KitchenLib's Privacy Policy has been updated.");
			AddInfo("Please review the updated policy, and adjust your preferences.");
			New<SpacerElement>(true);

			AddButton("Open Privacy Policy", delegate (int i)
			{
				System.Diagnostics.Process.Start("https://github.com/KitchenMods/KitchenLib/blob/master/PRIVACYPOLICY.MD");
			}, 0, 1f, 0.2f);

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
				Main.manager.GetPreference<PreferenceBool>("newpolicy").Set(true);
				Main.manager.Save();
				RequestSubMenu(typeof(RevisedMainMenu));
			}, 0, 1f, 0.2f);
		}

		private Option<bool> _over_13 = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("over13").Value, new List<string> { "Yes", "No" });
		private Option<bool> _data_consent = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("datacollection").Value, new List<string> { "Yes", "No" });
	}
}

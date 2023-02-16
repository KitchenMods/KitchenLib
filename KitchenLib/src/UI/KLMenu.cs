using System.Collections.Generic;
using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using System.Linq;
using System;

namespace KitchenLib
{
    public class KLMenu<T> : Menu<T>
    {
        public KLMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id) { }
        protected void BoolOption(BoolPreference pref)
        {
			this.Add<bool>(new Option<bool>(new List<bool>
			{
				false,
				true
			}, (bool)pref.Value, new List<string>
			{
				this.Localisation["SETTING_DISABLED"],
				this.Localisation["SETTING_ENABLED"]
			}, null)).OnChanged += delegate(object _, bool f)
			{
				pref.Value = f;
			};
        }
		private string mod_id = "";
		private int CreateNewProfileIndex;
		private PreferenceManager manager;
		protected void AddProfileSelector(string mod_id, Action<string> action, PreferenceManager manager, bool updateOnHighlight = true)
		{
			this.mod_id = mod_id;
			this.manager = manager;
			List<string> profiles = GlobalPreferences.GetProfiles(mod_id).ToList();
			string current_profile = GlobalPreferences.GetProfile(mod_id);
			
			if (profiles.Count > 0)
			{
				if (!profiles.Contains(current_profile))
					current_profile = profiles[0];
			}
			else
			{
				current_profile = "";
			}

			profiles.Add("Create");
			CreateNewProfileIndex = profiles.Count - 1;
			manager.SetProfile(current_profile);
			manager.Load();


			Option<string> options = new Option<string>(
				profiles,
				current_profile,
				profiles);

			SelectElement element = AddSelect<string>(options);
			options.OnChanged += (s, args) =>
			{
				current_profile = args;
			};

			if (updateOnHighlight)
			{

				element.OnOptionChosen += (i) =>
				{
					if (i == CreateNewProfileIndex)
					{
						base.RequestSubMenu(typeof(TextEntryMainMenu), true);
						TextInputView.RequestTextInput(base.Localisation["NEW_PROFILE_PROMPT"], "", 20, new Action<TextInputView.TextInputState, string>(this.CreateNewProfile));
					}
				};
				element.OnOptionHighlighted += (i) =>
				{
					if (current_profile != "Create")
					{
						Main.instance.Log("------------------------ Selected Profile " + current_profile);
						GlobalPreferences.SetProfile(mod_id, current_profile);
						action(current_profile);
						manager.SetProfile(current_profile);
						manager.Load();
					}
					else
					{
						manager.SetProfile();
						manager.Load();
					}

					manager.Save();
				};
			}
			else
			{
				element.OnOptionChosen += (i) =>
				{
					if (i == CreateNewProfileIndex)
					{
						base.RequestSubMenu(typeof(TextEntryMainMenu), true);
						TextInputView.RequestTextInput(base.Localisation["NEW_PROFILE_PROMPT"], "", 20, new Action<TextInputView.TextInputState, string>(this.CreateNewProfile));
					}
					else
					{
						if (current_profile != "Create")
						{
							Main.instance.Log("------------------------ Selected Profile " + current_profile);
							GlobalPreferences.SetProfile(mod_id, current_profile);
							action(current_profile);
							manager.SetProfile(current_profile);
							manager.Load();
						}
						else
						{
							manager.SetProfile();
							manager.Load();
						}

						manager.Save();
					}
				};
			}
		}

		private void CreateNewProfile(TextInputView.TextInputState result, string name)
		{
			if (result == TextInputView.TextInputState.TextEntryComplete)
			{
				if (name != "Create")
				{
					List<string> profiles = GlobalPreferences.GetProfiles(mod_id).ToList();
					if (!profiles.Contains(name))
						GlobalPreferences.AddProfile(mod_id, name);
					GlobalPreferences.SetProfile(mod_id, name);
					manager.SetProfile(name);
					manager.Save();
				}
			}
			base.RequestSubMenu(base.GetType(), true);
		}
    }
}
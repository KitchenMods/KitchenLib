using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KitchenLib.Utils;
using TMPro;
using UnityEngine;

namespace KitchenLib
{
	public class KLMenu : KLMenu<MenuAction>
	{
		internal bool ShouldCloneType;
		internal Type TypeToClone;
		public KLMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}

		public override void Setup(int player_id)
		{
			if (ShouldCloneType)
			{
				MethodInfo methodInfo = ReflectionUtils.GetMethod(TypeToClone, "Setup");
			}
		}
	}

	public class KLMenu<T> : Menu<T>
	{
		private PlayerPauseView view;
		private Transform container;
		private ControlRebindElement rebind;
		public KLMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
			view = container.transform.parent.parent.parent.GetComponent<PlayerPauseView>();
			this.container = container;
		}
		private Dictionary<int, PageDetails> pageActions;
		private Option<int> pageSelector;
		protected Option<int> CreatePageSelector(Dictionary<int, PageDetails> pages)
		{
			pageActions = pages;
			List<int> pageKeys = new List<int>(pageActions.Keys);
			List<string> pageNames = new List<string>();
			foreach (int key in pageKeys)
			{
				pageNames.Add(pageActions[key].title);
			}
			pageSelector = new Option<int>(pageKeys, 0, pageNames);
			
			pageSelector.OnChanged += (object _, int page) =>
			{
				if (pageActions.ContainsKey(page))
				{
					pageActions[page].action?.Invoke();
				}
			};
			
			return pageSelector;
		}
		
		protected void CreateModLabels(Vector2 startingPosition, List<string> modNames, float columnWidth, float rowHeight, int modsPerColumn)
		{
			int counter = 0;
			int row = 0;
			
			Dictionary<int, List<InfoBoxElement>> columns = new Dictionary<int, List<InfoBoxElement>>();

			foreach (var modName in modNames)
			{
				InfoBoxElement infobox = AddInfo(modName);
				TMP_Text text = infobox.gameObject.GetComponentsInChildren<TMP_Text>()[0];
				RectTransform rect = text.gameObject.GetComponent<RectTransform>();
				
				rect.sizeDelta = new Vector2(rect.sizeDelta.x - 0.5f, 1);
				text.alignment = TextAlignmentOptions.Center;
				text.enableAutoSizing = true;
				text.havePropertiesChanged = true;
				text.enableWordWrapping = false;
				text.SetVerticesDirty();
				text.SetLayoutDirty();
				
				if (!columns.ContainsKey(row))
					columns.Add(row, new List<InfoBoxElement>());
				
				columns[row].Add(infobox);
				counter++;
				if (counter >= modsPerColumn)
				{
					counter = 0;
					row++;
				}
			}
			
			foreach (var column in columns)
			{
				float columnCount = column.Value.Count;
				float columnPosition = startingPosition.x - (columnWidth * (columnCount - 1) / 2f);
				float rowPosition = startingPosition.y - (rowHeight * column.Key);
				for (int i = 0; i < columnCount; i++)
				{
					column.Value[i].Position = new Vector2(columnPosition + (i * columnWidth), rowPosition);
				}
			}
		}
		
		protected void ResetPanel()
		{
			if (view == null) return;
			MethodInfo setparneltarget = ReflectionUtils.GetMethod<PlayerPauseView>("SetPanelTarget");
			container.transform.parent.parent.parent.localPosition = -ModuleList.BoundingBox.center;
			setparneltarget.Invoke(view, new object[] { RequiresBackingPanel ? ModuleList : null });
		}

		protected ControlRebindElement GetRebindElement()
		{
			if (rebind != null)
				return rebind;
			
			FieldInfo rebindField = ReflectionUtils.GetField<PlayerPauseView>("Rebind");
			rebind = (ControlRebindElement)rebindField.GetValue(view);
			return rebind;
		}

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
			}, null)).OnChanged += delegate (object _, bool f)
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
						Main.LogInfo("------------------------ Selected Profile " + current_profile);
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
							Main.LogInfo("------------------------ Selected Profile " + current_profile);
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
					manager.Load();
					manager.Save();
				}
			}
			base.RequestSubMenu(base.GetType(), true);
		}
	}
	
	public struct PageDetails
	{
		public Action action;
		public string title;
	}
}
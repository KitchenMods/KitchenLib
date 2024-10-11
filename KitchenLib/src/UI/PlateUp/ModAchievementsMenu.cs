using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Achievements;
using KitchenMods;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	public class ModAchievementsMenu : KLMenu
	{
		public ModAchievementsMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}

		public Option<AchievementsManager> managers = null;
		public Dictionary<AchievementsManager, Option<int>> pages = new Dictionary<AchievementsManager, Option<int>>();
		public Dictionary<AchievementsManager, List<Achievement>> achievements = new Dictionary<AchievementsManager, List<Achievement>>();
		
		private int maxPerPage = 6;
		public override void Setup(int player_id)
		{
			pages.Clear();
			achievements.Clear();
			if (AchievementsManager.Managers.Count < 0)
			{
				AddLabel("Mod Achievements");
				AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate(int i)
				{
					this.RequestPreviousMenu();
				}, 0, 1f, 0.2f);
				return;
			}

			List<AchievementsManager> _managers = new List<AchievementsManager>();
			List<string> _managerNames = new List<string>();
			foreach (AchievementsManager manager in AchievementsManager.Managers)
			{
				_managers.Add(manager);
				_managerNames.Add(manager.DisplayName);
			}
			managers = new Option<AchievementsManager>(_managers, _managers[0], _managerNames);
			managers.OnChanged += (object _, AchievementsManager manager) =>
			{
				pages[manager].SetChosen(0);
				Redraw(manager);
			};

			foreach (AchievementsManager manager in _managers)
			{
				List<Achievement> _achievements = manager.GetAchievements();
				achievements.Add(manager, _achievements);
				List<int> pageKeys = new List<int>();
				List<string> pageNames = new List<string>();
				for (int i = 0; i < Mathf.Ceil(_achievements.Count / (float)maxPerPage); i++)
				{
					pageKeys.Add(i);
					pageNames.Add($"Page {i + 1}");
				}
				pages.Add(manager, new Option<int>(pageKeys, 0, pageNames));
				pages[manager].OnChanged += (object _, int page) =>
				{
					Redraw(manager, page, true);
				};
			}
			
			Redraw(_managers[0]);
		}

		public void Redraw(AchievementsManager manager, int page = 0, bool selectPages = false)
		{
			ModuleList.Clear();
			AddSelect(managers);
			if (pages[manager].Names.Count < 1)
			{
				SelectElement pagesElement = AddSelect(pages[manager]);
				if (selectPages) ModuleList.Select(pagesElement);
			}

			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate(int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);

			if (Main.debugLogging)
			{
				AddButton("DEBUG Reset Achievements", delegate(int i)
				{
					foreach (Achievement achievement in achievements[manager])
					{
						achievement.manager.ReverseAchievement(achievement.Key);
					}
					this.RequestPreviousMenu();
				}, 0, 1f, 0.2f);
			}
			
			
			List<Achievement> _achievements = achievements[manager];
			for (int i = page * maxPerPage; i < Mathf.Min(_achievements.Count, (page + 1) * maxPerPage); i++)
			{
				Achievement achievement = _achievements[i];
				if (achievement.IsUnlocked())
				{
					CreateAchievementModule(achievement.UnlockDateString, achievement.Name, achievement.Description, achievement.Icon);
				}
				else
				{
					CreateLockedAchievementModule();
				}
			}
		}

		public void CreateAchievementModule(string unlockDate, string title, string description, Texture2D icon)
		{
			AchivementElement e = ModuleDirectory.Add<AchivementElement>(Container, Vector2.zero);
			e.SetUnlockDate(unlockDate);
			e.SetTitle(title);
			e.SetDescription(description);
			e.SetStyle(Style);
			e.SetIcon(icon);
			e.SetUnlocked(string.IsNullOrEmpty(unlockDate));
			e.SetSelectable(false);
			ModuleList.AddModule(e);
		}
		
		public void CreateLockedAchievementModule()
		{
			AchivementElement e = ModuleDirectory.Add<AchivementElement>(Container, Vector2.zero);
			e.SetUnlockDate("");
			e.SetTitle("????");
			e.SetDescription("Details for this achievement will be revealed once unlocked. ");
			e.SetStyle(Style);
			e.SetIcon(null);
			e.SetUnlocked(false);
			e.SetSelectable(false);
			ModuleList.AddModule(e);
		}
	}
}

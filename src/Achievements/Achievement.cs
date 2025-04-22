using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace KitchenLib.Achievements
{
	public class SavedAchievement
	{
		public string Key;
		public bool HasCompleted;
		public long UnlockDate;
	}

	public enum AchievementHiddenState
	{
		NotHidden,
		HiddenUntilUnlocked,
		HiddenUntilCompleted
	}

	public class Achievement
	{
		public string Key { get; }
		public bool HasCompleted { get; internal set; }
		public long UnlockDate { get; internal set; }

		[JsonIgnore] public string Name;
		[JsonIgnore] public string Description;
		[JsonIgnore] public Texture2D Icon;
		[JsonIgnore] public List<string> RequiredCompletedAchievements = new List<string>();
		[Obsolete("Please use HiddenState")][JsonIgnore] public bool IsHidden;
		[JsonIgnore] public AchievementHiddenState HiddenState;
		[JsonIgnore] public Action OnUnlock;
		[JsonIgnore] public string UnlockDateString { get; internal set; }
		[JsonIgnore] internal AchievementsManager manager;
		
		public Achievement(string key, string name, string description, Texture2D icon)
		{
			Key = key;
			Name = name;
			Description = description;
			Icon = icon;
			if (Icon == null)
			{
				Icon = Main.bundle.LoadAsset<Texture2D>("defaultAchievementIcon");
			}
		}
		
		public Achievement(string key, string name, string description, Texture2D icon, List<string> requiredCompletedAchievements)
		{
			Key = key;
			Name = name;
			Description = description;
			Icon = icon;
			RequiredCompletedAchievements = requiredCompletedAchievements;
		}

		public Achievement SetName(string value)
		{
			Name = value;
			return this;
		}

		public Achievement SetDescription(string value)
		{
			Description = value;
			return this;
		}

		public Achievement SetIcon(Texture2D value)
		{
			Icon = value;
			return this;
		}

		public Achievement SetRequirements(List<string> value)
		{
			RequiredCompletedAchievements = value;
			return this;
		}

		public Achievement SetHidden(bool value)
		{
			if (value)
			{
				HiddenState = AchievementHiddenState.HiddenUntilUnlocked;
			}
			else
			{
				HiddenState = AchievementHiddenState.NotHidden;
			}

			return this;
		}

		public Achievement SetHiddenState(AchievementHiddenState value)
		{
			HiddenState = value;

			return this;
		}

		public Achievement AddAction(Action value)
		{
			OnUnlock += value;
			return this;
		}

		public bool IsUnlocked()
		{
			bool result = true;
			
			foreach (string requiredCompletedAchievements in RequiredCompletedAchievements)
			{
				if (manager.TryGetAchievement(requiredCompletedAchievements, out Achievement achievement))
				{
					if (achievement.HasCompleted)
					{
						continue;
					}

					result = false;
					break;
				}

				result = false;
			}
			
			return result;
		}

		public bool CanComplete()
		{
			return !HasCompleted && IsUnlocked();
		}
	}
}
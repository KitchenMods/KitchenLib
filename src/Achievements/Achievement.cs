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
	public class Achievement
	{
		public string Key { get; }
		public bool HasCompleted { get; internal set; }
		public long UnlockDate { get; internal set; }

		[JsonIgnore] public string Name;
		[JsonIgnore] public string Description;
		[JsonIgnore] public Texture2D Icon;
		[JsonIgnore] public List<string> RequiredCompletedAchievements = new List<string>();
		[JsonIgnore] public string UnlockDateString { get; internal set; }
		[JsonIgnore] internal AchievementsManager manager;
		
		public Achievement(string key, string name, string description, Texture2D icon)
		{
			Key = key;
			Name = name;
			Description = description;
			Icon = icon;
		}
		
		public Achievement(string key, string name, string description, Texture2D icon, List<string> requiredCompletedAchievements)
		{
			Key = key;
			Name = name;
			Description = description;
			Icon = icon;
			RequiredCompletedAchievements = requiredCompletedAchievements;
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
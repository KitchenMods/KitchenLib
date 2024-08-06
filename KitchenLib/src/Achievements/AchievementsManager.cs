using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using KitchenLib.Systems;
using KitchenLib.Utils;
using Newtonsoft.Json;
using Shapes;
using TMPro;
using UnityEngine;

namespace KitchenLib.Achievements
{
	public class AchievementsManager
	{
		private readonly string ACHIEVEMENT_FOLDER_PATH = Application.persistentDataPath + "/ModData/KitchenLib/Achievements";
		private string achievementFilePath = "";
		private string fileType = ".json";
		
		internal static readonly List<AchievementsManager> Managers = new List<AchievementsManager>();

		private readonly string modId;
		public readonly string DisplayName;
		private readonly Dictionary<string, Achievement> achievements = new();

		public AchievementsManager(string modId, string displayName)
		{
			this.modId = modId;
			DisplayName = displayName;
			Setup();
		}
		
		#region api
		
		public static AchievementsManager GetManager(string modId)
		{
			return Managers.FirstOrDefault(manager => manager.modId == modId);
		}
		
		public List<Achievement> GetAchievements()
		{
			return achievements.Values.ToList();
		}

		public void UnlockAchievement(string key)
		{
			AchievementUnlockSystem.Instance.UnlockAchievement(modId, key);
		}

		public Achievement GetAchievement(string key)
		{
			if (achievements.ContainsKey(key))
				return achievements[key];
			return null;
		}
		
		public bool IsUnlocked(string key)
		{
			return achievements.ContainsKey(key) && achievements[key].IsUnlocked();
		}
		
		public bool TryGetAchievement(string key, out Achievement achievement)
		{
			if (achievements.ContainsKey(key))
			{
				achievement = achievements[key];
				return true;
			}
			achievement = null;
			return false;
		}
		
		public void Load()
		{
			try
			{
				string json = "";
				if (File.Exists(achievementFilePath))
					json = File.ReadAllText(achievementFilePath);
				
				if (string.IsNullOrEmpty(json))
				{
					Main.LogWarning($"Unable to load achievements for {achievementFilePath}, file empty or not saved.");
					return;
				}
				
				List<SavedAchievement> loadedAchievements = JsonConvert.DeserializeObject<List<SavedAchievement>>(json);
				
				foreach (SavedAchievement achievement in loadedAchievements)
				{
					if (!achievements.ContainsKey(achievement.Key))
					{
						Main.LogWarning($"Unable to load {achievement.Key}, key not registered.");
						continue;
					}
					
					achievements[achievement.Key].HasCompleted = achievement.HasCompleted;
					achievements[achievement.Key].UnlockDate = achievement.UnlockDate;
					if (achievements[achievement.Key].UnlockDate > 0)
						achievements[achievement.Key].UnlockDateString = DateTimeOffset.FromUnixTimeMilliseconds(achievement.UnlockDate + (long)TimeZoneInfo.Local.BaseUtcOffset.TotalMilliseconds).DateTime.ToString("dd/MM/yyyy");
				}
			}
			catch (Exception e)
			{
				if (File.Exists(achievementFilePath))
				{
					Main.LogWarning(e.Message);
					Main.LogWarning($"Failed to load achievements file {achievementFilePath} for {modId}, backing up and replacing.");
					File.Move(achievementFilePath, achievementFilePath + ".backup");
					Save();
					Load();
				}
			}
		}

		public void Save()
		{
			string json = JsonConvert.SerializeObject(achievements.Values.ToList(), Formatting.Indented);
			File.WriteAllText(achievementFilePath, json);
		}
		
		public Achievement RegisterAchievement(Achievement achievement)
		{
			if (achievements.ContainsKey(achievement.Key))
			{
				Main.LogWarning($"Unable to register {achievement.Key}, key already registered.");
				return null;
			}
			achievement.manager = this;
			achievements.Add(achievement.Key, achievement);
			return achievement;
		}
		
		#endregion
        
		internal void ChangeFileType(string newFileType)
		{
			fileType = newFileType;
	        
			string oldPath = achievementFilePath;
			achievementFilePath = $"{ACHIEVEMENT_FOLDER_PATH}/{modId}{fileType}";
	        
			if (File.Exists(oldPath) && !File.Exists(achievementFilePath))
				File.Copy(oldPath, achievementFilePath, true);

			Load();
		}
		
		private void Setup()
		{
			if (!Directory.Exists($"{ACHIEVEMENT_FOLDER_PATH}"))
				Directory.CreateDirectory($"{ACHIEVEMENT_FOLDER_PATH}");

			if (PreferenceManager.globalManager != null && PreferenceManager.globalManager.GetPreference<PreferenceInt>("steamCloud").Value == 2)
				fileType = ".plateupsave";

			achievementFilePath = $"{ACHIEVEMENT_FOLDER_PATH}/{this.modId}{fileType}";
			
			Managers.Add(this);
		}
		
		internal bool CompleteAchievement(string key)
		{
			if (achievements.ContainsKey(key) && achievements[key].CanComplete())
			{
				achievements[key].HasCompleted = true;
				achievements[key].UnlockDate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
				achievements[key].UnlockDateString = DateTimeOffset.FromUnixTimeMilliseconds(achievements[key].UnlockDate + (long)TimeZoneInfo.Local.BaseUtcOffset.TotalMilliseconds).DateTime.ToString("dd/MM/yyyy");
				Save();
				return true;
			}
			return false;
		}
		
		internal bool ReverseAchievement(string key)
		{
			if (achievements.ContainsKey(key))
			{
				achievements[key].HasCompleted = false;
				achievements[key].UnlockDate = 0;
				achievements[key].UnlockDateString = "";
				Save();
				return true;
			}
			return false;
		}

		internal static void SetupMenuElement()
		{
			FieldInfo DefaultModules = ReflectionUtils.GetField(typeof(ModuleDirectory), "DefaultModule");
			Dictionary<Type, Element> defaultModules = (Dictionary<Type, Element>)DefaultModules.GetValue(ModuleDirectory.Main);
				
			ButtonElement defaultButton = defaultModules[typeof(ButtonElement)] as ButtonElement;
				
			GameObject label = Main.bundle.LoadAsset<GameObject>("Achievement").AssignMaterialsByNames(); 
				
			AchivementElement element = label.AddComponent<AchivementElement>();
				
			Rectangle r1 = label.GetChild("Backing").AddComponent<Rectangle>();
			Rectangle r2 = label.GetChild("Mouse Backing").AddComponent<Rectangle>();

			r1.Width = 4.4f;
			r1.Height = 1f;
			r1.Type = Rectangle.RectangleType.RoundedSolid;
			r1.CornerRadiii = new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
			r1.Thickness = 0.04f;
			r1.Color = new Color(Color.black.r, Color.black.g, Color.black.b, 0.75f);

			r2.Width = 4.4f;
			r2.Height = 1f;
			r2.Type = Rectangle.RectangleType.RoundedHollow;
			r2.CornerRadiii = new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
			r2.Thickness = 0.04f;
			r2.Color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.75f);
				

			element.BackingBorder = r1;
			element.MouseBackingBorder = r2;
			element.UnlockDate = label.GetChild("Unlock Date").GetComponent<TextMeshPro>();
			element.Title = label.GetChild("Title").GetComponent<TextMeshPro>();
			element.Description = label.GetChild("Description").GetComponent<TextMeshPro>();
			element.Icon = label.GetChild("Icon").GetComponent<MeshRenderer>();
				
			label.GetComponent<Animator>().runtimeAnimatorController = defaultButton.GetComponent<Animator>().runtimeAnimatorController;
				
			FieldInfo GenericAnimator = ReflectionUtils.GetField(typeof(Element), "GenericAnimator");
			GenericAnimator.SetValue(element, label.GetComponent<Animator>());
				
			defaultModules.Add(typeof(AchivementElement), element);
			DefaultModules.SetValue(ModuleDirectory.Main, defaultModules);
		}
	}
}
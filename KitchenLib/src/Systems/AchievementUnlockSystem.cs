using Kitchen;
using KitchenLib.Achievements;
using KitchenLib.Components;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class AchievementUnlockSystem : GameSystemBase, IModSystem
	{
		internal static AchievementUnlockSystem Instance;
		protected override void OnUpdate()
		{
			if (Instance == null) Instance = this;
			return;
			if (Input.GetKeyDown(KeyCode.F))
			{
				AchievementsManager.GetManager("kitchenlib").UnlockAchievement("test");
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				AchievementsManager.GetManager("kitchenlib").UnlockAchievement("test2");
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				AchievementsManager.GetManager("kitchenlib").UnlockAchievement("test3");
			}
		}

		public void UnlockAchievement(string modid, string key)
		{
			Entity entity = EntityManager.CreateEntity([
				typeof(CPosition)
			]);
			EntityManager.AddComponentData(entity, new CRequiresView
			{
				Type = (ViewType)VariousUtils.GetID("KitchenLib.Views.ModAchievementDistributionView")
			});
			EntityManager.AddComponentData(entity, new CRequestAchievementUnlock
			{
				modId = modid,
				achivementKey = key
			});
		}
	}
}
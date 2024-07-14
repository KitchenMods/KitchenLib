using Kitchen;
using KitchenLib.Components;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class unlockachievement : GameSystemBase, IModSystem
	{
		
		private EntityQuery _notificationManager;
		protected override void Initialise()
		{
			base.Initialise();
			_notificationManager = GetEntityQuery(typeof(SAchievementDisplayView.Marker));
		}

		protected override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				UnlockAchievement("kitchenlib", "test");
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				UnlockAchievement("kitchenlib", "test2");
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				UnlockAchievement("kitchenlib", "test3");
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
using Kitchen;
using KitchenLib.Components;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Entities;

namespace KitchenLib.Systems
{
	/*
	 * This system is designed to trigger achievement unlocks
	 * This system static and accessible anywhere
	 */
	public class AchievementUnlockSystem : GameSystemBase, IModSystem
	{
		internal static AchievementUnlockSystem Instance;
		protected override void OnUpdate()
		{
			Instance ??= this;
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
using Kitchen;
using KitchenLib.Components;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class RequestAchievementTickets : GameSystemBase, IModSystem
	{
		private EntityQuery _notificationManager;
		private EntityQuery _notifications;
		protected override void Initialise()
		{
			base.Initialise();
			_notificationManager = GetEntityQuery(typeof(SAchievementDisplayView.Marker));
			_notifications = GetEntityQuery(typeof(CRequestAchievementUnlock));
		}

		protected override void OnUpdate()
		{
			using NativeArray<Entity> entities = _notifications.ToEntityArray(Allocator.Temp);
			
			if (entities.Length == 0) return;
			
			Entity notificationManager = _notificationManager.GetSingletonEntity();
			DynamicBuffer<SAchievementDisplayView> notifications = EntityManager.GetBuffer<SAchievementDisplayView>(notificationManager);
			
			
			foreach (Entity entity in entities)
			{
				if (Require(entity, out CRequestAchievementUnlock cRequestAchievementUnlock))
				{
					notifications.Add(new SAchievementDisplayView
					{
						modId = cRequestAchievementUnlock.modId,
						achivementKey = cRequestAchievementUnlock.achivementKey
					});
				}
			}
		}
	}
}
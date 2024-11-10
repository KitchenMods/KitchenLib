using Kitchen;
using KitchenLib.Components;
using KitchenMods;
using Unity.Entities;

namespace KitchenLib.Systems
{
	
	[UpdateInGroup(typeof(ViewSystemsGroup), OrderFirst = true)]
	public class CleanAchievementEvents : GenericSystemBase, IModSystem
	{
		private EntityQuery ViewedEvents;
	
		protected override void Initialise()
		{
			ViewedEvents = GetEntityQuery(new EntityQueryDesc[] { new QueryHelper().Any(new ComponentType[]
			{
				typeof(CRequestAchievementUnlock)
			}).All(new ComponentType[] { typeof(CLinkedView) }) });
		}

		protected override void OnUpdate()
		{
			EntityManager.DestroyEntity(ViewedEvents);
		}
	}
	
}
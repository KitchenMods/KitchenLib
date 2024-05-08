using Kitchen;
using KitchenLib.Components;
using Unity.Entities;

namespace KitchenLib.Systems
{
	[UpdateBefore(typeof(AddNewViews))]
	[UpdateInGroup(typeof(ViewSystemsGroup))]
	public class CleanAchievementEvents : GenericSystemBase
	{
		protected override void Initialise()
		{
			this.ViewedEvents = base.GetEntityQuery(new EntityQueryDesc[] { new QueryHelper().Any(new ComponentType[]
			{
				typeof(CRequestAchievementUnlock)
			}).All(new ComponentType[] { typeof(CLinkedView) }) });
		}

		protected override void OnUpdate()
		{
			base.EntityManager.DestroyEntity(this.ViewedEvents);
		}

		private EntityQuery ViewedEvents;
	}
}

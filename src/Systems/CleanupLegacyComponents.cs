using Kitchen;
using KitchenLib.IMMS;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{
	/*
	 * This system is designed to cleanup and remove any entities with legacy components
	 */
	public class CleanupLegacyComponents : GameSystemBase, IModSystem
	{
		private EntityQuery query;
		
		// Initialise a list of legacy components
		protected override void Initialise()
		{
			query = GetEntityQuery(new QueryHelper().All(
				typeof(CViewHolder),
				typeof(CCommandView),
				typeof(CInfoView),
				typeof(CSendToClientView),
				typeof(CTileHightlighterView),
				typeof(CClientEquipCapeView),
				typeof(CSyncModsView),
				typeof(SIMMSManager)
				));
		}

		protected override void OnUpdate()
		{
			NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);
			using (entities)
			{
				for (int i = 0; i < entities.Length; i++)
				{
					EntityManager.DestroyEntity(entities[i]);
				}
			}
			entities.Dispose();
		}
	}
}
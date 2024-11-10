using Kitchen;
using KitchenLib.IMMS;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class ComponentCleanup : GameSystemBase, IModSystem
	{
		private EntityQuery query;
		
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
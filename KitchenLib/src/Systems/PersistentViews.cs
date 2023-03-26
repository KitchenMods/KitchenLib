using Kitchen;
using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class PersistentViews : RestaurantSystem, IModSystem
	{
		protected override void Initialise()
		{
			query = GetEntityQuery(typeof(CViewHolder));
		}
		protected override void OnUpdate()
		{
			using (NativeArray<Entity> nativeArray = query.ToEntityArray(Allocator.Temp))
			{
				if (nativeArray.Length < 1)
				{
					Entity entity = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition), typeof(CViewHolder));
					EntityManager.SetComponentData(entity, new CCreateAppliance { ID = Main.ViewHolderAppliance.ID });
					EntityManager.SetComponentData(entity, new CPosition());
				}
				else if (nativeArray.Length > 1)
				{
					Main.LogError($"{nativeArray.Length} CViewHolders in scene. There should only be one.");
				}
			}
		}
		private EntityQuery query;
	}
	public struct CViewHolder : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct TEST : IApplianceProperty, IAttachableProperty, IComponentData { }
}
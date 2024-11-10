using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{

	[UpdateAfter(typeof(CreateOffice))]
	public class RefreshDishUpgrades : GenericSystemBase, IModSystem
	{
		private EntityQuery DishUpgrades;
		private EntityQuery DishOptions;
		public static bool Refresh;
		protected override void Initialise()
		{
			DishUpgrades = GetEntityQuery(typeof(CDishUpgrade));
			DishOptions = GetEntityQuery(typeof(CDishChoice));
		}
		protected override void OnUpdate()
		{
		//		
			NativeArray<Entity> dishUpgrades = DishUpgrades.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> dishOptions = DishOptions.ToEntityArray(Allocator.Temp);
			if (Refresh)
			{
				Refresh = false;

				dishUpgrades.ShuffleInPlace();
				for (int i = 0; i < dishOptions.Length; i++)
				{
					int x = EntityManager.GetComponentData<CDishUpgrade>(dishUpgrades[i]).DishID;
					EntityManager.SetComponentData<CDishChoice>(dishOptions[i], new CDishChoice { Dish = x });
				}
			}
			dishUpgrades.Dispose();
			dishOptions.Dispose();
		}
	}
}

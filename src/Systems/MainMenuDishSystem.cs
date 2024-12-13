using Kitchen;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{
	[UpdateBefore(typeof(GrantUpgrades))]
	public class MainMenuDishSystem : FranchiseFirstFrameSystem, IModSystem
	{
		public static List<int> MenuOptions = new List<int>();
		private EntityQuery EntitiesToActOn;
		protected override void Initialise()
		{
			base.Initialise();
			EntitiesToActOn = GetEntityQuery(typeof(CUpgrade));
		}
		protected override void OnUpdate()
		{
			foreach (int id in MenuOptions)
			{
				bool found = false;
				using var ents = EntitiesToActOn.ToEntityArray(Allocator.Temp);

				foreach (var ent in ents)
				{
					CUpgrade cUpgrade = EntityManager.GetComponentData<CUpgrade>(ent);
					if (cUpgrade.ID == id)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					Entity e = EntityManager.CreateEntity(typeof(CUpgrade), typeof(CPersistThroughSceneChanges));
					EntityManager.SetComponentData<CUpgrade>(e, new CUpgrade { ID = id, IsFromLevel = false });
				}
			}
		}
	}

	[UpdateBefore(typeof(CreateDishOptions))]
	[UpdateAfter(typeof(GrantUpgrades))]
	public class MainMenuDishDebugSystem : FranchiseFirstFrameSystem, IModSystem
	{
		public static List<int> MenuOptions = new List<int>();
		private EntityQuery EntitiesToActOn;
		protected override void Initialise()
		{
			base.Initialise();
			EntitiesToActOn = GetEntityQuery(typeof(CDishUpgrade));
		}
		protected override void OnUpdate()
		{
			foreach (int id in MenuOptions)
			{
				bool found = false;
				using var ents = EntitiesToActOn.ToEntityArray(Allocator.Temp);

				foreach (var ent in ents)
				{
					CDishUpgrade cUpgrade = EntityManager.GetComponentData<CDishUpgrade>(ent);
					if (cUpgrade.DishID == id)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					Entity e = EntityManager.CreateEntity(typeof(CDishUpgrade), typeof(CPersistThroughSceneChanges));
					EntityManager.SetComponentData<CDishUpgrade>(e, new CDishUpgrade { DishID = id });
				}
			}
		}
	}
}

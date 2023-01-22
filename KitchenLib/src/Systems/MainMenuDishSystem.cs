using Kitchen;
using KitchenData;
using KitchenLib.References;
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
			EntitiesToActOn = GetEntityQuery(typeof(CUpgrade));
		}
		protected override void OnUpdate()
		{//-1355749467
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
					Entity e = EntityManager.CreateEntity(typeof(CUpgrade));
					EntityManager.SetComponentData<CUpgrade>(e, new CUpgrade { ID = id, IsFromLevel = false});
				}
			}
		}
	}
}

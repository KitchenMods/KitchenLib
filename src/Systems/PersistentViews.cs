using Kitchen;
using KitchenData;
using KitchenMods;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class PersistentViews : RestaurantSystem, IModSystem
	{
		protected override void Initialise()
		{
			syncModsView = GetEntityQuery(typeof(CSyncModsView));
		}
		protected override void OnUpdate()
		{
			// EnsureView(syncModsView.ToEntityArray(Allocator.Temp), Main.SyncModsViewHolder.ID, typeof(CSyncModsView));
		}

		private void EnsureView(NativeArray<Entity> entities, int ApplianceID, Type marker)
		{
			using (entities)
			{
				if (entities.Length < 1)
				{
					Entity entity = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition), typeof(CDoNotPersist), marker);
					EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceID });
					EntityManager.SetComponentData(entity, new CPosition(new Vector3(75, 0, 75)));
				}
				else
				{
					for (int i = 1; i < entities.Length; i++)
					{
						EntityManager.DestroyEntity(entities[i]);
					}
				}
			}
		}
		private EntityQuery syncModsView;
	}
	/*
	 * As of 23/JUNE/2023
	 * KitchenLib has removed it's secret "Prank Menu" although these views are still here in an attempt to keep player's save files safe.
	 * The "Prank Menu" is nolonger in KitchenLib, and the functionality is no longer present.
	 */
	[Obsolete("No longer used")]
	public struct CViewHolder : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CCommandView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CInfoView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CSendToClientView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CTileHightlighterView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CClientEquipCapeView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CSyncModsView : IApplianceProperty, IAttachableProperty, IComponentData { }
}
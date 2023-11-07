using Kitchen;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;
using System.Collections.Generic;
using KitchenLib.Components;

namespace KitchenLib.Views
{
	public class SyncMods : UpdatableObjectView<SyncMods.ViewData>
	{
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			private EntityQuery Views;
			protected override void Initialise()
			{
				base.Initialise();
				Views = GetEntityQuery(new QueryHelper().All(typeof(SModSync), typeof(CLinkedView)));
			}
			protected override void OnUpdate()
			{
				NativeArray<CLinkedView> linkedViews = Views.ToComponentDataArray<CLinkedView>(Allocator.TempJob);
				foreach (CLinkedView linkedView in linkedViews)
				{
					List<ulong> mods = new List<ulong>();
					foreach (Mod mod in ModPreload.Mods)
					{
						mods.Add(mod.ID);
					}
					SendUpdate(linkedView.Identifier, new ViewData
					{
						Mods = mods
					});
				}
				linkedViews.Dispose();
			}
		}
		[MessagePackObject(false)]
		public struct ViewData : IViewData, IViewData.ICheckForChanges<ViewData>
		{
			[Key(0)] public List<ulong> Mods;
			public bool IsChangedFrom(ViewData cached)
			{
				return Mods.Count != cached.Mods.Count;
			}
		}
		public static List<ulong> MissingMods = new List<ulong>();
		public static List<ulong> AllMods = new List<ulong>();
		protected override void UpdateData(ViewData view_data)
		{
			MissingMods.Clear();
			AllMods.Clear();
			List<ulong> localMods = new List<ulong>();
			foreach (Mod mod in ModPreload.Mods)
			{
				localMods.Add(mod.ID);
			}
			foreach (ulong mod in view_data.Mods)
			{
				AllMods.Add(mod);
				if (!localMods.Contains(mod))
				{
					MissingMods.Add(mod);
				}
			}
		}
	}
}
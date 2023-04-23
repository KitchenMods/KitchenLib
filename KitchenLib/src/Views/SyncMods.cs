using Kitchen;
using KitchenLib.Systems;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;
using System.Collections.Generic;

namespace KitchenLib.Views
{
	internal class SyncMods : UpdatableObjectView<SyncMods.ViewData>
	{
		#region ECS View System (Runs on host and updates views to be broadcasted to clients)
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			private EntityQuery Views;

			protected override void Initialise()
			{
				base.Initialise();

				Views = GetEntityQuery(new QueryHelper().All(typeof(CSyncModsView), typeof(CLinkedView)));
			}

			protected override void OnUpdate()
			{
				using var entities = Views.ToEntityArray(Allocator.Temp);
				using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				

				for (var i = 0; i < views.Length; i++)
				{
					var view = views[i];

					List<ulong> mods = new List<ulong>();

					foreach (Mod mod in ModPreload.Mods)
					{
						mods.Add(mod.ID);
					}

					ViewData data = new ViewData
					{
						Mods = mods
					};

					SendUpdate(view, data);
				}
			}
		}
		#endregion

		#region Message Packet
		[MessagePackObject(false)]
		public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			[Key(0)] public List<ulong> Mods;

			public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<SyncMods>();

			public bool IsChangedFrom(ViewData cached)
			{
				return Mods != cached.Mods;
			}
		}
		#endregion
		public static bool _isMissingMod = false;
		public static List<ulong> _mods;
		protected override void UpdateData(ViewData view_data)
		{
			List<ulong> mods = new List<ulong>();

			foreach (Mod mod in ModPreload.Mods)
			{
				mods.Add(mod.ID);
			}

			_mods = view_data.Mods;

			foreach (ulong modid in view_data.Mods)
			{
				if (!mods.Contains(modid))
				{
					_isMissingMod = true;
				}
			}
		}

		void Update()
		{
		}
	}
}
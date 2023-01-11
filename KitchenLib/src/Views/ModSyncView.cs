using Kitchen;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using MessagePack;
using System;

namespace KitchenLib.Views
{
	public struct CMyComponent : IComponentData
	{
		public int Value;
	}
	public class MyNewView : UpdatableObjectView<MyNewView.ViewData>
	{
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			private EntityQuery Views;

			protected override void Initialise()
			{
				base.Initialise();
				Views = GetEntityQuery(new QueryHelper()
										   .All(typeof(CLinkedView), typeof(CMyComponent))
									   );
			}

			protected override void OnUpdate()
			{
				using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				using var components = Views.ToComponentDataArray<CMyComponent>(Allocator.Temp);

				for (var i = 0; i < views.Length; i++)
				{
					var view = views[i];
					var data = components[i];

					SendUpdate(view, new ViewData
					{
						MySentData1 = data.Value
					}, MessageType.SpecificViewUpdate);
				}
			}
		}

		// you must mark your ViewData as MessagePackObject and mark each field with a key
		// if you don't, the game will run locally but fail in multiplayer
		[MessagePackObject]
		public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			[Key(0)] public int MySentData1;

			// this tells the game how to find this subview within a prefab
			// GetSubView<T> is a cached method that looks for the requested T in the view and its children
			public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<MyNewView>();

			// this is used to determine if the data needs to be sent again
			public bool IsChangedFrom(ViewData check) => MySentData1 != check.MySentData1;
		}

		// this receives the updated data from the ECS backend whenever a new update is sent
		// in general, this should update the state of the view to match the values in view_data
		// ideally ignoring all current state; it's possible that not all updates will be received so
		// you should avoid relying on previous state where possible
		protected override void UpdateData(ViewData view_data)
		{
			// perform the update here
			// this is a Unity MonoBehavior so we can do normal Unity things here
			Main.instance.Log("----------------------------------------- " + view_data.MySentData1);
		}
	}
}
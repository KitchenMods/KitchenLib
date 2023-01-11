using Kitchen;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using MessagePack;

namespace KitchenLib.Views
{
	public struct CMyViewData : IComponentData
	{
		public List<string> CoolThings;
	}
	public class MyView : UpdatableObjectView<MyView.ViewData>
	{
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			private EntityQuery Views;

			protected override void Initialise()
			{
				Views = GetEntityQuery(new QueryHelper().All(typeof(CLinkedView), typeof(CMyViewData)));
			}

			protected override void OnUpdate()
			{
				using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				using var components = Views.ToComponentDataArray<CMyViewData>(Allocator.Temp);

				for (var i = 0; i < views.Length; i++)
				{
					var view = views[i];
					var data = components[i];

					SendUpdate(view, new ViewData
					{
						CoolThings = data.CoolThings
					}, MessageType.SpecificViewUpdate);
				}
			}
		}
		[MessagePackObject]
		public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			[Key(0)] public List<string> CoolThings;
			
			public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<MyView>();
			
			public bool IsChangedFrom(ViewData check) => CoolThings != check.CoolThings;
		}
		protected override void UpdateData(ViewData view_data)
		{
			foreach (string thing in view_data.CoolThings)
			{
				Main.instance.Log("" + thing);
			}
		}
	}
}

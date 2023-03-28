using Kitchen;
using KitchenLib.Systems;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Views
{
	public class SendToClientView : UpdatableObjectView<SendToClientView.ViewData>
	{
		#region ECS View System (Runs on host and updates views to be broadcasted to clients)
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			public static bool isDark = false;
			private EntityQuery Views;

			protected override void Initialise()
			{
				base.Initialise();

				Views = GetEntityQuery(new QueryHelper().All(typeof(CSendToClientView), typeof(CLinkedView)));
			}

			protected override void OnUpdate()
			{
				using var entities = Views.ToEntityArray(Allocator.Temp);
				using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				
				for (var i = 0; i < views.Length; i++)
				{
					var view = views[i];

					ViewData data = new ViewData
					{
						isDark = isDark
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
			[Key(1)] public bool isDark;

			public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<SendToClientView>();

			public bool IsChangedFrom(ViewData cached)
			{
				return isDark != cached.isDark;
			}
		}
		#endregion

		protected bool isDark = false;
		protected GameObject Light = null;
		protected override void UpdateData(ViewData view_data)
		{
			isDark = view_data.isDark;
		}

		void Update()
		{
			if (Light == null)
				Light = GameObject.Find("Directional Light");
			if (isDark)
				Light.SetActive(false);
			else
				Light.SetActive(true);
		}
	}
}
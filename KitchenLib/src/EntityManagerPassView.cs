using Kitchen;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Collections;
using System;

namespace KitchenLib
{
	public class EntityManagerPassView : ResponsiveViewSystemBase<EntityManagerPassView.ViewData, EntityManagerPassView.ResponceData>
	{
		protected override void OnUpdate()
		{
			var Entities = GetEntityQuery(new QueryHelper()
					.All(typeof(EntityManagerPassView.ViewData)));
			foreach (var entity in Entities.ToEntityArray(Allocator.TempJob))
			{
				CLinkedView linkedView;
				Require<CLinkedView>(entity, out linkedView);

				ApplyUpdates(linkedView.Identifier, new Action<EntityManagerPassView.ResponceData>(delegate (EntityManagerPassView.ResponceData data)
				{
				}), true);
			}
		}

		public struct ViewData : IViewData, IViewResponseData, IViewData.ICheckForChanges<EntityManagerPassView.ViewData>, IRollUp
		{
			public IUpdatableObject GetRelevantSubview(IObjectView view)
			{
				return view.GetSubView<CrateView>();
			}

			public bool IsChangedFrom(EntityManagerPassView.ViewData check)
			{
				return false;
			}

		}

		public struct ResponceData : IResponseData, IViewResponseData, IRollUp
		{
			public List<string> commands;
		}
	}

}

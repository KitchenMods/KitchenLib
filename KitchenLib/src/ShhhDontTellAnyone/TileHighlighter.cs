using Kitchen;
using KitchenLib.Systems;
using KitchenMods;
using MessagePack;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenLib.ShhhDontTellAnyone
{
	public class TileHighlighter : UpdatableObjectView<TileHighlighter.ViewData>, ISpecificViewResponse
	{
		public class UpdateView : ResponsiveViewSystemBase<ViewData, ResponseData>, IModSystem
		{
			private Vector3 lastLocation = new Vector3(0, 0, 0);

			EntityQuery Query;
			protected override void Initialise()
			{
				base.Initialise();
				Query = GetEntityQuery(typeof(CLinkedView), typeof(CTileHightlighterView));
			}

			protected override void OnUpdate()
			{
				using NativeArray<CLinkedView> linkedViews = Query.ToComponentDataArray<CLinkedView>(Allocator.Temp);

				foreach (CLinkedView view in linkedViews)
				{
					SendUpdate(view.Identifier, new ViewData
					{
						Location = lastLocation
					});
					if (ApplyUpdates(view.Identifier, PerformUpdateWithResponse, only_final_update: false)) { }
				}
			}

			private void PerformUpdateWithResponse(ResponseData data)
			{
				Entity e = GetOccupant(data.Location);
				if (Require(e, out CPosition pos))
				{
					lastLocation = pos.Position;
				}
			}
		}

		[MessagePackObject(false)]
		public class ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			[Key(0)]
			public Vector3 Location;
			public IUpdatableObject GetRelevantSubview(IObjectView view)
			{
				return view.GetSubView<TileHighlighter>();
			}

			public bool IsChangedFrom(ViewData check)
			{
				return true;
			}
		}

		[MessagePackObject(false)]
		public class ResponseData : IResponseData, IViewResponseData
		{
			[Key(0)]
			public Vector3 Location;
		}

		private Action<IResponseData, Type> Callback;

		private static GameObject tileHighlighter = null;
		private Vector3 location = new Vector3(0, 0, 0);
		private Vector3 gridLocation = new Vector3(0, 0, 0);
		public void Update()
		{
			if (tileHighlighter == null)
				tileHighlighter = Instantiate(Main.bundle.LoadAsset<GameObject>("TileHighlight"));

			if (!RefVars.DoesModeRequirePointer(RefVars.CurrentMode))
				tileHighlighter.SetActive(false);
			else
				tileHighlighter.SetActive(true);

			tileHighlighter.transform.position = gridLocation;

			Plane plane = new Plane(Vector3.down, 0.3f);
			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
			if (plane.Raycast(ray, out float distance))
			{
				Vector3 intersection = ray.GetPoint(distance);
				location = new Vector3(intersection.x, 0, intersection.z);
			}
		}

		protected override void UpdateData(ViewData data)
		{
			if (Callback == null)
				return;
			if (data.Location != null)
			{
				gridLocation = data.Location;
			}
			ResponseData response = new ResponseData
			{
				Location = location
			};

			Callback.Invoke(response, typeof(ResponseData));
		}

		public void SetCallback(Action<IResponseData, Type> callback)
		{
			Callback = callback;
		}
	}
}

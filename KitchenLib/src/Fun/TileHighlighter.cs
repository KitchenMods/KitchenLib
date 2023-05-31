using Kitchen;
using KitchenData;
using KitchenLib.Systems;
using KitchenLib.Utils;
using KitchenMods;
using MessagePack;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenLib.Fun
{
	public class TileHighlighter : UpdatableObjectView<TileHighlighter.ViewData>, ISpecificViewResponse
	{
		public class UpdateView : ResponsiveViewSystemBase<ViewData, ResponseData>, IModSystem
		{
			private Vector3 lastLocation = new Vector3(0, 0, 0);
			public static OccupancyLayer SelectedLayer = OccupancyLayer.Default;

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
				Entity e = GetOccupant(data.Location, SelectedLayer);
				if (Require(e, out CPosition pos))
				{
					lastLocation = pos.Position;
				}

				if (data.Scroll == 1)
				{
					if (SelectedLayer == OccupancyLayer.Default)
						SelectedLayer = OccupancyLayer.Wall;
					else if (SelectedLayer == OccupancyLayer.Wall)
						SelectedLayer = OccupancyLayer.Floor;
					else if (SelectedLayer == OccupancyLayer.Floor)
						SelectedLayer = OccupancyLayer.Ceiling;
					else if (SelectedLayer == OccupancyLayer.Ceiling)
						SelectedLayer = OccupancyLayer.Default;
				}
				else if (data.Scroll == -1)
				{
					if (SelectedLayer == OccupancyLayer.Default)
						SelectedLayer = OccupancyLayer.Ceiling;
					else if (SelectedLayer == OccupancyLayer.Wall)
						SelectedLayer = OccupancyLayer.Default;
					else if (SelectedLayer == OccupancyLayer.Floor)
						SelectedLayer = OccupancyLayer.Wall;
					else if (SelectedLayer == OccupancyLayer.Ceiling)
						SelectedLayer = OccupancyLayer.Floor;
				}
				data.Scroll = 0;
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
			[Key(1)]
			public int Scroll;
		}

		private Action<IResponseData, Type> Callback;

		private static GameObject tileHighlighter = null;
		private static MeshRenderer renderer = null;
		private Vector3 location = new Vector3(0, 0, 0);
		private Vector3 gridLocation = new Vector3(0, 0, 0);
		public void Update()
		{
			if (tileHighlighter == null)
			{
				tileHighlighter = Instantiate(Main.bundle.LoadAsset<GameObject>("TileHighlight"));
				renderer = tileHighlighter.transform.Find("TileHighlight").GetComponent<MeshRenderer>();
			}

			if (!RefVars.DoesModeRequirePointer(RefVars.CurrentMode))
				tileHighlighter.SetActive(false);
			else
				tileHighlighter.SetActive(true);

			switch (RefVars.CurrentMode)
			{
				case FunMode.Fire:
					renderer.material = MaterialUtils.GetExistingMaterial("Balloon - Red");
					break;
				case FunMode.Process:
					renderer.material = MaterialUtils.GetExistingMaterial("Plastic - White");
					break;
				case FunMode.Theme:
					renderer.material = MaterialUtils.GetExistingMaterial("Plastic - Shiny Gold");
					break;
				case FunMode.Garbage:
					renderer.material = MaterialUtils.GetExistingMaterial("AppleBurnt");
					break;
				case FunMode.ItemProvider:
					renderer.material = MaterialUtils.GetExistingMaterial("Plastic - White");
					break;
				case FunMode.ResetOrder:
					renderer.material = MaterialUtils.GetExistingMaterial("Balloon - Blue");
					break;
				default:
					renderer.material = MaterialUtils.GetExistingMaterial("Apple Flesh");
					break;
			}

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
			if (FunMenu.isOpen)
			{
				if (Callback == null)
					return;
				if (data.Location != null)
				{
					gridLocation = data.Location;
				}
				int scroll = 0;
				if (Input.mouseScrollDelta.y > 0)
					scroll = 1;
				else if (Input.mouseScrollDelta.y < 0)
					scroll = -1;
				
				ResponseData response = new ResponseData
				{
					Location = location,
					Scroll = scroll
				};

				Callback.Invoke(response, typeof(ResponseData));
			}
		}

		public void SetCallback(Action<IResponseData, Type> callback)
		{
			Callback = callback;
		}
	}
}

using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Systems;
using KitchenLib.UI;
using KitchenLib.Utils;
using KitchenMods;
using MessagePack;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace KitchenLib.Views
{
	public class CommandView : UpdatableObjectView<CommandView.ViewData>, ISpecificViewResponse
	{
		public class UpdateView : ResponsiveViewSystemBase<ViewData, ResponseData>, IModSystem
		{
			/*
			 * HELPERS
			 */

			protected bool GetItemFromHolder(Entity entity, out Entity item, out CItem cItem)
			{
				if (Require(entity, out CItemHolder cItemHolder))
				{
					item = cItemHolder.HeldItem;
					Require(item, out cItem);
					return true;
				}

				cItem = new CItem();
				item = Entity.Null;
				return false;
			}

			protected void TryRunProcessOnItem(Entity holder, Process process)
			{
				if (GetItemFromHolder(holder, out Entity item, out CItem cItem))
				{
					Item newItem = GameData.Main.Get<Item>(cItem.ID);
					int id = GDOUtils.GetItemProcessResult(newItem, process);
					if (GameData.Main.Get<Item>(id) != null)
					{
						cItem.ID = id;
						EntityManager.SetComponentData(item, cItem);
					}
				}
			}
			/*
			 * 
			 */
			EntityQuery Query;
			int money;
			protected override void Initialise()
			{
				base.Initialise();
				Query = GetEntityQuery(typeof(CLinkedView), typeof(CViewHolder));
			}

			protected override void OnUpdate()
			{
				using NativeArray<CLinkedView> linkedViews = Query.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				if (HasSingleton<SMoney>())
					this.money = GetSingleton<SMoney>().Amount;

				foreach (CLinkedView view in linkedViews)
				{
					SendUpdate(view.Identifier, new ViewData());

					if (ApplyUpdates(view.Identifier, PerformUpdateWithResponse, only_final_update: false))
					{
					}
				}
			}

			private void PerformUpdateWithResponse(ResponseData data)
			{
				if (data.Mode == FunMode.None)
					return;
				if (data.Mode == FunMode.Fire)
				{
					Entity entity = GetOccupant(data.vector3_1);
					if (Has<CAppliance>(entity))
					{
						if (Has<CIsOnFire>(entity))
							EntityManager.RemoveComponent<CIsOnFire>(entity);
						else
							EntityManager.AddComponent<CIsOnFire>(entity);
					}
				}
				if (data.Mode == FunMode.Chop)
				{
					Entity entity = GetOccupant(data.vector3_1);
					TryRunProcessOnItem(entity, GameData.Main.Get<Process>(ProcessReferences.Chop));
				}
				if (data.Mode == FunMode.Clean)
				{
					Entity entity = GetOccupant(data.vector3_1);
					TryRunProcessOnItem(entity, GameData.Main.Get<Process>(ProcessReferences.Clean));
				}
				if (data.Mode == FunMode.Cook)
				{
					Entity entity = GetOccupant(data.vector3_1);
					TryRunProcessOnItem(entity, GameData.Main.Get<Process>(ProcessReferences.Cook));
				}
				if (data.Mode == FunMode.Knead)
				{
					Entity entity = GetOccupant(data.vector3_1);
					TryRunProcessOnItem(entity, GameData.Main.Get<Process>(ProcessReferences.Knead));
				}
				if (data.Mode == FunMode.Customer)
				{
					GenerateCustomers.isCat = data.bool1;
					GenerateCustomers.AddCustomer = data.int1;
				}
				if (data.Mode == FunMode.SpawnBlueprint)
				{
					SpawnUtils.SpawnApplianceBlueprintAtPosition(data.int1, data.vector3_1, 0, 0, useRedBlueprint);
				}
				if (data.Mode == FunMode.Mess)
				{
					Entity entity = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition), typeof(CViewHolder));
					if (isKitchenMess)
					{
						switch (messType)
						{
							case 1:
								EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessKitchen1 });
								break;
							case 2:
								EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessKitchen2 });
								break;
							case 3:
								EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessKitchen3 });
								break;
						}
					}
					else
					{
						switch (messType)
						{
							case 1:
								EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessCustomer1 });
								break;
							case 2:
								EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessCustomer2 });
								break;
							case 3:
								EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessCustomer3 });
								break;
						}
					}
					EntityManager.SetComponentData(entity, new CPosition(data.vector3_1));
				}
				if (data.Mode == FunMode.Theme)
				{
					Entity entity = GetOccupant(data.vector3_1);
					if (!Has<CAppliance>(entity))
						return;
					if (!Has<CGivesDecoration>(entity))
						EntityManager.AddComponent<CGivesDecoration>(entity);
					CGivesDecoration cGivesDecoration = new CGivesDecoration();
					cGivesDecoration.DecorationValues = new DecorationValues
					{
						Exclusive = data.int1,
						Affordable = data.int2,
						Charming = data.int3,
						Formal = data.int4,
						Kitchen = data.int5,
					};
					EntityManager.SetComponentData(entity, cGivesDecoration);
				}
			}
		}

		[MessagePackObject(false)]
		public class ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			public IUpdatableObject GetRelevantSubview(IObjectView view)
			{
				return view.GetSubView<CommandView>();
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
			public FunMode Mode;

			[Key(1)]
			public Vector3 vector3_1;

			[Key(2)]
			public int int1;

			[Key(3)]
			public int int2;

			[Key(4)]
			public int int3;

			[Key(5)]
			public int int4;

			[Key(6)]
			public int int5;

			[Key(7)]
			public bool bool1;
		}

		private Action<IResponseData, Type> Callback;


		private bool wasPressed = false;
		private Vector3 location = new Vector3(0, 0, 0);
		private FunMode Mode;
		private ButtonControl incrementCounterKey = Mouse.current.rightButton;
		public static bool isCat = false;
		public static float customerCount = 0;
		public static int selectedBlueprint = 0;
		public static int manualBlueprint = 0;
		public static bool useRedBlueprint = false;
		public static int messType = 1;
		public static bool isKitchenMess = false;
		public static int exclusiveLevel = 0;
		public static int affordableLevel = 0;
		public static int charmingLevel = 0;
		public static int formalLevel = 0;
		public static int kitchenLevel = 0;


		public void Update()
		{
			if (incrementCounterKey.isPressed)
			{
				if (!wasPressed)
				{
					Plane plane = new Plane(Vector3.down, 0.3f);
					Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
					if (plane.Raycast(ray, out float distance))
					{
						Vector3 intersection = ray.GetPoint(distance);
						location = new Vector3(intersection.x, 0, intersection.z);
					}
					Mode = FunMenu.Mode;
				}
				wasPressed = true;
			}
			else wasPressed = false;
		}

		protected override void UpdateData(ViewData data)
		{
			if (Mode == FunMode.None)
				return;

			if (Callback == null)
				return;

			ResponseData response = new ResponseData
			{
				Mode = Mode
			};

			switch (FunMenu.Mode)
			{
				case FunMode.Fire:
					response.vector3_1 = location;
					break;
				case FunMode.Chop:
					response.vector3_1 = location;
					break;
				case FunMode.Clean:
					response.vector3_1 = location;
					break;
				case FunMode.Cook:
					response.vector3_1 = location;
					break;
				case FunMode.Knead:
					response.vector3_1 = location;
					break;
				case FunMode.Customer:
					response.bool1 = isCat;
					response.int1 = int.Parse(customerCount.ToString());
					break;
				case FunMode.SpawnBlueprint:
					response.vector3_1 = location;
					response.int1 = selectedBlueprint;
					if (manualBlueprint != 0)
						response.int1 = manualBlueprint;
					break;
				case FunMode.Mess:
					response.vector3_1 = location;
					break;
				case FunMode.Theme:
					response.vector3_1 = location;
					response.int1 = exclusiveLevel;
					response.int2 = affordableLevel;
					response.int3 = charmingLevel;
					response.int4 = formalLevel;
					response.int5 = kitchenLevel;
					break;
			}

			Callback.Invoke(response, typeof(ResponseData));

			Mode = FunMode.None;
		}

		public void SetCallback(Action<IResponseData, Type> callback)
		{
			Callback = callback;
		}
	}
}

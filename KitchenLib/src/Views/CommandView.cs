using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Systems;
using KitchenLib.UI;
using KitchenLib.Utils;
using KitchenMods;
using MessagePack;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Scripting;

namespace KitchenLib.Views
{
	public class CommandView : UpdatableObjectView<CommandView.ViewData>, ISpecificViewResponse
	{
		public class UpdateView : ResponsiveViewSystemBase<ViewData, ResponseData>, IModSystem
		{
			private CommandViewHelpers helpers = null;

			EntityQuery Query;
			EntityQuery Players;
			Dictionary<int, Entity> PlayerEntities = new Dictionary<int, Entity>();
			int money;
			protected override void Initialise()
			{
				base.Initialise();
				Query = GetEntityQuery(typeof(CLinkedView), typeof(CCommandView));
				Players = GetEntityQuery(typeof(CPlayer));
			}

			protected override void OnUpdate()
			{
				if (helpers == null)
					helpers = SystemUtils.GetSystem<CommandViewHelpers>();
				using NativeArray<CLinkedView> linkedViews = Query.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				if (HasSingleton<SMoney>())
					this.money = GetSingleton<SMoney>().Amount;

				foreach (Entity entity in Players.ToEntityArray(Allocator.Temp))
				{
					if (Require(entity, out CPlayer player))
					{
						if (PlayerEntities.ContainsKey(player.ID))
							PlayerEntities[player.ID] = entity;
						else
							PlayerEntities.Add(player.ID, entity);
					}
				}

				foreach (CLinkedView view in linkedViews)
				{
					SendUpdate(view.Identifier, new ViewData());

					if (ApplyUpdates(view.Identifier, PerformUpdateWithResponse, only_final_update: false)) { }
				}
			}

			private void PerformUpdateWithResponse(ResponseData data)
			{
				switch (data.Mode)
				{
					case FunMode.None:
						return;
					case FunMode.Fire:
						helpers.ToggleFire(GetOccupant(data.vector3_1));
						break;
					case FunMode.Chop:
						helpers.TryRunProcessOnItem(GetOccupant(data.vector3_1), GameData.Main.Get<Process>(ProcessReferences.Chop));
						break;
					case FunMode.Clean:
						helpers.TryRunProcessOnItem(GetOccupant(data.vector3_1), GameData.Main.Get<Process>(ProcessReferences.Clean));
						break;
					case FunMode.Cook:
						helpers.TryRunProcessOnItem(GetOccupant(data.vector3_1), GameData.Main.Get<Process>(ProcessReferences.Cook));
						break;
					case FunMode.Knead:
						helpers.TryRunProcessOnItem(GetOccupant(data.vector3_1), GameData.Main.Get<Process>(ProcessReferences.Knead));
						break;
					case FunMode.Customer:
						helpers.AddCustomers(data.int1, data.bool1);
						break;
					case FunMode.SpawnBlueprint:
						SpawnUtils.SpawnApplianceBlueprintAtPosition(data.int1, data.vector3_1, 0, 0, useRedBlueprint);
						break;
					case FunMode.Mess:
						helpers.SpawnMess(data.vector3_1, data.bool1, data.int1);
						break;
					case FunMode.Theme:
						helpers.AssignDecorationValues(GetOccupant(data.vector3_1), data.int1, data.int2, data.int3, data.int4, data.int5);
						break;
					case FunMode.Cosmetics:
						if (PlayerEntities.ContainsKey(data.int1))
							helpers.AssignPlayerCosmetics(PlayerEntities[data.int1], data.int2, data.int3);
						break;
					case FunMode.Color:
						if (PlayerEntities.ContainsKey(data.int1))
							helpers.AssignPlayerColor(PlayerEntities[data.int1], data.string1);
						break;
					case FunMode.Speed:
						PlayerSpeedOverride.SetPlayerSpeedMultiplier(data.int1, data.float1);
						break;
					case FunMode.Blindness:
						helpers.ToggleBlindness();
						break;
					case FunMode.Garbage:
						helpers.FillGarbage(GetOccupant(data.vector3_1));
						break;
					case FunMode.ItemProvider:
						helpers.AddToItemProvider(GetOccupant(data.vector3_1));
						break;
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
			public int mouseButton;

			[Key(1)]
			public FunMode Mode;

			[Key(2)]
			public Vector3 vector3_1;

			[Key(3)]
			public int int1;

			[Key(4)]
			public int int2;

			[Key(5)]
			public int int3;

			[Key(6)]
			public int int4;

			[Key(7)]
			public int int5;

			[Key(8)]
			public bool bool1;
			[Key(9)]
			public string string1;

			[Key(10)]
			public float float1;
		}

		private Action<IResponseData, Type> Callback;


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
		public static Dictionary<int, string> players = new Dictionary<int, string>();
		public static int selectedPlayer = 0;
		public static int selectedOutfit = 0;
		public static int selectedHat = 0;
		public static string color;
		public static float speed;

		private static int pressedButton = 0;

		private bool wasPressed = false;
		private Vector3 location = new Vector3(0, 0, 0);
		private FunMode Mode;
		private ButtonControl rightButton = Mouse.current.rightButton;
		private ButtonControl middleButton = Mouse.current.middleButton;
		public void Update()
		{
			if (rightButton.isPressed || middleButton.isPressed)
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
					if (rightButton.isPressed)
						pressedButton = 0;
					else if (middleButton.isPressed)
						pressedButton = 1;
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
				Mode = Mode,
				mouseButton = pressedButton
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
					response.bool1 = isKitchenMess;
					response.int1 = messType;
					break;
				case FunMode.Theme:
					response.vector3_1 = location;
					response.int1 = exclusiveLevel;
					response.int2 = affordableLevel;
					response.int3 = charmingLevel;
					response.int4 = formalLevel;
					response.int5 = kitchenLevel;
					break;
				case FunMode.Cosmetics:
					response.int1 = selectedPlayer;
					response.int2 = selectedOutfit;
					response.int3 = selectedHat;
					break;
				case FunMode.Color:
					response.int1 = selectedPlayer;
					response.string1 = color;
					break;
				case FunMode.Speed:
					response.int1 = selectedPlayer;
					response.float1 = speed;
					break;
				case FunMode.Blindness:
					break;
				case FunMode.Garbage:
					response.vector3_1 = location;
					break;
				case FunMode.ItemProvider:
					response.vector3_1 = location;
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

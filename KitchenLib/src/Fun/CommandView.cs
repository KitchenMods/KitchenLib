using Kitchen;
using KitchenData;
using KitchenLib.References;
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
	public class CommandView : UpdatableObjectView<CommandView.ViewData>, ISpecificViewResponse
	{
		public class UpdateView : ResponsiveViewSystemBase<ViewData, ResponseData>, IModSystem
		{
			private CommandViewHelpers helpers = null;

			EntityQuery Query;
			protected override void Initialise()
			{
				base.Initialise();
				Query = GetEntityQuery(typeof(CLinkedView), typeof(CCommandView));
			}

			protected override void OnUpdate() // Constantly send updates (Empty packets) to force a response.
			{
				if (helpers == null)
					helpers = SystemUtils.GetSystem<CommandViewHelpers>();

				using NativeArray<CLinkedView> linkedViews = Query.ToComponentDataArray<CLinkedView>(Allocator.Temp);

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
					case FunMode.Fire:
						CommandViewHelpers.Main.ToggleFireOnLocation(data.Location);
						break;
					case FunMode.Outfit:
						CommandViewHelpers.Main.SetPlayerCosmetic(CosmeticType.Outfit, data.int1, data.int2);
						CommandViewHelpers.Main.SetPlayerCosmetic(CosmeticType.Hat, data.int1, data.int3);
						break;
					case FunMode.Color:
						CommandViewHelpers.Main.SetPlayerColor(data.int1, data.color1);
						break;
					case FunMode.HexColor:
						CommandViewHelpers.Main.SetPlayerColor(data.int1, data.color1);
						break;
					case FunMode.Speed:
						CommandViewHelpers.Main.SetPlayerSpeedMultiplier(data.int1, data.float1);
						break;
					case FunMode.Process:
						CommandViewHelpers.Main.TryRunProcessOnItem(data.Location, RefVars.GetProcessFromType(data.ProcessType));
						break;
					case FunMode.Theme:
						CommandViewHelpers.Main.SetThemeLevel(data.Location, data.int1, data.int2, data.int3, data.int4, data.int5);
						break;
					case FunMode.CustomerSpawn:
						CommandViewHelpers.Main.AddCustomers(data.int1, data.bool1);
						break;
					case FunMode.BlueprintSpawn:
						SpawnUtils.SpawnApplianceBlueprintAtPosition(data.int1, data.Location, 1, data.int2, data.bool1);
						break;
					case FunMode.ManualBlueprintSpawn:
						SpawnUtils.SpawnApplianceBlueprintAtPosition(data.int1, data.Location, 1, data.int2, data.bool1);
						break;
					case FunMode.Mess:
						CommandViewHelpers.Main.SpawnMess(data.Location, data.bool1, data.int1);
						break;
					case FunMode.Blindness:
						CommandViewHelpers.Main.ToggleBlindness();
						break;
					case FunMode.Garbage:
						CommandViewHelpers.Main.AdjustBin(data.Location, data.UpNumpad, data.DownNumpad);
						break;
					case FunMode.ItemProvider:
						CommandViewHelpers.Main.AdjustItemProvider(data.Location, data.UpNumpad, data.DownNumpad);
						break;
					case FunMode.Unlock:
						CommandViewHelpers.Main.ToggleDish(data.int1);
						break;
					case FunMode.Arsonist:
						CommandViewHelpers.Main.BurnEverything();
						break;
					case FunMode.FireFighter:
						CommandViewHelpers.Main.UnBurnEverything();
						break;
					case FunMode.ResetOrder:
						CommandViewHelpers.Main.ResetOrder(data.Location);
						break;
					case FunMode.Money:
						CommandViewHelpers.Main.SetMoney(data.int1);
						break;
					case FunMode.NewProcess:
						CommandViewHelpers.Main.New_TryRunProcessOnItem(data.Location, GameData.Main.Get<Process>(data.int1), data.bool1);
						break;
					case FunMode.Destroy:
						CommandViewHelpers.Main.DestroyAppliance(data.Location);
						break;
					case FunMode.ChangeOrder:
						CommandViewHelpers.Main.ChangeOrder(data.Location, data.int1);
						break;
				}

				data.Mode = FunMode.None;
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
			public bool RightMouseButton;
			[Key(2)]
			public bool UpNumpad;
			[Key(3)]
			public bool DownNumpad;
			[Key(4)]
			public Vector3 Location;

			[Key(5)]
			public int int1;
			[Key(6)]
			public int int2;
			[Key(7)]
			public int int3;
			[Key(8)]
			public int int4;
			[Key(9)]
			public int int5;
			[Key(10)]
			public Color color1;
			[Key(11)]
			public float float1;
			[Key(12)]
			public ProcessType ProcessType;
			[Key(13)]
			public bool bool1;
		}

		private Action<IResponseData, Type> Callback;

		private Vector3 location = new Vector3(0, 0, 0);
		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Keypad2))
			{
				RefVars.SendUpdate = true;
			}
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
			if (!RefVars.SendUpdate)
				return;
			RefVars.SendUpdate = false;
			if (RefVars.CurrentMode == FunMode.None)
				return;

			ResponseData response = new ResponseData
			{
				Mode = RefVars.CurrentMode,
				RightMouseButton = Input.GetKey(KeyCode.Mouse1),
				UpNumpad = Input.GetKey(KeyCode.Keypad8),
				DownNumpad = Input.GetKey(KeyCode.Keypad2),
				Location = location
			};

			switch (RefVars.CurrentMode)
			{
				case FunMode.Outfit:
					response.int1 = RefVars.SelectedPlayer;
					response.int2 = RefVars.SelectedOutfit;
					response.int3 = RefVars.SelectedHat;
					break;
				case FunMode.Color:
					response.int1 = RefVars.SelectedPlayer;
					response.color1 = RefVars.SelectedPlayerColor;
					break;
				case FunMode.HexColor:
					response.int1 = RefVars.SelectedPlayer;
					ColorUtility.TryParseHtmlString(RefVars.SelectedPlayerHex, out response.color1);
					break;
				case FunMode.Speed:
					response.int1 = RefVars.SelectedPlayer;
					response.float1 = RefVars.SelectedPlayerSpeed;
					break;
				case FunMode.Process:
					response.ProcessType = RefVars.CurrentProcessType;
					break;
				case FunMode.Theme:
					response.int1 = RefVars.SelectedExclusiveLevel;
					response.int2 = RefVars.SelectedAffordableLevel;
					response.int3 = RefVars.SelectedCharmingLevel;
					response.int4 = RefVars.SelectedFormalLevel;
					response.int5 = RefVars.SelectedKitchenLevel;
					break;
				case FunMode.CustomerSpawn:
					response.int1 = RefVars.SelectedCustomerCount;
					response.bool1 = RefVars.SelectedIsCustomerCat;
					break;
				case FunMode.BlueprintSpawn:
					response.int1 = RefVars.SelectedAppliance;
					response.int2 = RefVars.ForcedAppliancePrice;
					response.bool1 = RefVars.IsRedAppliance;
					break;
				case FunMode.ManualBlueprintSpawn:
					response.int1 = RefVars.SelectedManualBlueprint;
					response.int2 = RefVars.ForcedAppliancePrice;
					response.bool1 = RefVars.IsRedAppliance;
					break;
				case FunMode.Mess:
					response.int1 = RefVars.SelectedMessLevel;
					response.bool1 = RefVars.IsMessKitchen;
					break;
				case FunMode.Unlock:
					response.int1 = RefVars.SelectedUnlock;
					break;
				case FunMode.Money:
					response.int1 = RefVars.SelectedMoney;
					break;
				case FunMode.NewProcess:
					response.int1 = RefVars.SelectedProcess;
					response.bool1 = RefVars.ReversedProcess;
					break;
				case FunMode.ChangeOrder:
					response.int1 = RefVars.SelectedOrder;
					break;
			}
			Callback.Invoke(response, typeof(ResponseData));
			if (!RefVars.DoesModePersist(RefVars.CurrentMode))
				RefVars.CurrentMode = FunMode.None;
		}

		public void SetCallback(Action<IResponseData, Type> callback)
		{
			Callback = callback;
		}
	}
}

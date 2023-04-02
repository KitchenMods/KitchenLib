using KitchenData;
using KitchenLib.References;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Fun
{
	internal class RefVars
	{
		internal static bool SendUpdate = false;
		internal static FunMode CurrentMode = FunMode.None;
		internal static ProcessType CurrentProcessType = ProcessType.None;


		//Client Variables
		private static Dictionary<int, string> keyPairValues = new Dictionary<int, string>();
		private static List<FunMode> RequiresPointer = new List<FunMode> { FunMode.Fire, FunMode.Process, FunMode.Theme, FunMode.Garbage, FunMode.ItemProvider, FunMode.ResetOrder};
		private static List<FunMode> Persists = new List<FunMode> { FunMode.Fire, FunMode.Process, FunMode.Theme, FunMode.BlueprintSpawn, FunMode.ManualBlueprintSpawn, FunMode.Mess, FunMode.Garbage, FunMode.ItemProvider, FunMode.ResetOrder };
		private static Dictionary<ProcessType, int> processKeyPairs = new Dictionary<ProcessType, int>
		{
			{ProcessType.Chop, ProcessReferences.Chop},
			{ProcessType.Clean, ProcessReferences.Clean},
			{ProcessType.Cook, ProcessReferences.Cook},
			{ProcessType.ExtinguishFire, ProcessReferences.ExtinguishFire},
			{ProcessType.FillCoffee, ProcessReferences.FillCoffee},
			{ProcessType.Knead, ProcessReferences.Knead}
		};

		internal static Dictionary<int, string> AvailableOutfits = new Dictionary<int, string>();
		internal static Dictionary<int, string> AvailableHats = new Dictionary<int, string>();
		internal static Dictionary<int, string> AvailableAppliances = new Dictionary<int, string>();
		internal static Dictionary<Color, string> AvailableColors = new Dictionary<Color, string>();
		internal static Dictionary<int, string> CurrentPlayers = new Dictionary<int, string>();
		internal static Dictionary<int, string> AvailableUnlocks = new Dictionary<int, string>();
		internal static List<int> ActiveUnlocks = new List<int>();
		
		internal static float SelectedPlayerSpeed = 1f;
		internal static string SelectedPlayerHex = "";
		internal static Color SelectedPlayerColor = Color.red;
		internal static int SelectedOutfit = 0;
		internal static int SelectedHat = 0;
		internal static int SelectedPlayer = 0;
		internal static int SelectedExclusiveLevel = 0;
		internal static int SelectedAffordableLevel = 0;
		internal static int SelectedCharmingLevel = 0;
		internal static int SelectedFormalLevel = 0;
		internal static int SelectedKitchenLevel = 0;
		internal static int SelectedCustomerCount = 0;
		internal static bool SelectedIsCustomerCat = false;
		internal static int SelectedManualBlueprint = 0;
		internal static int SelectedAppliance = 0;
		internal static bool IsRedAppliance = false;
		internal static int ForcedAppliancePrice = 0;
		internal static int SelectedMessLevel = 1;
		internal static bool IsMessKitchen = false;
		internal static int SelectedUnlock = 0;

		//Host Variables

		internal static bool IsBlind = false;

		internal static string GetKeyPair(int id)
		{
			return keyPairValues[id];
		}

		internal static void SetKeyPair(int id, string value)
		{
			if (keyPairValues.ContainsKey(id))
			{
				keyPairValues[id] = value;
			}
			else
			{
				keyPairValues.Add(id, value);
			}
		}

		internal static void ForceUpdate(FunMode mode)
		{
			CurrentMode = mode;
			SendUpdate = true;
		}

		internal static bool DoesModePersist(FunMode mode)
		{
			return Persists.Contains(mode);
		}

		internal static bool DoesModeRequirePointer(FunMode mode)
		{
			return RequiresPointer.Contains(mode);
		}
		internal static Process GetProcessFromType(ProcessType type)
		{
			return GameData.Main.Get<Process>(processKeyPairs[type]);
		}
		internal static void ToggleProcessMode(ProcessType type)
		{
			if (CurrentMode != FunMode.Process)
			{
				CurrentMode = FunMode.Process;
				CurrentProcessType = type;
			} else if (CurrentMode == FunMode.Process && CurrentProcessType == type)
			{
				CurrentMode = FunMode.None;
				CurrentProcessType = ProcessType.None;
			}
			else if (CurrentMode == FunMode.Process && CurrentProcessType != type)
			{
				CurrentProcessType = type;
			}
		}

		internal static void ToggleFunMode(FunMode mode)
		{
			if (CurrentMode == mode)
			{
				CurrentMode = FunMode.None;
			} else if (CurrentMode != mode)
			{
				CurrentMode = mode;
			}
		}
	}

	public enum FunMode
	{
		None,
		Fire,
		Outfit,
		Color,
		HexColor,
		Speed,
		Process,
		Theme,
		CustomerSpawn,
		BlueprintSpawn,
		ManualBlueprintSpawn,
		Mess,
		Blindness,
		Garbage,
		ItemProvider,
		Unlock,
		Arsonist,
		FireFighter,
		ResetOrder
	}

	public enum ProcessType
	{
		None,
		Chop,
		Clean,
		Cook,
		ExtinguishFire,
		FillCoffee,
		Knead
	}
}

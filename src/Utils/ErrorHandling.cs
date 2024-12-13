using System;
using System.Collections.Generic;
using Kitchen;
using KitchenLib.Customs;
using KitchenLib.Preferences;
using KitchenLib.UI.PlateUp;
using KitchenMods;
using UnityEngine;

namespace KitchenLib.Utils
{
	internal class ErrorHandling
	{
		public static List<FailedGDO> FailedGDOs = new List<FailedGDO>();
		public static List<FailedMod> FailedMods = new List<FailedMod>();
		
		public static void AddFailedGDO(CustomGameDataObject gdo, Exception e, GDOFailureState state)
		{
			FailedGDOs.Add(new FailedGDO(gdo, state, e));
		}
		
		public static void AddFailedMod(Mod mod, Exception e, ModFailureState state)
		{
			FailedMods.Add(new FailedMod(mod, state, e));
		}
		
		private static Dictionary<Type, bool> MenuLoadingOrder = new Dictionary<Type, bool>
		{
			{typeof(BetaWarningMenu), Main.MOD_BETA_VERSION != "" && !VariousUtils.HasCommandlineArgument("-noklbetawarning") },
			{typeof(FailedGDOsMenu), FailedGDOs.Count > 0},
			{typeof(FailedModsMenu), FailedMods.Count > 0},
			{typeof(GameUpdateWarning), Main.manager != null && !Main.manager.GetPreference<PreferenceString>("lastVersionCheck").Value.Equals(Application.version) },
			{typeof(SaveDataDisclosure), PreferenceManager.globalManager != null && PreferenceManager.globalManager.GetPreference<PreferenceInt>("steamCloud").Value == 0},
			{typeof(StartMainMenu), true}
		};
		
		private static List<Type> visitedMenus = new List<Type>();

		public static Type GetNextMenu(Type currentMenu)
		{
			foreach (var menu in MenuLoadingOrder)
			{
				if (menu.Key == currentMenu) continue;
				if (!menu.Value || visitedMenus.Contains(menu.Key)) continue;
				
				visitedMenus.Add(menu.Key);
				return menu.Key;
			}

			return typeof(StartMainMenu);
		}
	}
	
	public enum GDOFailureState
	{
		FailedToConvert,
		FailedToAttachDependent,
		FailedToRegister
	}
	
	public enum ModFailureState
	{
		FailedToPostActivate,
		FailedToPreInject,
		FailedToPostInject
	}

	public struct FailedGDO
	{
		public CustomGameDataObject GDO;
		public GDOFailureState State;
		public Exception Exception;

		public FailedGDO(CustomGameDataObject gdo, GDOFailureState state, Exception e)
		{
			GDO = gdo;
			State = state;
			Exception = e;
		}
	}

	public struct FailedMod
	{
		public Mod Mod;
		public ModFailureState State;
		public Exception Exception;

		public FailedMod(Mod mod, ModFailureState state, Exception e)
		{
			Mod = mod;
			State = state;
			Exception = e;
		}
	}
}
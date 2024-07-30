using System;
using System.Collections.Generic;
using KitchenLib.Customs;
using KitchenLib.Preferences;
using KitchenLib.UI;
using KitchenLib.UI.PlateUp;
using KitchenMods;

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

		public static Type GetNextMenu(Type currentMenu)
		{
			if (currentMenu == null)
			{
				if (ErrorHandling.FailedGDOs.Count > 0)
					return typeof(FailedGDOsMenu);
				
				if (ErrorHandling.FailedMods.Count > 0)
					return typeof(FailedModsMenu);
				
				if (PreferenceManager.globalManager != null && PreferenceManager.globalManager.GetPreference<PreferenceInt>("steamCloud").Value == 0)
					return typeof(SaveDataDisclosure);
				
				return typeof(RevisedMainMenu);
			}
			
			if (currentMenu == typeof(FailedGDOsMenu))
			{
				if (ErrorHandling.FailedMods.Count > 0)
					return typeof(FailedModsMenu);
				
				if (PreferenceManager.globalManager != null && PreferenceManager.globalManager.GetPreference<PreferenceInt>("steamCloud").Value == 0)
					return typeof(SaveDataDisclosure);
				
				return typeof(RevisedMainMenu);
			}
			
			return typeof(RevisedMainMenu);
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
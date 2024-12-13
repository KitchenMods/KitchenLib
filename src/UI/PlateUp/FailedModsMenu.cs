using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	public class FailedModsMenu : KLMenu
	{
		public FailedModsMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}

		private Option<FailedMod> failedMods;
		
		public override void Setup(int player_id)
		{
			List<string> _failedMods = new List<string>();
			foreach (FailedMod failedMod in ErrorHandling.FailedMods)
			{
				_failedMods.Add(failedMod.Mod.Name);
			}
			failedMods = new Option<FailedMod>(ErrorHandling.FailedMods, ErrorHandling.FailedMods[0], _failedMods);
			failedMods.OnChanged += (sender, e) =>
			{
				Redraw(player_id);
			};
			Redraw(player_id);
		}

		public void Redraw(int player_id)
		{
			ModuleList.Clear();
			AddLabel($"{ErrorHandling.FailedMods.Count} Mods failed to load");
			AddSelect(failedMods);

			FailedMod selectedFailure = failedMods.GetOption(failedMods.Chosen);
			
			AddLabel($"Fail State: {selectedFailure.State}");
			AddInfo($"Exception: {selectedFailure.Exception.Message}");
				
			New<SpacerElement>(true);
			New<SpacerElement>(true);
			New<SpacerElement>(true);
				
			AddButton("Exit Game", delegate (int i)
			{
				Application.Quit();
			}, 0, 1f, 0.2f);
				
			AddButton("<color=red>Proceed Anyway", delegate (int i)
			{
				RequestSubMenu(ErrorHandling.GetNextMenu(GetType()));
			}, 0, 1f, 0.2f);
		}
	}
}

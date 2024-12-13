using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	public class FailedGDOsMenu : KLMenu
	{
		public FailedGDOsMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}

		private Option<FailedGDO> failedGDOS;
		
		public override void Setup(int player_id)
		{
			List<string> _failedGDOs = new List<string>();
			foreach (FailedGDO failedGDO in ErrorHandling.FailedGDOs)
			{
				_failedGDOs.Add($"{failedGDO.GDO.ModName}:{failedGDO.GDO.UniqueNameID}");
			}
			failedGDOS = new Option<FailedGDO>(ErrorHandling.FailedGDOs, ErrorHandling.FailedGDOs[0], _failedGDOs);
			failedGDOS.OnChanged += (sender, e) =>
			{
				Redraw(player_id);
			};
			Redraw(player_id);
		}

		public void Redraw(int player_id)
		{
			ModuleList.Clear();
			AddLabel($"{ErrorHandling.FailedGDOs.Count} GDOs failed to load");
			AddSelect(failedGDOS);

			FailedGDO selectedFailure = failedGDOS.GetOption(failedGDOS.Chosen);
			
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

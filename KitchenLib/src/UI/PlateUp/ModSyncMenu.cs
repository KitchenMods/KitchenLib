using System.Collections.Generic;
using Kitchen.Modules;
using Kitchen;
using KitchenLib.Views;
using KitchenMods;
using Steamworks.Ugc;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	internal class ModSyncMenu : KLMenu<PauseMenuAction>
	{
		public ModSyncMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		public override void Setup(int player_id)
		{
			Redraw();
		}
		
		private List<Steamworks.Ugc.Item> MissingMods = new List<Steamworks.Ugc.Item>();
		
		private async void Redraw()
		{
			ModuleList.Clear();
			if (SyncMods.MissingMods.Count == 0)
			{
				AddInfo("I have no clue why you're here, but you're missing no mods.");
			}
			else if (MissingMods.Count == 0)
			{
				foreach (ulong missingMod in SyncMods.MissingMods)
				{
					var mod = await Steamworks.Ugc.Item.GetAsync(missingMod);
					MissingMods.Add(mod.Value);
				}
				Redraw();
				return;
			}
			else
			{
				AddInfo("Would you like to install the following mods?");
				string label = "";
				int count = 0;
				
				foreach (Item mod in MissingMods)
				{
					AddLabel(mod.Title);
				}
				
				New<SpacerElement>(true);
				
				AddButton("Install", async delegate (int i)
				{
					ConfirmModSync.MissingMods = MissingMods;
					RequestSubMenu(typeof(ConfirmModSync));
				}, 0, 1f, 0.2f);
			}

			New<SpacerElement>(true);
			New<SpacerElement>(true);
			
			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
			ResetPanel();
		}
	}
}
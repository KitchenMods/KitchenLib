﻿using Kitchen.Modules;
using Kitchen;
using System.Collections.Generic;
using Steamworks.Ugc;
using UnityEngine;

namespace KitchenLib.UI
{
	internal class ConfirmModSync : KLMenu<PauseMenuAction>
	{
		public ConfirmModSync(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		public static List<Steamworks.Ugc.Item> MissingMods = new List<Steamworks.Ugc.Item>();
		public override void Setup(int player_id)
		{
			Redraw();
		}

		private bool ConfirmSync = false;
		private bool Complete = false;
		
		private async void Redraw()
		{
			ModuleList.Clear();
			if (!ConfirmSync && !Complete)
			{
				AddInfo("Are you SURE you would like to install these mods?");
				AddButton("Confirm", delegate (int i)
				{
					ConfirmSync = true;
					Redraw();
				}, 0, 1f, 0.2f);
			}else if (ConfirmSync && !Complete)
			{
				AddInfo("Installing.. Please wait a moment.");
				foreach (Item mod in MissingMods)
				{
					Main.LogInfo("Installing " + mod.Title);
					await mod.Subscribe();
				}
				Complete = true;
				Redraw();
				return;
			}else if (ConfirmSync && Complete)
			{
				ConfirmSync = false;
				Complete = false;
				AddInfo("Install Complete, please restart the game.");
				
				New<SpacerElement>(true);
				New<SpacerElement>(true);

				AddButton("Quit", delegate (int i)
				{
					Application.Quit();
				}, 0, 1f, 0.2f);
				ResetPanel();
				return;
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

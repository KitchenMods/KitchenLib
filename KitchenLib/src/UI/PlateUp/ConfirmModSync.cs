using Kitchen.Modules;
using Kitchen;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KitchenLib.UI
{
	internal class ConfirmModSync : KLMenu<PauseMenuAction>
	{
		public static Dictionary<ulong, Steamworks.Ugc.Item> Mods = new Dictionary<ulong, Steamworks.Ugc.Item>();
		private static List<string> ModNames = new List<string>();
		public ConfirmModSync(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		public override void Setup(int player_id)
		{
			StringBuilder builder = new StringBuilder();
			foreach (Steamworks.Ugc.Item item in Mods.Values)
			{
				builder.Append(", " + item.Title);
			}

			AddLabel("Are you sure you want to subscribe to these mods?");
			AddInfo(builder.ToString().Substring(2));
			
			New<SpacerElement>(true);
			New<SpacerElement>(true);
			
			AddButton("Confirm", delegate (int i)
			{
				foreach (Steamworks.Ugc.Item item in Mods.Values)
				{
					item.Subscribe();
				}
			}, 0, 1f, 0.2f);


			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}
	}
}

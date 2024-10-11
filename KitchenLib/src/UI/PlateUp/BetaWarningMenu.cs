using Kitchen;
using Kitchen.Modules;
using KitchenLib.Utils;
using Steamworks.Data;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	public class BetaWarningMenu : KLMenu
	{
		public BetaWarningMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		
		public override void Setup(int player_id)
		{
			Steamworks.Ugc.Item KLMain = new Steamworks.Ugc.Item(new PublishedFileId { Value = 2898069883 });
			Steamworks.Ugc.Item KLBeta = new Steamworks.Ugc.Item(new PublishedFileId { Value = 2932799348 });

			AddLabel("!! KitchenLib Beta Warning !!");
			AddInfo("You are running a beta version of KitchenLib.");
			AddInfo("This version may contain bugs and issues that are not present in the main branch.");
			AddInfo("Please report any issues you encounter to the mod author.");

			New<SpacerElement>();
			
			AddButton("<color=red>Continue With Beta", delegate (int i)
			{
				RequestSubMenu(ErrorHandling.GetNextMenu(GetType()));
			});
			
			AddButton("Return To Main Branch", async delegate (int i)
			{
				await KLBeta.Unsubscribe();
				await KLMain.Subscribe();
				Application.Quit(0);
			});
		}
	}
}

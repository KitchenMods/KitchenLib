using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	public class GameUpdateWarning : KLMenu
	{
		public GameUpdateWarning(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		
		public override void Setup(int player_id)
		{
			AddInfoText("<color=red>!! WARNING !!");
			AddInfoText("It looks like PlateUp! got an update, game updates can sometimes cause issues with mods. If you encounter any issues, please report them to the mod author.");
			New<SpacerElement>();
			New<SpacerElement>();
			AddButton("Okay Thanks!", delegate (int i)
			{
				Main.manager.GetPreference<PreferenceString>("lastVersionCheck").Set(Application.version);
				Main.manager.Save();
				RequestSubMenu(ErrorHandling.GetNextMenu(GetType()));
			});
		}
	}
}
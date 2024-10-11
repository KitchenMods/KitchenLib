using Kitchen;
using Kitchen.Modules;
using System.Collections.Generic;
using KitchenLib.Preferences;
using UnityEngine;

namespace KitchenLib.UI.PlateUp
{
	public class DeveloperOptions : KLMenu
	{
		public DeveloperOptions(Transform container, ModuleList module_list) : base(container, module_list) { }
		
		private Option<bool> forceLocalDishes = new Option<bool>(new List<bool>{true, false}, Main.manager.GetPreference<PreferenceBool>("forceLocalDishes").Value, new List<string>{"Enabled", "Disabled"});

		public override void Setup(int player_id)
		{
			AddLabel("Spawn Local Dishes (Requires Restart)");

			New<SpacerElement>(true);

			AddSelect(forceLocalDishes);
			forceLocalDishes.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("forceLocalDishes").Set(result);
			};
			New<SpacerElement>(true);
			New<SpacerElement>(true);

			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}
	}
}
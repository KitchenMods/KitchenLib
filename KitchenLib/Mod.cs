using UnityEngine;
using Kitchen.Modules;


namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib", "1.0.5") { }
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
		}

		public override void OnInitializeMelon()
		{
			/*
			BoolPreference x = PreferencesRegistry.Register<BoolPreference>("kitchenlib", "enabled", "Is Enabled");

            PreferencesRegistry.Load();
			Events.SetupEvent += (s, args) =>
			{
                args.Menu.AddNewButton(typeof(KLSettingsMenu<MainMenuAction>), "KL Settings");
			};
			Events.CreateSubMenusEvent += (s, args) =>
			{
                args.Menus.Add(typeof(KLSettingsMenu<MainMenuAction>), new KLSettingsMenu<MainMenuAction>(args.Container, args.Module_list));
			};
			*/
		}
  }

    

    public class KLSettingsMenu<T> : KLMenu<T>
    {
	    public KLSettingsMenu(Transform container, ModuleList module_list) : base(container, module_list)
	    {
	    }

	    public override void Setup(int player_id)
	    {
			/*
			* KitchenLib doesn't have any preferences at the moment, this is just a template for developers to follow.
			*/
			AddInfo("KitchenLib doesn't actually have any preferences... yet.");

            //AddLabel("Enable Mod");
            //BoolOption(PreferencesRegistry.Get<BoolPreference>("kitchenlib","enabled"));


            New<SpacerElement>();
		    New<SpacerElement>();
			/*
		    AddButton("Apply", delegate
		    {
                PreferencesRegistry.Save();
		    });
			*/
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate
		    {
			    RequestPreviousMenu();
		    });
	    }
    }
}

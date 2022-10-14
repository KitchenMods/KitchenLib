using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using KitchenLib.Event;
using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using KitchenLib.Utils;
using System.Reflection;


namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib", "1.0.5") { }
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
		}

		public override void OnInitializeMelon()
		{
            Mod.Log("um");
			TestA x = PreferencesRegistry.Register<TestA>("kitchenlib:settings", "KL Settings");
            x.myBool = true;
            x.myString = "sup";

			PreferencesRegistry.Save();
            PreferencesRegistry.Load();
			Events.SetupEvent += (s, args) =>
			{
                Mod.Log("Setup");
                args.Menu.AddNewButton(typeof(testmenu<MainMenuAction>), PreferencesRegistry.Get<TestA>("kitchenlib:settings").DisplayName);
			};
			Events.CreateSubMenusEvent += (s, args) =>
			{
                Mod.Log("Create");
                args.Menus.Add(typeof(testmenu<MainMenuAction>), new testmenu<MainMenuAction>(args.Container, args.Module_list));
			};
		}
  }
  class TestA : ModPreference {
        public string myString;
        public bool myBool;

	
	    public TestA() : base() { }
    
        public override void Serialize(BinaryWriter writer) {
            writer.Write(myString);
            writer.Write(myBool);
        }
    
        public override void Deserialize(BinaryReader reader) {
            myString = reader.ReadString();
            myBool = reader.ReadBoolean();
        }
    }
}

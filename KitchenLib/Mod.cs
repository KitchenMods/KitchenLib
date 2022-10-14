using System;
using System.Text;
using System.IO;
using System.Collections.Generic;


namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib", "1.0.5") { }
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
		}

		public override void OnInitializeMelon()
		{
			PreferencesRegistry.Register<TestA>("kitchenlib:settings", "KL Settings");
			PreferencesRegistry.Save();
            PreferencesRegistry.Load();
		}
  }
  class TestA : ModPreference {
    public string myString;
    public int myInt;
	
	public TestA() : base() { }
    
    public override void Serialize(BinaryWriter writer) {
        writer.Write(myString);
        writer.Write(myInt);
    }
    
    public override void Deserialize(BinaryReader reader) {
        myString = reader.ReadString();
        myInt = reader.ReadInt32();
    }
}
}

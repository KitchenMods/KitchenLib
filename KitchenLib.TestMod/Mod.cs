using KitchenLib.Appliances;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib.testmod", new string[] { "49FD" }) { }
        public static CustomAppliance d;
		public override void OnApplicationStart() {
			base.OnApplicationStart();
			d = AddAppliance<TestingTerminalAppliance>();
        }
    }
}
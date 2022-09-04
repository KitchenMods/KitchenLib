using KitchenLib.Appliances;
using KitchenLib.Utils;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib.testmod", ">=1.0.0 <=1.0.5") { }
        public static CustomAppliance d;
		public override void OnApplicationStart() {
			base.OnApplicationStart();
			d = AddAppliance<TestingTerminalAppliance>();
        }
    }
}
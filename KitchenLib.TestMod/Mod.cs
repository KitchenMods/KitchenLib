using KitchenLib.Appliances;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		public Mod() : base() { }
		
		public override void OnApplicationStart() {
			base.OnApplicationStart();
			AddAppliance<TestingTerminalAppliance>();
		}
	}
}
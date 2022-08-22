using KitchenLib.Appliances;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		public Mod() : base() {
			RegisterCustomAppliance(new CustomApplianceInfo() {
				Name            = "Testing Terminal",
				Description     = "This is how we test things, from a terminal!"
			});
		}
	}
}
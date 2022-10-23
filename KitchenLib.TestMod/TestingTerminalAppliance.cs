using KitchenData;
using KitchenLib.Customs;

namespace KitchenLib.TestMod
{
	public class TestingTerminalAppliance : CustomAppliance
	{
		public override string Name
		{
			get { return "Sushi Provider"; }
		}

		public override int BaseApplianceId
		{
			get { return -13481890; }
		}
    }
}

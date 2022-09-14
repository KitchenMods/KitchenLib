using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;

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
		
		public override List<IApplianceProperty> Properties
		{
			get { return new List<IApplianceProperty> { KitchenPropertiesUtils.GetUnlimitedCItemProvider(Mod.sushiRoll.ID) }; }
		}
    }
}

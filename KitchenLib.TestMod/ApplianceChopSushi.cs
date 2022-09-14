using KitchenLib.Customs;
using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.TestMod
{
	public class ApplianceChopSushi : CustomApplianceProcess
	{

		public override string ProcessName
		{
			get { return "ChopSushi"; }
		}

		public override Process Process
		{
			get { return Mod.rollProcess.Process; }
		}

        public override bool IsAutomatic
        {
            get { return false; }
        }

        public override float Speed
        {
            get { return 1f; }
        }

		public override void OnRegister(Appliance.ApplianceProcesses process)
		{
			GDOUtils.GetExistingAppliance(AssetReference.Counter).Processes.Add(process);
		}

	}
}

using KitchenLib.Customs;
using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.TestMod
{
	public class ChopSushi : CustomItemProcess
	{

		public override string ProcessName
		{
			get { return "ChopSushi"; }
		}

		public override Process Process
		{
			//get { return GDOUtils.GetExistingProcess("Chop"); }
			get { return Mod.rollProcess.Process; }
		}

		public override Item Result
		{
			get { return GDOUtils.GetExistingItem(-1631681807); }
		}

		public override float Duration
		{
			get { return 1f; }
		}

		public override bool IsBad
		{
			get { return false; }
		}

		public override bool RequiresWrapper
		{
			get { return false; }
		}

	}
}

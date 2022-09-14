using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomApplianceProcess
	{
		public virtual string ProcessName { get; internal set; }
		public virtual Process Process { get; internal set; }
		public virtual bool IsAutomatic { get; internal set; }
		public virtual float Speed { get; internal set; }
        public virtual ProcessValidity Validity {get; internal set;}

		public virtual void OnRegister(Appliance.ApplianceProcesses process) {}

	}
}

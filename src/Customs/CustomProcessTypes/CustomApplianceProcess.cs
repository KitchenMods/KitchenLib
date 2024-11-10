using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomApplianceProccess : CustomSubProcess
	{
		public virtual Process Process { get; protected set; }
		public virtual bool IsAutomatic { get; protected set; }
		public virtual float Speed { get; protected set; }
		public virtual ProcessValidity Validity { get; protected set; }

		public virtual void Convert(out Appliance.ApplianceProcesses applianceProcess)
		{
			Appliance.ApplianceProcesses result = new Appliance.ApplianceProcesses();

			if (result.Process != Process) result.Process = Process;
			if (result.IsAutomatic != IsAutomatic) result.IsAutomatic = IsAutomatic;
			if (result.Speed != Speed) result.Speed = Speed;
			if (result.Validity != Validity) result.Validity = Validity;

			applianceProcess = result;
		}
	}
}
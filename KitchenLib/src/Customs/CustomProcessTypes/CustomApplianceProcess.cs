using KitchenData;

namespace KitchenLib.Customs
{
    public abstract class CustomApplianceProcess : CustomSubProcess
    {
        public virtual Process Process { get; internal set; }
        public virtual bool IsAutomatic { get; internal set; }
        public virtual float Speed { get; internal set; }
        public virtual ProcessValidity Validity { get; internal set; }

        public override void Convert(out Appliance.ApplianceProcesses applianceProcess)
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
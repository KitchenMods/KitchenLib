using KitchenData;

namespace KitchenLib.Customs
{
    public abstract class CustomItemProcess : CustomSubProcess
    {
        public virtual Process Process { get; internal set; }
        public virtual Item Result { get; internal set; }
        public virtual float Duration { get; internal set; }
        public virtual bool IsBad { get; internal set; }
        public virtual bool RequiresWrapper { get; internal set; }

        public override void Convert(out Item.ItemProcess itemProcess)
        {
            Item.ItemProcess result = new Item.ItemProcess();

            if (result.Process != Process) result.Process = Process;
            if (result.Result != Result) result.Result = Result;
            if (result.Duration != Duration) result.Duration = Duration;
            if (result.IsBad != IsBad) result.IsBad = IsBad;
            if (result.RequiresWrapper != RequiresWrapper) result.RequiresWrapper = RequiresWrapper;

            itemProcess = result;
        } 
    }
}
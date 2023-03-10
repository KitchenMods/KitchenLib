using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomItemProcess : CustomSubProcess
	{
		public virtual Process Process { get; protected set; }
		public virtual Item Result { get; protected set; }
		public virtual float Duration { get; protected set; }
		public virtual bool IsBad { get; protected set; }
		public virtual bool RequiresWrapper { get; protected set; }

		public virtual void Convert(out Item.ItemProcess itemProcess)
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
using KitchenData;

namespace KitchenLib.JSON.Models.Containers
{
	public struct ApplianceProcessesContainer
	{
		public string Process;
		public bool IsAutomatic;
		public float Speed;
		public ProcessValidity Validity;

		public Appliance.ApplianceProcesses Convert()
		{
			return new Appliance.ApplianceProcesses()
			{
				Process = JSONPackUtils.GDOConverter<Process>(Process),
				IsAutomatic = IsAutomatic,
				Speed = Speed,
				Validity = Validity
			};
		}
	}
}

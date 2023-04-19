using KitchenData;

namespace KitchenLib.JSON.Models.Containers
{
	public class ApplianceProcessesContainer
	{
		public string Process;
		public bool IsAutomatic;
		public float Speed;
		public ProcessValidity Validity;

		public Appliance.ApplianceProcesses Convert()
		{
			Appliance.ApplianceProcesses processes = new Appliance.ApplianceProcesses();
			processes.Process = ContentPackPatches.GDOConverter<Process>(Process);
			processes.IsAutomatic = IsAutomatic;
			processes.Speed = Speed;
			processes.Validity = Validity;
			return processes;
		}
	}
}

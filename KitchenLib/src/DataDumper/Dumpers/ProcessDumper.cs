using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ProcessDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder processDump = new StringBuilder();

			processDump.AppendLine("ID,Type,BasicEnablingAppliance,EnablingApplianceCount,IsPseudoprocessFor,CanObfuscateProgress,Icon");

			foreach (Process process in GameData.Main.Get<Process>())
			{
				processDump.AppendLine($"{process.ID},{process.name},{process.BasicEnablingAppliance},{process.EnablingApplianceCount},{process.IsPseudoprocessFor},{process.CanObfuscateProgress},{process.Icon}");
			}

			SaveCSV("Process", "Processes", processDump);
		}
	}
}

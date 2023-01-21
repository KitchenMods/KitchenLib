using KitchenData;
using KitchenData.Workshop;
using System.Collections.Generic;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ContractDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder contractDump = new StringBuilder();

			contractDump.AppendLine("ID,Type,Status,ExperienceMultiplier,Name,Description");

			foreach (Contract contract in GameData.Main.Get<Contract>())
			{
				contractDump.AppendLine($"{contract.ID},{contract.name},{contract.Status},{contract.ExperienceMultiplier},{contract.Name},{contract.Description}");
			}

			SaveCSV("Contract", "Contracts", contractDump);
		}
	}
}

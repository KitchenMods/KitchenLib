using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class CrateSetDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("ID,Type,List<Appliance> Options");
			foreach (CrateSet crateSet in GameData.Main.Get<CrateSet>())
			{
				foreach (Appliance appliance in crateSet.Options)
				{
					sb.AppendLine($"{crateSet.ID},{crateSet.name},{appliance.ID}:{appliance.Name}");
				}
			}

			SaveCSV("CrateSet", "CrateSets", sb);
		}
	}
}

using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class CustomerGroupDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder customerGroupDump = new StringBuilder();

			customerGroupDump.AppendLine("ID,Type,Icon,R,G,B");

			foreach (CustomerGroup group in GameData.Main.Get<CustomerGroup>())
			{
				customerGroupDump.AppendLine($"{group.ID},{group.name},{group.Icon},{group.Colour.r},{group.Colour.g},{group.Colour.b}");
			}

			SaveCSV("CustomerGroup", "CustomerGroups", customerGroupDump);
		}
	}
}

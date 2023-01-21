using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class DecorDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("ID,Type,Material,ApplicatorAppliance,Type,IsAvailable");
			foreach (Decor decor in GameData.Main.Get<Decor>())
			{
				sb.AppendLine($"{decor.ID},{decor.name},{decor.Material},{decor.ApplicatorAppliance},{decor.Type},{decor.IsAvailable}");
			}

			SaveCSV("Decor", "Decors", sb);
		}
	}
}

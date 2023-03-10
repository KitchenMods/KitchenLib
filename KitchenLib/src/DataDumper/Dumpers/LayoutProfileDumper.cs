using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class LayoutProfileDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder layoutProfileDump = new StringBuilder();
			StringBuilder layoutProfileRequiredAppliancesDump = new StringBuilder();

			layoutProfileDump.AppendLine("ID,Type,Graph,MaximumTables,Table," +
				"Counter,ExternalBin,WallPiece,InternalWallPiece,StreetPiece," +
				"Name,Description");
			layoutProfileRequiredAppliancesDump.AppendLine("ID,Type,GameDataObject");

			foreach (LayoutProfile layoutProfile in GameData.Main.Get<LayoutProfile>())
			{
				layoutProfileDump.AppendLine($"{layoutProfile.ID},{layoutProfile.name},{layoutProfile.Graph},{layoutProfile.MaximumTables},{layoutProfile.Table}," +
					$"{layoutProfile.Counter},{layoutProfile.ExternalBin},{layoutProfile.WallPiece},{layoutProfile.InternalWallPiece},{layoutProfile.StreetPiece}," +
					$"{layoutProfile.Name},{layoutProfile.Description}");

				foreach (GameDataObject obj in layoutProfile.RequiredAppliances)
				{
					layoutProfileRequiredAppliancesDump.AppendLine($"{layoutProfile.ID},{layoutProfile.name},{obj.ID}:{obj.name}");
				}
			}

			SaveCSV("LayoutProfile", "LayoutProfiles", layoutProfileDump);
			SaveCSV("LayoutProfile", "LayoutProfileRequiredAppliances", layoutProfileRequiredAppliancesDump);
		}
	}
}

using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class CompositeUnlockPackDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder compositeUnlockPackDump = new StringBuilder();

			compositeUnlockPackDump.AppendLine("ID,Type,Pack");

			foreach (CompositeUnlockPack compositeUnlockPack in GameData.Main.Get<CompositeUnlockPack>())
			{
				foreach (UnlockPack pack in compositeUnlockPack.Packs)
				{
					compositeUnlockPackDump.AppendLine($"{compositeUnlockPack.ID},{compositeUnlockPack.name},{pack}");
				}
			}

			SaveCSV("CompositeUnlockPack", "CompositeUnlockPacks", compositeUnlockPackDump);
		}
	}
}

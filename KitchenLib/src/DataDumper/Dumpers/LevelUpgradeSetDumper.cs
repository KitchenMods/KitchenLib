using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class LevelUpgradeSetDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder layoutProfileDump = new StringBuilder();

			layoutProfileDump.AppendLine("ID,Type,Level,Upgrade");

			foreach (LevelUpgradeSet levelUpgradeSet in GameData.Main.Get<LevelUpgradeSet>())
			{
				foreach (LevelUpgrade upgrade in levelUpgradeSet.Upgrades)
				{
					layoutProfileDump.AppendLine($"{levelUpgradeSet.ID},{levelUpgradeSet.name},{upgrade.Level},{upgrade.Upgrade.ID}:{upgrade.Upgrade.MaximumUpgradeCount}:{upgrade.Upgrade.UpgradeName}:{upgrade.Upgrade.UpgradeDescription}");
				}
			}

			SaveCSV("LevelUpgradeSet", "LevelUpgradeSets", layoutProfileDump);

		}
	}
}

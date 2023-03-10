using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class FranchiseUpgradeDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder franchiseUpgradeDump = new StringBuilder();
			StringBuilder franchiseUpgradeUpgradesDump = new StringBuilder();

			franchiseUpgradeDump.AppendLine("ID,Type,MaximumUpgradeCount,Name,Description");
			franchiseUpgradeUpgradesDump.AppendLine("ID,Type,IFranchiseUpgrade");

			foreach (FranchiseUpgrade franchiseUpgrade in GameData.Main.Get<FranchiseUpgrade>())
			{
				franchiseUpgradeDump.AppendLine($"{franchiseUpgrade.ID},{franchiseUpgrade.name},{franchiseUpgrade.MaximumUpgradeCount},{franchiseUpgrade.Name},{franchiseUpgrade.Description}");
				foreach (IFranchiseUpgrade upgrade in franchiseUpgrade.Upgrades)
				{
					franchiseUpgradeUpgradesDump.AppendLine($"{franchiseUpgrade.ID},{franchiseUpgrade.name},{upgrade}");
				}
			}

			SaveCSV("FranchiseUpgrade", "FranchiseUpgrades", franchiseUpgradeDump);
			SaveCSV("FranchiseUpgrade", "FranchiseUpgradeUpgrades", franchiseUpgradeUpgradesDump);
		}
	}
}

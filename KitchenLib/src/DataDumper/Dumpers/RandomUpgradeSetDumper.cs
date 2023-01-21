using KitchenData;
using System.Collections.Generic;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class RandomUpgradeSetDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder randomUpgradeSetDump = new StringBuilder();
			StringBuilder randomUpgradeSetRewardsDump = new StringBuilder();

			randomUpgradeSetDump.AppendLine("ID,Type,Tier");
			randomUpgradeSetRewardsDump.AppendLine("ID,Type,ID,MaximumUpgradeCount,UpgradeName,UpgradeDescription");

			foreach (RandomUpgradeSet randomUpgradeSet in GameData.Main.Get<RandomUpgradeSet>())
			{
				randomUpgradeSetDump.AppendLine($"{randomUpgradeSet.ID},{randomUpgradeSet.name},{randomUpgradeSet.Tier}");
				foreach (IUpgrade upgrade in randomUpgradeSet.Rewards)
				{
					randomUpgradeSetRewardsDump.AppendLine($"{randomUpgradeSet.ID},{randomUpgradeSet.name},{upgrade.ID},{upgrade.MaximumUpgradeCount},{upgrade.UpgradeName},{upgrade.UpgradeDescription}");
				}
			}

			SaveCSV("RandomUpgradeSet", "RandomUpgradeSets", randomUpgradeSetDump);
			SaveCSV("RandomUpgradeSetRewards", "RandomUpgradeSetRewards", randomUpgradeSetRewardsDump);
		}
	}
}

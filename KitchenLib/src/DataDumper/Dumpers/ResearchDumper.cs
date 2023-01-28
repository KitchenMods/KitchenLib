using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ResearchDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder researchDump = new StringBuilder();
			StringBuilder researchRewardsDump = new StringBuilder();
			StringBuilder researchEnablesResearchOfDump = new StringBuilder();
			StringBuilder researchRequiresForResearchDump = new StringBuilder();

			researchDump.AppendLine("ID,Type,RequiredResearch");
			researchRewardsDump.AppendLine("ID,Type,ID,MaximumUpgradeCount,UpgradeName,UpgradeDescription");
			researchEnablesResearchOfDump.AppendLine("ID,Type,Research");
			researchRequiresForResearchDump.AppendLine("ID,Type,Research");

			foreach (Research research in GameData.Main.Get<Research>())
			{
				researchDump.AppendLine($"{research.ID},{research.name},{research.RequiredResearch}");
				foreach (IUpgrade researchReward in research.Rewards)
				{
					researchRewardsDump.AppendLine($"{research.ID},{research.name},{researchReward.ID},{researchReward.MaximumUpgradeCount},{researchReward.UpgradeName},{researchReward.UpgradeDescription}");
				}
				foreach (Research research2 in research.EnablesResearchOf)
				{
					researchEnablesResearchOfDump.AppendLine($"{research.ID},{research.name},{research2}");
				}
				foreach (Research research3 in research.RequiresForResearch)
				{
					researchRequiresForResearchDump.AppendLine($"{research.ID},{research.name},{research3}");
				}
			}

			SaveCSV("Research", "Research", researchDump);
			SaveCSV("Research", "ResearchRewards", researchRewardsDump);
			SaveCSV("Research", "ResearchEnablesResearchOf", researchEnablesResearchOfDump);
			SaveCSV("Research", "ResearchRequiresForResearch", researchRequiresForResearchDump);
		}
	}
}

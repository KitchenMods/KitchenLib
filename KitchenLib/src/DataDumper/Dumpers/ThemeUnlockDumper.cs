using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ThemeUnlockDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder themeUnlockDump = new StringBuilder();

			StringBuilder themeUnlockRequiresDump = new StringBuilder();
			StringBuilder themeUnlockBlockedByDump = new StringBuilder();

			themeUnlockDump.AppendLine("ID,Type,IsPrimary,Type,ParentTheme1,ParentTheme2,ExpReward,IsUnlockable,UnlockGroup,CardType," +
				"MinimumFranchiseTier,IsSpecificFranchiseTier,CustomerMultiplier,SelectionBias");

			themeUnlockRequiresDump.AppendLine("ID,Type,Unlock");
			themeUnlockBlockedByDump.AppendLine("ID,Type,Unlock");

			foreach (ThemeUnlock themeUnlock in GameData.Main.Get<ThemeUnlock>())
			{
				themeUnlockDump.AppendLine($"{themeUnlock.ID},{themeUnlock.name},{themeUnlock.IsPrimary},{themeUnlock.Type},{themeUnlock.ParentTheme1},{themeUnlock.ParentTheme2}," +
					$"{themeUnlock.ExpReward},{themeUnlock.IsUnlockable},{themeUnlock.UnlockGroup},{themeUnlock.CardType},{themeUnlock.MinimumFranchiseTier}," +
					$"{themeUnlock.IsSpecificFranchiseTier},{themeUnlock.CustomerMultiplier},{themeUnlock.SelectionBias}");

				foreach (Unlock unlock in themeUnlock.Requires)
				{
					themeUnlockRequiresDump.AppendLine($"{themeUnlock.ID},{themeUnlock.name},{unlock}");
				}

				foreach (Unlock unlock in themeUnlock.BlockedBy)
				{
					themeUnlockBlockedByDump.AppendLine($"{themeUnlock.ID},{themeUnlock.name},{unlock}");
				}
			}

			SaveCSV("ThemeUnlock", "ThemeUnlocks", themeUnlockDump);
			SaveCSV("ThemeUnlock", "ThemeUnlockRequires", themeUnlockRequiresDump);
			SaveCSV("ThemeUnlock", "ThemeUnlockBlockedBy", themeUnlockBlockedByDump);
		}
	}
}

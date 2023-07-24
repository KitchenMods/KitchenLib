using KitchenData;
using Shapes;
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
			StringBuilder themeAllowedFoodsDump = new StringBuilder();

			themeUnlockDump.AppendLine("ID,Type,IsPrimary,Type,ParentTheme1,ParentTheme2,ExpReward,IsUnlockable,UnlockGroup,CardType," +
				"MinimumFranchiseTier,IsSpecificFranchiseTier,CustomerMultiplier,SelectionBias,BlocksAllOtherFood,ForceFranchiseSetting");

			themeUnlockRequiresDump.AppendLine("ID,Type,Unlock");
			themeUnlockBlockedByDump.AppendLine("ID,Type,Unlock");
			themeAllowedFoodsDump.AppendLine("ID,Type,Unlock");

			foreach (ThemeUnlock themeUnlock in GameData.Main.Get<ThemeUnlock>())
			{
				themeUnlockDump.AppendLine($"{themeUnlock.ID},{themeUnlock.name},{themeUnlock.IsPrimary},{themeUnlock.Type},{themeUnlock.ParentTheme1},{themeUnlock.ParentTheme2}," +
					$"{themeUnlock.ExpReward},{themeUnlock.IsUnlockable},{themeUnlock.UnlockGroup},{themeUnlock.CardType},{themeUnlock.MinimumFranchiseTier}," +
				$"{themeUnlock.IsSpecificFranchiseTier},{themeUnlock.CustomerMultiplier},{themeUnlock.SelectionBias},{themeUnlock.BlocksAllOtherFood},{themeUnlock.ForceFranchiseSetting}");

				foreach (Unlock unlock in themeUnlock.Requires)
				{
					themeUnlockRequiresDump.AppendLine($"{themeUnlock.ID},{themeUnlock.name},{unlock}");
				}

				foreach (Unlock unlock in themeUnlock.BlockedBy)
				{
					themeUnlockBlockedByDump.AppendLine($"{themeUnlock.ID},{themeUnlock.name},{unlock}");
				}

				foreach (Unlock allowedFood in themeUnlock.AllowedFoods)
					themeAllowedFoodsDump.AppendLine($"{themeUnlock.ID},{themeUnlock.name},{allowedFood}");
			}

			SaveCSV("ThemeUnlock", "ThemeUnlocks", themeUnlockDump);
			SaveCSV("ThemeUnlock", "ThemeUnlockRequires", themeUnlockRequiresDump);
			SaveCSV("ThemeUnlock", "ThemeUnlockBlockedBy", themeUnlockBlockedByDump);
			SaveCSV("ThemeUnlock", "ThemeUnlockAllowedFoods", themeAllowedFoodsDump);
		}
	}
}

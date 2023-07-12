using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class UnlockCardDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder unlockCardDump = new StringBuilder();
			StringBuilder unlockCardEffectsDump = new StringBuilder();

			StringBuilder unlockCardRequiresDump = new StringBuilder();
			StringBuilder unlockCardBlockedByDump = new StringBuilder();
			StringBuilder unlockCardAllowedFoodsDump = new StringBuilder();

			unlockCardDump.AppendLine("ID,Type,ExpReward,IsUnlockable,UnlockGroup,CardType," +
				"MinimumFranchiseTier,IsSpecificFranchiseTier,CustomerMultiplier,SelectionBias,BlocksAllOtherFood,ForceFranchiseSetting");

			unlockCardEffectsDump.AppendLine("ID,Type,UnlockEffect");

			unlockCardRequiresDump.AppendLine("ID,Type,Unlock");
			unlockCardBlockedByDump.AppendLine("ID,Type,Unlock");
			unlockCardAllowedFoodsDump.AppendLine("ID,Type,Unlock");

			foreach (UnlockCard unlockCard in GameData.Main.Get<UnlockCard>())
			{
				unlockCardDump.AppendLine($"{unlockCard.ID},{unlockCard.name},{unlockCard.ExpReward},{unlockCard.IsUnlockable},{unlockCard.UnlockGroup},{unlockCard.CardType}," +
					$"{unlockCard.MinimumFranchiseTier},{unlockCard.IsSpecificFranchiseTier},{unlockCard.CustomerMultiplier},{unlockCard.SelectionBias},{unlockCard.BlocksAllOtherFood},{unlockCard.ForceFranchiseSetting}");

				foreach (UnlockEffect unlockEffect in unlockCard.Effects)
				{
					unlockCardEffectsDump.AppendLine($"{unlockCard.ID},{unlockCard.name},{unlockEffect}");
				}

				foreach (Unlock unlock in unlockCard.Requires)
				{
					unlockCardRequiresDump.AppendLine($"{unlockCard.ID},{unlockCard.name},{unlock}");
				}

				foreach (Unlock unlock in unlockCard.BlockedBy)
				{
					unlockCardBlockedByDump.AppendLine($"{unlockCard.ID},{unlockCard.name},{unlock}");
				}

				foreach (Unlock allowedFood in unlockCard.AllowedFoods)
					unlockCardAllowedFoodsDump.AppendLine($"{unlockCard.ID},{unlockCard.name},{allowedFood}");
			}

			SaveCSV("UnlockCard", "UnlockCards", unlockCardDump);
			SaveCSV("UnlockCard", "UnlockCardEffects", unlockCardEffectsDump);
			SaveCSV("UnlockCard", "UnlockCardRequires", unlockCardRequiresDump);
			SaveCSV("UnlockCard", "UnlockCardBlockedBy", unlockCardBlockedByDump);
			SaveCSV("UnlockCard", "UnlockCardAllowedFoods", unlockCardAllowedFoodsDump);
		}
	}
}

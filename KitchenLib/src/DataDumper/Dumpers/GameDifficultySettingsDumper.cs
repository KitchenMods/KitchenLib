using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class GameDifficultySettingsDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("ID,Type,IsActive,CustomersPerHourBase,CustomersPerHourIncreasePerDay,CustomerSideChance,QueuePatienceTime,QueuePatienceBoost,CustomerStarterChance,GroupDessertChance");
			foreach (GameDifficultySettings gameDifficultySettings in GameData.Main.Get<GameDifficultySettings>())
			{
				sb.AppendLine($"{gameDifficultySettings.ID},{gameDifficultySettings.name}," +
					$"{gameDifficultySettings.IsActive},{gameDifficultySettings.CustomersPerHourBase}," +
					$"{gameDifficultySettings.CustomersPerHourIncreasePerDay},{gameDifficultySettings.CustomerSideChance}," +
					$"{gameDifficultySettings.QueuePatienceTime},{gameDifficultySettings.QueuePatienceBoost}," +
					$"{gameDifficultySettings.CustomerStarterChance},{gameDifficultySettings.GroupDessertChance}");
			}

			SaveCSV("GameDifficultySettings", "GameDifficultySettings", sb);
		}
	}
}

using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class PlayerCosmeticDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder playerCosmeticDump = new StringBuilder();
			StringBuilder playerCosmeticCustomerSettingsDump = new StringBuilder();

			playerCosmeticDump.AppendLine("ID,Type,CosmeticType,DisableInGame,IsDefault,BlockHats,Visual");
			playerCosmeticCustomerSettingsDump.AppendLine("ID,Type,RestaurantSetting");


			foreach (PlayerCosmetic playerCosmetic in GameData.Main.Get<PlayerCosmetic>())
			{
				playerCosmeticDump.AppendLine($"{playerCosmetic.ID},{playerCosmetic.name},{playerCosmetic.CosmeticType},{playerCosmetic.DisableInGame},{playerCosmetic.IsDefault},{playerCosmetic.BlockHats},{playerCosmetic.Visual}");
				foreach (RestaurantSetting restaurantSetting in playerCosmetic.CustomerSettings)
				{
					playerCosmeticCustomerSettingsDump.AppendLine($"{playerCosmetic.ID},{playerCosmetic.name},{restaurantSetting}");
				}
			}

			SaveCSV("PlayerCosmetic", "PlayerCosmetics", playerCosmeticDump);
			SaveCSV("PlayerCosmetic", "PlayerCosmeticCustomerSettings", playerCosmeticCustomerSettingsDump);

		}
	}
}

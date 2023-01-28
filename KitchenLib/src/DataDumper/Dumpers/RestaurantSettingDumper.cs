using KitchenData;
using KitchenData.Workshop;
using System.Collections.Generic;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class RestaurantSettingDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder restaurantSettingDump = new StringBuilder();
			StringBuilder restaurantSettingDecoratorsDump = new StringBuilder();

			restaurantSettingDump.AppendLine("ID,Type,WeatherMode,UnlockPack,Name,Description");
			restaurantSettingDecoratorsDump.AppendLine("ID,Type,IDecorationConfiguration");

			foreach (RestaurantSetting restaurantSetting in GameData.Main.Get<RestaurantSetting>())
			{
				restaurantSettingDump.AppendLine($"{restaurantSetting.ID},{restaurantSetting.name},{restaurantSetting.WeatherMode},{restaurantSetting.UnlockPack},{restaurantSetting.Name},{restaurantSetting.Description}");
				foreach (IDecorationConfiguration decorator in restaurantSetting.Decorators)
				{
					restaurantSettingDecoratorsDump.AppendLine($"{restaurantSetting.ID},{restaurantSetting.name},{decorator}");
				}
			}

			SaveCSV("RestaurantSetting", "RestaurantSettings", restaurantSettingDump);
			SaveCSV("RestaurantSetting", "RestaurantSettingDecorators", restaurantSettingDecoratorsDump);
		}
	}
}

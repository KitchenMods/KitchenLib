using KitchenData;
using System.Collections;
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

			restaurantSettingDump.AppendLine("ID,Type,WeatherMode,UnlockPack,StartingUnlock,Prefab,Name,Description");
			restaurantSettingDecoratorsDump.AppendLine("ID,Type,IDecorationConfiguration");

			foreach (RestaurantSetting restaurantSetting in GameData.Main.Get<RestaurantSetting>())
			{
				restaurantSettingDump.AppendLine($"{restaurantSetting.ID},{restaurantSetting.name},{restaurantSetting.WeatherMode},{restaurantSetting.UnlockPack},{restaurantSetting.StartingUnlock},{restaurantSetting.Prefab.name},{restaurantSetting.Name},{restaurantSetting.Description}");
				foreach (IDecorationConfiguration decorator in restaurantSetting.Decorators)
				{
					restaurantSettingDecoratorsDump.AppendLine($"{restaurantSetting.ID},{restaurantSetting.name},{decorator}");
					Main.LogInfo("-------------------------------------");
					foreach (var field in decorator.GetType().GetFields())
					{
						// if field is list log all values in list without IList
						if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
						{
							Main.LogInfo("" + field.Name + " : " + field.GetValue(decorator));
							foreach (var item in (IList)field.GetValue(decorator))
							{
								Main.LogInfo("    " + item);
								foreach (var field2 in item.GetType().GetFields())
								{
									Main.LogInfo("        " + field2.Name + " : " + field2.GetValue(item));
								}
							}
						}
						else



							Main.LogInfo("" + field.Name + " : " + field.GetValue(decorator));
					}
				}
			}

			SaveCSV("RestaurantSetting", "RestaurantSettings", restaurantSettingDump);
			SaveCSV("RestaurantSetting", "RestaurantSettingDecorators", restaurantSettingDecoratorsDump);
		}
	}
}

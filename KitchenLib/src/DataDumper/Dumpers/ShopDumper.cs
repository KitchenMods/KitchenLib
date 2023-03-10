using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ShopDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder shopDump = new StringBuilder();
			StringBuilder shopStockDump = new StringBuilder();
			StringBuilder shopDecorDump = new StringBuilder();

			shopDump.AppendLine("ID,Type,Type,ItemsForSaleCount,WallpapersForSaleCount");

			foreach (Shop shop in GameData.Main.Get<Shop>())
			{
				shopDump.AppendLine($"{shop.ID},{shop.name},{shop.Type},{shop.ItemsForSaleCount},{shop.WallpapersForSaleCount}");

				foreach (Appliance stock in shop.Stock)
				{
					shopStockDump.AppendLine($"{shop.ID},{shop.name},{stock}");
				}

				foreach (Decor decor in shop.Decors)
				{
					shopDecorDump.AppendLine($"{shop.ID},{shop.name},{decor}");
				}
			}

			SaveCSV("Shop", "Shops", shopDump);
			SaveCSV("Shop", "ShopStocks", shopStockDump);
			SaveCSV("Shop", "ShopDecors", shopDecorDump);
		}
	}
}

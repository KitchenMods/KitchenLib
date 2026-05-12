using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomShop : CustomGameDataObject<Shop>
	{
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is not Shop shop)
			{
				return;
			}

			#region Apply Properties

			OverrideVariable(shop, "Type", Type);
			OverrideVariable(shop, "ItemsForSaleCount", ItemsForSaleCount);
			OverrideVariable(shop, "WallpapersForSaleCount", WallpapersForSaleCount);

			#endregion
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not Shop shop)
			{
				return;
			}

			#region Apply Properties

			OverrideVariable(shop, "Stock", Stock);
			OverrideVariable(shop, "Decors", Decors);

			#endregion
		}

		#region Base Game Variables

		public virtual List<Appliance> Stock { get; protected set; } = new();
		public virtual List<Decor> Decors { get; protected set; } = new();
		public virtual ShopType Type { get; protected set; }
		public virtual int ItemsForSaleCount { get; protected set; } = 3;
		public virtual int WallpapersForSaleCount { get; protected set; } = 6;

		#endregion
	}
}
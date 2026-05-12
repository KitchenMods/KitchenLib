using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomCustomerGroup : CustomUnlockCard
	{
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is CustomerGroup customerGroup)
			{
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is CustomerGroup customerGroup)
			{
			}
		}
	}
}
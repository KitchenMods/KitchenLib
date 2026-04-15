using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomEffectRepresentation : CustomLocalisedGameDataObject<EffectRepresentation, EffectInfo>
	{
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
		}
	}
}
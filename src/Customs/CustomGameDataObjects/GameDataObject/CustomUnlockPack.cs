using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomUnlockPack<T> : CustomGameDataObject<T> where T : UnlockPack
	{

		#region Base Game Variables

		public virtual bool OverrideIconToBeHeat { get; protected set; }

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not UnlockPack customUnlockPack) return;

			#region Apply Properties

			OverrideVariable(customUnlockPack, "OverrideIconToBeHeat", OverrideIconToBeHeat);

			#endregion
		}
	}
}
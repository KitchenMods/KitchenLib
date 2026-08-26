using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomUnlockCard : CustomUnlock<UnlockCard>
	{
		#region Base Game Variables

		public virtual List<UnlockEffect> Effects { get; protected set; } = new();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not UnlockCard unlockCard) return;

			#region Apply Properties

			OverrideVariable(unlockCard, "Effects", Effects);

			#endregion
		}
	}
}
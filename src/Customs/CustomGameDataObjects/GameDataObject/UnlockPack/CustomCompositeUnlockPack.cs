using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomCompositeUnlockPack : CustomUnlockPack<CompositeUnlockPack>
	{
		#region Base Game Variables

		public virtual List<UnlockPack> Packs { get; protected set; } = new();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not CompositeUnlockPack compositeUnlockPack) return;

			#region Apply Properties

			OverrideVariable(compositeUnlockPack, "Packs", Packs);

			#endregion
		}
	}
}
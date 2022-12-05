using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomCompositeUnlockPack : CustomUnlockPack
	{

		public virtual List<UnlockPack> Packs { get { return new List<UnlockPack>(); } }

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			CompositeUnlockPack result = new CompositeUnlockPack();
			CompositeUnlockPack empty = new CompositeUnlockPack();

			if (empty.ID != ID) result.ID = ID;

			if (empty.Packs != Packs) result.Packs = Packs;

			gameDataObject = result;
		}
	}
}

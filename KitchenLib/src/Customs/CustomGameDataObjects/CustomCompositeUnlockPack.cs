using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomCompositeUnlockPack : CustomUnlockPack
	{

		public virtual List<UnlockPack> Packs { get { return new List<UnlockPack>(); } }

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			CompositeUnlockPack result = ScriptableObject.CreateInstance<CompositeUnlockPack>();
			CompositeUnlockPack empty = ScriptableObject.CreateInstance<CompositeUnlockPack>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<CompositeUnlockPack>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;

			if (empty.Packs != Packs) result.Packs = Packs;

			gameDataObject = result;
		}
	}
}

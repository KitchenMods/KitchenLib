using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomCompositeUnlockPack : CustomUnlockPack<CompositeUnlockPack>
    {
        public virtual List<UnlockPack> Packs { get; protected set; } = new List<UnlockPack>();

        //private static readonly CompositeUnlockPack empty = ScriptableObject.CreateInstance<CompositeUnlockPack>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            CompositeUnlockPack result = ScriptableObject.CreateInstance<CompositeUnlockPack>();

			Main.LogDebug($"[CustomCompositeUnlockPack.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<CompositeUnlockPack>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            CompositeUnlockPack result = (CompositeUnlockPack)gameDataObject;

			Main.LogDebug($"[CustomCompositeUnlockPack.AttachDependentProperties] [1.1] Converting Base");

			if (result.Packs != Packs) result.Packs = Packs;
        }
    }
}

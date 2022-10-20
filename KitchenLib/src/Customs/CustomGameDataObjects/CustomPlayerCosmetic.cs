using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomPlayerCosmetic : CustomGameDataObject
    {
		public virtual CosmeticType CosmeticType { get; internal set; }
		public virtual GameObject Visual { get; internal set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            PlayerCosmetic result = new PlayerCosmetic();
            PlayerCosmetic empty = new PlayerCosmetic();

            if (empty.ID != ID) result.ID = ID;
            if (empty.CosmeticType != CosmeticType) result.CosmeticType = CosmeticType;

            gameDataObject = result ;
        }
    }
}
using KitchenData;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KitchenLib.Customs
{
    public abstract class CustomPlayerCosmetic : CustomGameDataObject
    {
		public virtual CosmeticType CosmeticType { get; internal set; }
        public virtual List<RestaurantSetting> CustomerSettings { get { return new List<RestaurantSetting>(); } }
        public virtual bool DisableInGame { get; internal set; }
        public virtual bool IsDefault { get; internal set; }
        public virtual bool BlockHats { get; internal set; }
		public virtual GameObject Visual { get; internal set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            PlayerCosmetic result = ScriptableObject.CreateInstance<PlayerCosmetic>();
			PlayerCosmetic empty = ScriptableObject.CreateInstance<PlayerCosmetic>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<PlayerCosmetic>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.CosmeticType != CosmeticType) result.CosmeticType = CosmeticType;
            if (empty.CustomerSettings != CustomerSettings) result.CustomerSettings = CustomerSettings;
            if (empty.DisableInGame != DisableInGame) result.DisableInGame = DisableInGame;
            if (empty.IsDefault != IsDefault) result.IsDefault = IsDefault;
            if (empty.BlockHats != BlockHats) result.BlockHats = BlockHats;
            if (empty.Visual != Visual) result.Visual = Visual;

            gameDataObject = result ;
        }
    }
}
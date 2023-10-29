using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomPlayerCosmetic : CustomLocalisedGameDataObject<PlayerCosmetic, CosmeticInfo>
    {
	    // Base-Game Variables
        public virtual CosmeticType CosmeticType { get; protected set; }
        public virtual List<RestaurantSetting> CustomerSettings { get; protected set; } = new List<RestaurantSetting>();
        public virtual bool DisableInGame { get; protected set; }
        public virtual bool IsDefault { get; protected set; }
        public virtual bool BlockHats { get; protected set; }
        public virtual GameObject Visual { get; protected set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            PlayerCosmetic result = ScriptableObject.CreateInstance<PlayerCosmetic>();

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<PlayerCosmetic>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.CosmeticType != CosmeticType) result.CosmeticType = CosmeticType;
            if (result.DisableInGame != DisableInGame) result.DisableInGame = DisableInGame;
            if (result.IsDefault != IsDefault) result.IsDefault = IsDefault;
            if (result.BlockHats != BlockHats) result.BlockHats = BlockHats;
            if (result.Visual != Visual) result.Visual = Visual;
            if (result.Info != Info) result.Info = Info;

            if (InfoList.Count > 0)
            {
                result.Info = new LocalisationObject<CosmeticInfo>();
                foreach ((Locale, CosmeticInfo) info in InfoList)
                    result.Info.Add(info.Item1, info.Item2);
            }

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            PlayerCosmetic result = (PlayerCosmetic)gameDataObject;

			if (result.CustomerSettings != CustomerSettings) result.CustomerSettings = CustomerSettings;
        }
    }
}
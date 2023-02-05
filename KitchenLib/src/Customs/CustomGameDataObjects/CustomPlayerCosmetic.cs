using KitchenData;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace KitchenLib.Customs
{
    public abstract class CustomPlayerCosmetic : CustomLocalisedGameDataObject<CosmeticInfo>
    {
        public virtual CosmeticType CosmeticType { get; protected set; }
        public virtual List<RestaurantSetting> CustomerSettings { get; protected set; } = new List<RestaurantSetting>();
        public virtual bool DisableInGame { get; protected set; }
        public virtual bool IsDefault { get; protected set; }
        public virtual bool BlockHats { get; protected set; }
        public virtual GameObject Visual { get; protected set; }

        private static readonly PlayerCosmetic empty = ScriptableObject.CreateInstance<PlayerCosmetic>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            PlayerCosmetic result = ScriptableObject.CreateInstance<PlayerCosmetic>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<PlayerCosmetic>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.CosmeticType != CosmeticType) result.CosmeticType = CosmeticType;
            if (empty.DisableInGame != DisableInGame) result.DisableInGame = DisableInGame;
            if (empty.IsDefault != IsDefault) result.IsDefault = IsDefault;
            if (empty.BlockHats != BlockHats) result.BlockHats = BlockHats;
            if (empty.Visual != Visual) result.Visual = Visual;
            if (empty.Info != Info) result.Info = Info;

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

            if (empty.CustomerSettings != CustomerSettings) result.CustomerSettings = CustomerSettings;
        }
    }
}
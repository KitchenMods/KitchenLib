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
        public virtual float HeadSize { get; protected set; } = 1f;
        public virtual bool HideBody { get; protected set; }
        public virtual GameObject Visual { get; protected set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            PlayerCosmetic result = ScriptableObject.CreateInstance<PlayerCosmetic>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "CosmeticType", CosmeticType);
            OverrideVariable(result, "DisableInGame", DisableInGame);
            OverrideVariable(result, "IsDefault", IsDefault);
            OverrideVariable(result, "BlockHats", BlockHats);
            OverrideVariable(result, "HeadSize", HeadSize);
            OverrideVariable(result, "HideBody", HideBody);
            OverrideVariable(result, "Visual", Visual);
            OverrideVariable(result, "Info", Info);

            if (InfoList.Count > 0)
            {
                SetupLocalisation<CosmeticInfo>(InfoList, ref result.Info);
            }

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            PlayerCosmetic result = (PlayerCosmetic)gameDataObject;

			OverrideVariable(result, "CustomerSettings", CustomerSettings);
        }
    }
}
using System.Collections.Generic;
using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomPlayerCosmetic : CustomLocalisedGameDataObject<PlayerCosmetic, CosmeticInfo>
	{

		#region Base Game Variables

		public virtual CosmeticType CosmeticType { get; protected set; }
		public virtual List<RestaurantSetting> CustomerSettings { get; protected set; } = new();
		public virtual bool DisableInGame { get; protected set; }
		public virtual bool IsDefault { get; protected set; }
		public virtual bool BlockHats { get; protected set; }
		public virtual float HeadSize { get; protected set; } = 1;
		public virtual bool HideBody { get; protected set; }
		public virtual GameObject Visual { get; protected set; }

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not PlayerCosmetic playerCosmetic) return;

			#region Apply Properties

			OverrideVariable(playerCosmetic, "CosmeticType", CosmeticType);
			OverrideVariable(playerCosmetic, "DisableInGame", DisableInGame);
			OverrideVariable(playerCosmetic, "IsDefault", IsDefault);
			OverrideVariable(playerCosmetic, "BlockHats", BlockHats);
			OverrideVariable(playerCosmetic, "HeadSize", HeadSize);
			OverrideVariable(playerCosmetic, "HideBody", HideBody);
			OverrideVariable(playerCosmetic, "Visual", Visual);
			OverrideVariable(playerCosmetic, "CustomerSettings", CustomerSettings);

			#endregion
		}
	}
}
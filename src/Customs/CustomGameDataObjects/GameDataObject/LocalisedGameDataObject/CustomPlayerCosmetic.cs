using System.Collections.Generic;
using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomPlayerCosmetic : CustomLocalisedGameDataObject<PlayerCosmetic, CosmeticInfo>
	{
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is PlayerCosmetic playerCosmetic)
			{
				#region Apply Properties

				OverrideVariable(playerCosmetic, "CosmeticType", CosmeticType);
				OverrideVariable(playerCosmetic, "DisableInGame", DisableInGame);
				OverrideVariable(playerCosmetic, "IsDefault", IsDefault);
				OverrideVariable(playerCosmetic, "BlockHats", BlockHats);
				OverrideVariable(playerCosmetic, "HeadSize", HeadSize);
				OverrideVariable(playerCosmetic, "HideBody", HideBody);
				OverrideVariable(playerCosmetic, "RequiresDLC", RequiresDLC);
				OverrideVariable(playerCosmetic, "Visual", Visual);

				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is PlayerCosmetic playerCosmetic)
			{
				#region Apply Properties

				OverrideVariable(playerCosmetic, "CustomerSettings", CustomerSettings);

				#endregion
			}
		}

		#region Base Game Variables

		public virtual CosmeticType CosmeticType { get; protected set; }
		public virtual List<RestaurantSetting> CustomerSettings { get; protected set; } = new();
		public virtual bool DisableInGame { get; protected set; }
		public virtual bool IsDefault { get; protected set; }
		public virtual bool BlockHats { get; protected set; }
		public virtual float HeadSize { get; protected set; } = 1;
		public virtual bool HideBody { get; protected set; }
		public virtual ContentPack RequiresDLC { get; protected set; }
		public virtual GameObject Visual { get; protected set; }

		#endregion
	}
}
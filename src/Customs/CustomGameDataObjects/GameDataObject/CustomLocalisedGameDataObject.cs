using System;
using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomLocalisedGameDataObject<T, L> : CustomGameDataObject<T> where T : GameDataObject where L : Localisation
	{
		#region Base Game Variables

		public virtual LocalisationObject<L> Info { get; protected set; }

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, L)> InfoList { get; protected set; } = new();

		#endregion

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is not LocalisedGameDataObject<L> localisedGameDataObject)
				return;
			
			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref localisedGameDataObject.Info);
			}
			else
			{
				OverrideVariable(localisedGameDataObject, "Info", Info);
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
		}
	}
}
using System.Collections.Generic;
using KitchenData;
using KitchenData.Localisations;

namespace KitchenLib.Customs
{
	public abstract class CustomRarityTierLocalisation : CustomLocalisationSet<RarityTierLocalisation, RarityTierInfo>
	{
		#region Base Game Variables
		
		public virtual LocalisationObject<RarityTierInfo> Info { get; protected set; }
		
		#endregion
		
		#region KitchenLib Variables

		public virtual List<(Locale, RarityTierInfo)> InfoList { get; protected set; } = new();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is RarityTierLocalisation rarityTierLocalisation))
				return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref rarityTierLocalisation.Info);
			}
			else
			{
				OverrideVariable(rarityTierLocalisation, "Info", Info);
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (!(gameDataObject is RarityTierLocalisation rarityTierLocalisation))
				return;
		}
	}
}
using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomDecorationLocalisation : CustomLocalisationSet<DecorationLocalisation, DecorationBonusInfo>
	{
		#region Base Game Variables
		
		public virtual LocalisationObject<DecorationBonusInfo> Info { get; protected set; }
		
		#endregion
		
		#region KitchenLib Variables

		public virtual List<(Locale, DecorationBonusInfo)> InfoList { get; protected set; } = new();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is DecorationLocalisation decorationLocalisation))
				return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref decorationLocalisation.Info);
			}
			else
			{
				OverrideVariable(decorationLocalisation, "Info", Info);
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (!(gameDataObject is DecorationLocalisation decorationLocalisation))
				return;
		}
	}
}
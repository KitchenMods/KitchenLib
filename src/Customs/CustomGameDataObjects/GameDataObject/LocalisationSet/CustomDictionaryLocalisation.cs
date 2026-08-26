using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomDictionaryLocalisation : CustomLocalisationSet<DictionaryLocalisation, DictionaryInfo>
	{
		#region Base Game Variables

		public virtual LocalisationObject<DictionaryInfo> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, DictionaryInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not DictionaryLocalisation dictionaryLocalisation) return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref dictionaryLocalisation.Info);
			}
			else if (Info != null)
			{
				OverrideVariable(dictionaryLocalisation, "Info", Info);
			}
		}
	}
}
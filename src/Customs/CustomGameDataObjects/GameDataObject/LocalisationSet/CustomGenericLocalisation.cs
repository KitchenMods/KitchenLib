using System.Collections.Generic;
using KitchenData;
using KitchenData.Localisations;

namespace KitchenLib.Customs
{
	public abstract class CustomGenericLocalisation<T> : CustomLocalisationSet<T, BasicInfo> where T : GameDataObject
	{
		#region Base Game Variables

		public virtual LocalisationObject<BasicInfo> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, BasicInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is GenericLocalisation genericLocalisation))
			{
				return;
			}

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref genericLocalisation.Info);
			}
			else if (Info != null)
			{
				OverrideVariable(genericLocalisation, "Info", Info);
			}
		}
	}
}
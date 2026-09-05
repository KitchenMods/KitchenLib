using System.Collections.Generic;
using KitchenData;
using KitchenData.Localisations;

namespace KitchenLib.Customs
{
	public abstract class CustomContractLocalisation : CustomLocalisationSet<ContractLocalisation, ContractInfo>
	{
		#region Base Game Variables

		public virtual LocalisationObject<ContractInfo> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, ContractInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not ContractLocalisation contractLocalisation) return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref contractLocalisation.Info);
			}
			else if (Info != null)
			{
				OverrideVariable(contractLocalisation, "Info", Info);
			}
		}
	}
}
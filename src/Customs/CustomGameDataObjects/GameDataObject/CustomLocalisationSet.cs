using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomLocalisationSet<T, L> : CustomGameDataObject<T> where T : GameDataObject where L : Localisation
	{
		#region Base Game Variables

		public virtual LocalisationObject<L> LocalisationInfo { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, L)> InfoList { get; protected set; } = new();

		#endregion
		
		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (!(gameDataObject is LocalisationSet<L> localisationSet))
			{
				return;
			}
			
			#region Apply Properties

			OverrideVariable(localisationSet, "LocalisationInfo", LocalisationInfo);

			#endregion
		}
	}
}
using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomContractLocalisation<T> : CustomLocalisationSet<T, ContractInfo> where T : GameDataObject
	{
		public virtual LocalisationObject<ContractInfo> Info { get; protected set; }
		public virtual List<(Locale, ContractInfo)> InfoList { get; protected set; } = new List<(Locale, ContractInfo)>();
	}
}

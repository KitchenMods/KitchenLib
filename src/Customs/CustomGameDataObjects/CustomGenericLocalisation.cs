using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomGenericLocalisation<T> : CustomLocalisationSet<T, BasicInfo> where T : GameDataObject
	{
		public virtual LocalisationObject<BasicInfo> Info { get; protected set; }
		public virtual List<(Locale, BasicInfo)> InfoList { get; protected set; } = new List<(Locale, BasicInfo)>();
	}
}

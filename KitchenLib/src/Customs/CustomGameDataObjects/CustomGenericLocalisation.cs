using KitchenData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenLib.Customs
{
	public abstract class CustomGenericLocalisation<T> : CustomLocalisationSet<T, BasicInfo> where T : GameDataObject
	{
		public virtual LocalisationObject<BasicInfo> Info { get; protected set; }
		public virtual List<(Locale, BasicInfo)> InfoList { get; protected set; } = new List<(Locale, BasicInfo)>();
	}
}
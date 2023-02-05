using KitchenData;
using System;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomLocalisedGameDataObject<T> : CustomGameDataObject where T : Localisation
    {
		[Obsolete("Please use InfoList.")]
        public virtual LocalisationObject<T> Info { get; protected set; }
		public virtual List<(Locale, T)> InfoList { get; protected set; } = new List<(Locale, T)>();
	}
}

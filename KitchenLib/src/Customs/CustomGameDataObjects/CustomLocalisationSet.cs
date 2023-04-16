using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomLocalisationSet<T, L> : CustomGameDataObject<T> where T : GameDataObject where L : Localisation
	{
	}
}

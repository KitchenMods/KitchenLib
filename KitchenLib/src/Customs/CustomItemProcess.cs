using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomItemProcess
	{
		public virtual string ProcessName { get; internal set; }
		public virtual Process Process { get; internal set; }
		public virtual Item Result { get; internal set; }
		public virtual float Duration { get; internal set; }
		public virtual bool IsBad { get; internal set; }
		public virtual bool RequiresWrapper { get; internal set; }

	}
}

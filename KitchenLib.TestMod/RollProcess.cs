using UnityEngine;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenLib.TestMod
{
	public class RollProcess : CustomProcess
	{
        public override string ProcessName
        {
            get
            {
                return "RollProcess";
            }
        }

        public override int BaseProcessId
        {
            get
            {
                return 2087693779;
            }
        }

        public override string Icon
        {
            get
            {
                return "<sprite=14>";
            }
        }

	}
}

using UnityEngine;
using KitchenLib.Customs;

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

        public override AudioClip ProcessAudioClip
        {
            get
            {
                return(AudioClip)Mod.bundle.LoadAsset("huh");
            }
        }

	}
}

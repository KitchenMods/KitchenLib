using KitchenLib.Customs;
using UnityEngine;
using System.Reflection;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		#if MELONLOADER
		public Mod() : base("kitchenlib.testmod", "1.1.0") { }
		#endif
		#if BEPINEX
		public Mod() : base("1.1.0", Assembly.GetCallingAssembly()) { }
		#endif
        public static CustomAppliance d;
		public static CustomItem sushiRoll;

		public static CustomProcess rollProcess;

		public static AssetBundle bundle;
 
    }
	/*
	* This is a custom AudioClip returned for the custom process 'rollProcess' - This needs to be added to KitchenLib
	*/
	
}
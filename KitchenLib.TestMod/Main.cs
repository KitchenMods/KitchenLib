using KitchenLib.Customs;
using UnityEngine;
using System.Reflection;
using System;
using KitchenMods;

namespace KitchenLib.TestMod
{
	public class Main : BaseMod
	{
		public Main() : base("kitchenlib.testmod", "KitchenLib TestMod", "KitchenMods", "0.1.0", "1.1.1", Assembly.GetExecutingAssembly()) { }

		protected override void OnFrameUpdate()
		{
			throw new NotImplementedException();
		}

		protected override void OnInitialise()
		{
			throw new NotImplementedException();
		}

		public override void PostActivate(Mod mod)
		{
			throw new NotImplementedException();
		}

		public override void PostInject()
		{
			throw new NotImplementedException();
		}

		public override void PreInject()
		{
			throw new NotImplementedException();
		}

		public static CustomAppliance d;
		public static CustomItem sushiRoll;

		public static CustomProcess rollProcess;

		public static AssetBundle bundle;
 
    }
	/*
	* This is a custom AudioClip returned for the custom process 'rollProcess' - This needs to be added to KitchenLib
	*/
	
}
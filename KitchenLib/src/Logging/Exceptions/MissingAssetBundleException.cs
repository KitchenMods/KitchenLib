using System;

namespace KitchenLib.Logging.Exceptions
{
	public class MissingAssetBundleException : Exception
	{
		public MissingAssetBundleException(string modid) : base(modid + " missing AssetBundle.") { }
	}
}

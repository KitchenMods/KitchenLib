using MelonLoader;
using System.Runtime.CompilerServices;
using KitchenLib.Registry;
using KitchenLib.Appliances;

namespace KitchenLib
{
	public abstract class BaseMod : MelonMod
	{
		public BaseMod() : base() {
			ModRegistery.Register(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Log(string message) {
			MelonLogger.Msg(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Error(string message) {
			MelonLogger.Error(message);
		}

		public int RegisterCustomAppliance(CustomApplianceInfo info) {
			info.ModName = Info.Name;
			return CustomAppliances.Register(info);
		}
	}
}

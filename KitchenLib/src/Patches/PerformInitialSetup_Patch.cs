using HarmonyLib;
using Kitchen;
using KitchenLib.Event;
using KitchenLib.Utils;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(GameCreator), "PerformInitialSetup")]
	public class PerformInitialSetup_Patch
	{
		static void Postfix()
		{
			EventUtils.InvokeEvent(nameof(Events.PerformInitialSetupEvent), Events.PerformInitialSetupEvent?.GetInvocationList(), null, new PerformInitialSetupEventArgs());
		}
	}
}

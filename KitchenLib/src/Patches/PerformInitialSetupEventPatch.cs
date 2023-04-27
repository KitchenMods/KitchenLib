using HarmonyLib;
using Kitchen;
using KitchenLib.Event;
using KitchenLib.Utils;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(GameCreator), "PerformInitialSetup")]
	internal class PerformInitialSetupEventPatch
	{
		static void Postfix()
		{
			EventUtils.InvokeEvent(nameof(Events.PerformInitialSetupEvent), Events.PerformInitialSetupEvent?.GetInvocationList(), null, new PerformInitialSetupEventArgs());
		}
	}
}

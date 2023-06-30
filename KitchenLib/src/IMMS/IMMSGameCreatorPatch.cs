using HarmonyLib;
using Kitchen;
using KitchenLib.IMMS;

namespace KitchenLib.IMMS
{
	[HarmonyPatch(typeof(GameCreator), "PerformInitialSetup")]
	internal class IMMSGameCreatorPatch
	{
		static void Postfix()
		{
			IMMSTarget.Setup();
		}
	}
}

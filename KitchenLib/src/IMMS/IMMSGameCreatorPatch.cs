using HarmonyLib;
using Kitchen;

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

using HarmonyLib;
using Kitchen;
using KitchenLib.IMMS;

namespace KitchenLib.src.IMMS
{
	[HarmonyPatch(typeof(GameCreator), "PerformInitialSetup")]
	class IMMSGameCreatorPatch
	{
		static void Postfix()
		{
			IMMSTarget.Setup();
		}
	}
}

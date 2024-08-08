using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Kitchen;
using KitchenLib.UI;

namespace KitchenLib.Patches
{
	
	[HarmonyPatch(typeof(StartMainMenu), "Setup")]
	public class StartMainMenuBetaInforPatch
	{
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new(instructions);

			matcher.MatchForward(false, new CodeMatch(OpCodes.Call), new CodeMatch(OpCodes.Callvirt), new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldarg_0))
				.Advance(2)
				.Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RevisedMainMenu), "AddBetaInfo")))
				.Insert(new CodeInstruction(OpCodes.Ldarg_0));
				
			return matcher.InstructionEnumeration();
		}
	}
	
	[HarmonyPatch(typeof(StartMainMenu), "Setup")]
	public class StartMainMenuPatch
	{
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new(instructions);

			matcher.MatchForward(false, new CodeMatch(OpCodes.Call), new CodeMatch(OpCodes.Ldc_I4_0), new CodeMatch(OpCodes.Callvirt), new CodeMatch(OpCodes.Pop), new CodeMatch(OpCodes.Call))
				.Advance(4)
				.Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RevisedMainMenu), "AddModMenus")))
				.Insert(new CodeInstruction(OpCodes.Ldarg_0));
				
			return matcher.InstructionEnumeration();
		}
	}
	
	
}
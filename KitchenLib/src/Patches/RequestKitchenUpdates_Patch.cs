using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Kitchen;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(RequestKitchenUpdates), "OnUpdate")]
	public class RequestKitchenUpdates_Patch
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{

			for (int i = 0; i < instructions.Count(); i++)
			{
				CodeInstruction prevInstruction = (i > 0) ? instructions.ElementAt(i - 1) : null;
				CodeInstruction instruction = instructions.ElementAt(i);
				CodeInstruction nextInstruction = (i < instructions.Count() - 1) ? instructions.ElementAt(i + 1) : null;
				CodeInstruction nextNextInstruction = (i < instructions.Count() - 2) ? instructions.ElementAt(i + 2) : null;
                
				if (prevInstruction != null && nextInstruction != null && instruction.opcode == OpCodes.Ldflda && prevInstruction.opcode == OpCodes.Ldarg && nextInstruction.opcode == OpCodes.Call && nextNextInstruction.opcode == OpCodes.Ldloca && nextNextInstruction.operand.ToString().Equals("Kitchen.CDishChoice (3)"))
				{
					yield return new CodeInstruction(OpCodes.Ldflda, AccessTools.Field(typeof(RequestKitchenUpdates), "_SingletonEntityQuery_SDishPedestal_8"));
				}
				else
				{
					yield return instruction;
				}

			}
		}
	}
}
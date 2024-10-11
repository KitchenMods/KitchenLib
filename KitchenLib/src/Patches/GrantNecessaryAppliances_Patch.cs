using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;

namespace KitchenLib.Patches
{
	/*
	[HarmonyPatch(typeof(GrantNecessaryAppliances), "OnUpdate")]
	internal class GrantNecessaryAppliances_Patch
	{
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new(instructions);
			
			matcher.MatchForward(false,
				new CodeMatch(OpCodes.Ldloc_S),
				new CodeMatch(OpCodes.Ldfld),
				new CodeMatch(OpCodes.Ldloca_S),
				new CodeMatch(OpCodes.Ldc_I4_0),
				new CodeMatch(OpCodes.Callvirt));

			CodeInstruction c_CProgressionUnlock = matcher.Instruction;

			matcher.MatchForward(false,
				new CodeMatch(OpCodes.Ldloca_S),
				new CodeMatch(OpCodes.Call),
				new CodeMatch(OpCodes.Ldfld),
				new CodeMatch(OpCodes.Ldc_I4_1),
				new CodeMatch(OpCodes.Bne_Un))
				.Advance(2)
				.InsertAndAdvance(new CodeInstruction(c_CProgressionUnlock.opcode, c_CProgressionUnlock.operand))
				.Set(OpCodes.Nop, null)
				.Advance(1)
				.Set(OpCodes.Call, AccessTools.Method(typeof(GrantNecessaryAppliances_Patch), nameof(RequirePlatesCheck)))
				.Advance(1)
				.Set(OpCodes.Brfalse_S, matcher.Operand);
			
			return matcher.InstructionEnumeration();
		}

		public static bool RequirePlatesCheck(Dish.MenuItem item, CProgressionUnlock unlock)
		{
			if (GDOUtils.GetCustomGameDataObject(unlock.ID) is not CustomDish customDish)
			{
				return item.Phase == MenuPhase.Main;
			}
			
			if (customDish.RequiredNoDishItem || customDish.RequiredDishItem != null)
			{
				return false;
			}
			
			return customDish.MinimumIngredients.All(minimumIngredient => minimumIngredient.ID != ItemReferences.Plate);
		}
	}
	*/
}

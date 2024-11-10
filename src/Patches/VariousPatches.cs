using System;
using System.Reflection;
using HarmonyLib;
using Kitchen;
using KitchenData;
using Unity.Entities;

namespace KitchenLib.Patches
{
	#region Main menu data collection indicator
	[HarmonyPatch(typeof(DisplayVersion), "Awake")]
	internal class DisplayVersion_Patch
	{
		public static void Postfix(DisplayVersion __instance)
		{
			if (!String.IsNullOrEmpty(Main.MOD_BETA_VERSION ))
			{
				__instance.Text.text = __instance.Text.text + "?";
			}
			else
			{
				__instance.Text.text = __instance.Text.text + "!";
			}

		}
	}
	#endregion
	
	
    /*
     * Thanks to IcedMilo for creating the patch
     *
     * DISABLED - *should* be in the base-game now.
     */

    /*
    [HarmonyPatch]
    static class ApplianceComponentHelpers_Patch
    {
	    [HarmonyTargetMethod]
	    static MethodBase ApplianceComponentSetDynamic_TargetMethod() =>
		    AccessTools.FirstMethod(typeof(ApplianceComponentHelpers), method => method.Name.Contains("SetDynamic") && method.IsGenericMethod).MakeGenericMethod(typeof(IApplianceProperty));

	    [HarmonyPrefix]
	    [HarmonyPriority(int.MinValue)]
	    static bool ApplianceComponentSetDynamic_Prefix(bool __runOriginal, EntityContext ctx, Entity e, IApplianceProperty component)
	    {
		    if (!__runOriginal || (component is IAttachmentLogic))
			    return true;

		    ctx.Set(e, (dynamic)component);

		    return false;
	    }
    }
    
    [HarmonyPatch]
    static class ItemComponentHelpers_Patch
    {
	    [HarmonyTargetMethod]
	    static MethodBase ItemComponentSetDynamic_TargetMethod() =>
		    AccessTools.FirstMethod(typeof(ItemComponentHelpers), method => method.Name.Contains("SetDynamic") && method.IsGenericMethod).MakeGenericMethod(typeof(IComponentData));

	    [HarmonyPrefix]
	    [HarmonyPriority(int.MinValue)]
	    static bool ItemComponentSetDynamic_Prefix(bool __runOriginal, EntityContext ctx, Entity e, IComponentData component)
	    {
		    if (!__runOriginal)
			    return true;

		    ctx.Set(e, (dynamic)component);

		    return false;
	    }
    }
    
    [HarmonyPatch(typeof(EffectHelpers))]
    static class EffectHelpers_Patch
    {
	    [HarmonyPatch(nameof(EffectHelpers.AddAttachedEffectComponents))]
	    [HarmonyPrefix]
	    [HarmonyPriority(int.MinValue)]
	    static bool AddAttachedEffectComponents_Prefix(bool __runOriginal, EntityCommandBuffer ecb, Entity e, Effect eff)
	    {
		    if (!__runOriginal || eff.Properties == null)
			    return true;

		    foreach (IEffectProperty property in eff.Properties)
		    {
			    ecb.AddComponent(e, (dynamic)property);
		    }

		    return false;
	    }

	    [HarmonyPatch(nameof(EffectHelpers.AddApplianceEffectComponents))]
	    [HarmonyPrefix]
	    [HarmonyPriority(int.MinValue)]
	    static bool AddApplianceEffectComponents_Prefix(bool __runOriginal, EntityCommandBuffer ecb, Entity e, IEffectPropertySource prop)
	    {
		    if (!__runOriginal || prop.EffectType == null || prop.EffectRange == null)
			    return true;

		    if (prop.EffectCondition != null)
			    ecb.AddComponent(e, (dynamic)prop.EffectCondition);
		    else 
			    ecb.AddComponent<CEffectAlways>(e);

		    ecb.AddComponent(e, (dynamic)prop.EffectType);
		    ecb.AddComponent(e, (dynamic)prop.EffectRange);

		    return true;
	    }
    }
    */
}
using HarmonyLib;
using Kitchen;

namespace KitchenLib
{
    [HarmonyPatch(typeof(DisplayVersion), "Awake")]
    public class DisplayVersion_Patch
    {
        public static void Postfix(DisplayVersion __instance)
        {
            __instance.Text.text = __instance.Text.text + ".";
        }
    }
}
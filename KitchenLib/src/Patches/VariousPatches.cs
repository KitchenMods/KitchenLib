using HarmonyLib;
using Kitchen.NetworkSupport;
using Kitchen;
using System.Collections.Generic;

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

    [HarmonyPatch(typeof(SteamRichPresenceView), "UpdateDiscordRichPresence")]
    public class SteamRichPresenceView_Patch
    {
        public static void Postfix(SteamRichPresenceView __instance, SteamRichPresenceView.ViewData view_data)
        {
#if MELONLOADER
            DiscordPlatform.Discord.SetActivity("Plating Up Some Spinach", "", view_data.Data.Players);
#endif

#if BEPINEX
            DiscordPlatform.Discord.SetActivity("Plating Up Some Burgers", "", view_data.Data.Players);
#endif

#if WORKSHOP
            DiscordPlatform.Discord.SetActivity("Plating Up Some Pizza", "", view_data.Data.Players);
#endif
		}
	}
    
}
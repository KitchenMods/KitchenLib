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
            DiscordPlatform.Discord.SetActivity("Plating Up Some Spinach", "", view_data.Data.Players);
        }
    }
    
    [HarmonyPatch(typeof(PackProgressionSaveSystem), "Serialise")]
    public class ProgressionSaveSystem_Patch
    {
        public static void Prefix(string path, PackSave save)
        {
            List<ISaveObject> toDelete = new List<ISaveObject>();
            foreach (ISaveObject obj in save.SaveObjects)
                if (obj.GetType() == typeof(PackSaveUpgrades.V1))
                    toDelete.Add(obj);
            foreach (ISaveObject obj in toDelete)
                if (((PackSaveUpgrades.V1)obj).ID == 82131534)
                    save.SaveObjects.Remove(obj);
        }
    }
    
}
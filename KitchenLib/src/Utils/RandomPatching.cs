using Kitchen;
using KitchenData;
using System.Reflection;
using HarmonyLib;
using KitchenLib.Utils;

namespace KitchenLib.src.Utils
{
    [HarmonyPatch(typeof(PlayerCosmeticSubview), "AddMissingAttachments")]
    class PlayerCosmeticSubview_Patch
    {
        static bool Prefix(PlayerCosmeticSubview __instance, PlayerCosmeticSubview.ViewData view_data)
        {
            int cosmeticID = 0;
            foreach (PlayerCosmetic cosmetic in GameData.Main.Get<PlayerCosmetic>())
            {
                if (cosmetic.Visual.name.ToLower().Contains("hot dog"))
                //if (cosmetic.Visual.name.ToLower().Contains("bunny"))
                {
                    cosmeticID = cosmetic.ID;
                }
            }
            PlayerCosmetic playerCosmetic;
            bool flag = GameData.Main.TryGet<PlayerCosmetic>(cosmeticID, out playerCosmetic, false);

			MethodInfo ca = ReflectionUtils.GetMethod<PlayerCosmeticSubview>("CleanAttachments");
			MethodInfo aa = ReflectionUtils.GetMethod<PlayerCosmeticSubview>("AddAttachment");
            ca.Invoke(__instance, new object[] { view_data });
            aa.Invoke(__instance, new object[] { playerCosmetic });

            return false;
        }
    }
}

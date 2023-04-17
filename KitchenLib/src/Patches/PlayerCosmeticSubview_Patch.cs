using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Patches
{

	[HarmonyPatch(typeof(PlayerCosmeticSubview), "AddAttachment")]
	public class PlayerCosmeticSubview_Patch
	{
		static void Prefix(PlayerCosmeticSubview __instance)
		{
			FieldInfo info = ReflectionUtils.GetField<PlayerCosmeticSubview>("AttachmentPoints");
			List<PlayerCosmeticSubview.AttachmentPoint> AttachmentPoints = (List<PlayerCosmeticSubview.AttachmentPoint>)info.GetValue(__instance);

			AttachmentPoints.Add(new PlayerCosmeticSubview.AttachmentPoint
			{
				Type = (CosmeticType)VariousUtils.GetID("Cape"),
				Transform = AttachmentPoints[0].Transform
			});

			info.SetValue(__instance, AttachmentPoints);
		}
	}
}

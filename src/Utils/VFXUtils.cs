using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace KitchenLib.Utils
{
	public static class VFXUtils
	{
		private static readonly Dictionary<string, VisualEffectAsset> VFXIndex = new Dictionary<string, VisualEffectAsset>();
		public static void SetupVFXIndex()
		{
			if (VFXIndex.Count > 0)
				return;

			foreach (VisualEffectAsset vfx in Resources.FindObjectsOfTypeAll(typeof(VisualEffectAsset)))
			{
				if (!VFXIndex.ContainsKey(vfx.name))
				{
					VFXIndex.Add(vfx.name, vfx);
				}
			}
		}
		
		public static VisualEffectAsset GetExistingVFX(string name)
		{
			if (VFXIndex.ContainsKey(name))
				return VFXIndex[name];
			
			return null;
		}

		public static GameObject AssignVFXByNames(this GameObject gameObject)
		{
			foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
			{
				if (child.gameObject.name.ToLower().StartsWith("vfx_"))
				{
					string vfx = child.gameObject.name.Split('_')[1];
					VisualEffectAsset vfxAsset = GetExistingVFX(vfx);
					if (vfxAsset != null)
					{
						if (child.gameObject.GetComponent<VisualEffect>() == null)
							child.gameObject.AddComponent<VisualEffect>();
						VisualEffect vfxComponent = child.gameObject.GetComponent<VisualEffect>();
						vfxComponent.visualEffectAsset = vfxAsset;
					}
					
				}
			}
			return gameObject;
		}
	}
}
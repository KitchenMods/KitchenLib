using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal class PlateUpSupportCape : BaseCape
	{
		public override string PrefabName => "PlateUp_Support_Cape";

		public override void OnRegister(PlayerCosmetic gameDataObject)
		{
			GameObject Prefab = gameDataObject.Visual;

			PlayerOutfitComponent playerOutfitComponent = Prefab.AddComponent<PlayerOutfitComponent>();
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "Cape").GetComponent<SkinnedMeshRenderer>());
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "PlateUpLogo").GetComponent<SkinnedMeshRenderer>());
		}
	}
}

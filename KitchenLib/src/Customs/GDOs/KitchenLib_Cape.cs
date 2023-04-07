using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class KitchenLib_Cape : CustomPlayerCosmetic
	{
		public override string UniqueNameID => "KitchenLib_Cape";
		public override CosmeticType CosmeticType => CosmeticType.Hat;
		public override GameObject Visual => Main.bundle.LoadAsset<GameObject>("KitchenLib_Cape");
		public override bool BlockHats => false;
		public override bool DisableInGame => true;

		public override void OnRegister(PlayerCosmetic gameDataObject)
		{
			GameObject Prefab = gameDataObject.Visual;

			PlayerOutfitComponent playerOutfitComponent = Prefab.AddComponent<PlayerOutfitComponent>();
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "Cape").GetComponent<SkinnedMeshRenderer>());
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "Hammers").GetComponent<SkinnedMeshRenderer>());
		}
	}
}

using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class PlateUp_Support_Cape : CustomPlayerCosmetic
	{
		public override string UniqueNameID => "PlateUp_Support_Cape";
		public override CosmeticType CosmeticType => CosmeticType.Hat;
		public override GameObject Visual => Main.bundle.LoadAsset<GameObject>("PlateUp_Support_Cape");
		public override bool BlockHats => false;
		public override bool DisableInGame => true;

		public override void OnRegister(PlayerCosmetic gameDataObject)
		{
			GameObject Prefab = gameDataObject.Visual;

			PlayerOutfitComponent playerOutfitComponent = Prefab.AddComponent<PlayerOutfitComponent>();
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "Cape").GetComponent<SkinnedMeshRenderer>());
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "PlateUpLogo").GetComponent<SkinnedMeshRenderer>());
		}
	}
}

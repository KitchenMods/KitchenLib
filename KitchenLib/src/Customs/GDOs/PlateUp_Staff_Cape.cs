using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class PlateUp_Staff_Cape : CustomPlayerCosmetic
	{
		public override string UniqueNameID => "PlateUp_Staff_Cape";
		public override CosmeticType CosmeticType => (CosmeticType)VariousUtils.GetID("Cape");
		public override GameObject Visual => Main.bundle.LoadAsset<GameObject>("PlateUp_Staff_Cape");
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

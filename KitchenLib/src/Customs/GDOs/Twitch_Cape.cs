using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class Twitch_Cape : CustomPlayerCosmetic
	{
		public override string UniqueNameID => "Twitch_Cape";
		public override CosmeticType CosmeticType => (CosmeticType)VariousUtils.GetID("Cape");
		public override GameObject Visual => Main.bundle.LoadAsset<GameObject>("Twitch_Cape");
		public override bool BlockHats => false;
		public override bool DisableInGame => true;

		public override void OnRegister(PlayerCosmetic gameDataObject)
		{
			GameObject Prefab = gameDataObject.Visual;

			PlayerOutfitComponent playerOutfitComponent = Prefab.AddComponent<PlayerOutfitComponent>();
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "Cape").GetComponent<SkinnedMeshRenderer>());
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "Twitch").GetComponent<SkinnedMeshRenderer>());
		}
	}
}

using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal abstract class BaseCape : CustomPlayerCosmetic
	{
		public abstract string CapeName { get; }
		public override string UniqueNameID => CapeName + "_Cape";
		public override CosmeticType CosmeticType => (CosmeticType)VariousUtils.GetID("Cape");
		public override GameObject Visual => Main.bundle.LoadAsset<GameObject>(CapeName + "_Cape");
		public override bool BlockHats => false;
		public override bool DisableInGame => true;

		public override void OnRegister(PlayerCosmetic gameDataObject)
		{
			Main.LogInfo($"Registering {CapeName} cape...");
			GameObject Prefab = gameDataObject.Visual;
			PlayerOutfitComponent playerOutfitComponent = Prefab.AddComponent<PlayerOutfitComponent>();
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, "Cape").GetComponent<SkinnedMeshRenderer>());
			playerOutfitComponent.Renderers.Add(GameObjectUtils.GetChildObject(Prefab, CapeName).GetComponent<SkinnedMeshRenderer>());
		}
	}
}

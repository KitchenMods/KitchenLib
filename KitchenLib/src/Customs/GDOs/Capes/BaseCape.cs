using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal abstract class BaseCape : CustomPlayerCosmetic
	{
		public abstract string PrefabName { get; }
		public override string UniqueNameID => PrefabName;
		public override CosmeticType CosmeticType => CosmeticType.Hat;
		public override GameObject Visual => Main.bundle.LoadAsset<GameObject>(PrefabName);
		public override bool BlockHats => false;
		public override bool DisableInGame => true;
	}
}

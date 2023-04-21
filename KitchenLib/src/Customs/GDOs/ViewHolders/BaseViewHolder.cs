using Kitchen;
using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal abstract class BaseViewHolder<T> : CustomAppliance where T : GenericObjectView
	{
		public abstract override string UniqueNameID { get; }
		public abstract override GameObject Prefab { get; }
		public override bool IsPurchasable => false;
		public override bool IsNonInteractive => true;

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<T>();
		}
	}
}

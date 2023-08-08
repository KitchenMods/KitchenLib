using Kitchen;
using KitchenLib.JSON.Models.Containers;
using UnityEngine;

namespace KitchenLib.JSON.Models.Views
{
	public class JsonItemGroupView : ItemGroupView
	{
		public void Setup(GameObject Prefab, ItemGroupViewContainer container)
		{
			container.Setup(Prefab, this);
		}
	}
}

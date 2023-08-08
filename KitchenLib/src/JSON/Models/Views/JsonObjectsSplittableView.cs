using Kitchen;
using KitchenLib.JSON.Models.Containers;
using UnityEngine;

namespace KitchenLib.JSON.Models.Views
{
	public class JsonObjectsSplittableView : ObjectsSplittableView
	{
		public void Setup(GameObject Prefab, ObjectsSplittableViewContainer container)
		{
			container.Setup(Prefab, this);
		}
	}
}

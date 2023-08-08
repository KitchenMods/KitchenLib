using Kitchen;
using KitchenLib.JSON.Models.Containers;
using UnityEngine;

namespace KitchenLib.JSON.Models.Views
{
	public class JsonPositionSplittableView : PositionSplittableView
	{
		public void Setup(GameObject Prefab, PositionSplittableViewContainer container)
		{
			container.Setup(Prefab, this);
		}
	}
}

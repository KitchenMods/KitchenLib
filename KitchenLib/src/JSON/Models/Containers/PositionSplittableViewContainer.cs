using KitchenLib.JSON.Models.Views;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
	public struct PositionSplittableViewContainer
	{
		public Vector3 FullPosition;
		public Vector3 EmptyPosition;
		public List<string> Objects;

		public PositionSplittableViewContainer() { }

		public void Setup(GameObject Prefab, JsonPositionSplittableView view)
		{
			FieldInfo fFullPosition = ReflectionUtils.GetField<JsonPositionSplittableView>("FullPosition");
			fFullPosition.SetValue(view, FullPosition);

			FieldInfo fEmptyPosition = ReflectionUtils.GetField<JsonPositionSplittableView>("EmptyPosition");
			fEmptyPosition.SetValue(view, EmptyPosition);

			FieldInfo fObjects = ReflectionUtils.GetField<JsonPositionSplittableView>("Objects");
			fObjects.SetValue(view, Objects.ConvertAll(_ => GameObjectUtils.GetChildObject(Prefab, _)));
		}
	}
}

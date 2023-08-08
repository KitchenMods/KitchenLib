using KitchenLib.JSON.Models.Views;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
	public struct ObjectsSplittableViewContainer
	{
		public List<string> Objects = new();

		public ObjectsSplittableViewContainer() { }

		public void Setup(GameObject Prefab, JsonObjectsSplittableView view)
		{
			FieldInfo fObjects = ReflectionUtils.GetField<JsonObjectsSplittableView>("Objects");
			fObjects.SetValue(view, Objects.ConvertAll(_ => GameObjectUtils.GetChildObject(Prefab, _)));
		}
	}
}

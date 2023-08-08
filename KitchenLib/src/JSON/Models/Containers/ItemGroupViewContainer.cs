using KitchenLib.JSON.Models.Views;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
	public struct ItemGroupViewContainer
	{
		public List<ComponentGroupContainer> ComponentGroups = new();

		public ItemGroupViewContainer() { }

		public void Setup(GameObject Prefab, JsonItemGroupView view)
		{
			if(ComponentGroups != null)
			{
				FieldInfo fComponentGroups = ReflectionUtils.GetField<JsonItemGroupView>("ComponentGroups");
				fComponentGroups.SetValue(view, ComponentGroups.Select(_ => _.Convert(Prefab)).ToList());
			}
		}
	}
}

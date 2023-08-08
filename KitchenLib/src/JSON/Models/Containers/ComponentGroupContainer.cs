using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
	public struct ComponentGroupContainer
	{
		public string Item;
		public string GameObject;
		public List<string> Objects;
		public bool DrawAll;

		public ItemGroupView.ComponentGroup Convert(GameObject Prefab)
		{
			return new ItemGroupView.ComponentGroup()
			{
				Item = JSONPackUtils.GDOConverter<Item>(Item),
				GameObject = GameObject != null ? GameObjectUtils.GetChildObject(Prefab, GameObject) : null,
				Objects = Objects?.Select(_ => GameObjectUtils.GetChildObject(Prefab, _)).ToList() ?? null,
				DrawAll = DrawAll
			};
		}
	}
}

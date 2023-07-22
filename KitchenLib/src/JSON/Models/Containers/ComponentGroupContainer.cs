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
		public List<string> Item;
		public string GameObject;
		public List<string> Objects;
		public bool DrawAll;


		public object Convert(GameObject Prefab)
		{
			if (Item.Count == 1)
			{
				return new ItemGroupView.ComponentGroup()
				{
					Item = JSONPackUtils.GDOConverter<Item>(Item[0]),
					GameObject = GameObject != null ? GameObjectUtils.GetChildObject(Prefab, GameObject) : null,
					Objects = Objects?.Select(_ => GameObjectUtils.GetChildObject(Prefab, _)).ToList() ?? null,
					DrawAll = DrawAll
				};
			}
			else
			{
				return new ComponentGroupCondition()
				{
					Item = Item.Select(_ => JSONPackUtils.GDOConverter<Item>(_)).ToList(),
					GameObject = GameObject != null ? GameObjectUtils.GetChildObject(Prefab, GameObject) : null,
					Objects = Objects?.Select(_ => GameObjectUtils.GetChildObject(Prefab, _)).ToList() ?? null,
					DrawAll = DrawAll
				};
			}

		}
	}

	public struct ComponentGroupCondition
	{
		public List<Item> Item;
		public GameObject GameObject;
		public List<GameObject> Objects;
		public bool DrawAll;
	}
}

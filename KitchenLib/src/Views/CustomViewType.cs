using Kitchen;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Views
{
	public sealed class CustomViewType
	{
		public static readonly SortedList<int, CustomViewType> Values = new();
		public static readonly Dictionary<CustomViewType, GameObject> Prefabs = new();

		public static readonly CustomViewType None = new CustomViewType(627000, (GameObject) null);

		private readonly int Value;

		public CustomViewType(int value, GameObject prefab)
		{
			Value = value;
			Values.Add(value, this);
			if (prefab != null)
			{
				Prefabs.Add(this, prefab);
			}
		}

		public CustomViewType(int value, Func<GameObject> prefab) : this(value, prefab.Invoke()) { }

		public static implicit operator CustomViewType(int value)
		{
			return Values[value];
		}

		public static implicit operator int(CustomViewType value)
		{
			return value.Value;
		}

		public static implicit operator CustomViewType(ViewType value)
		{
			if (Values.TryGetValue((int)value, out var res))
			{
				return res;
			}
			else
			{
				return None;
			}
		}

		public static implicit operator ViewType(CustomViewType value)
		{
			return (ViewType)value.Value;
		}
	}
}

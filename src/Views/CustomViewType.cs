using Kitchen;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Views
{
	public sealed class CustomViewType
	{
		internal static readonly SortedList<int, CustomViewType> Values = new();
		internal static readonly Dictionary<CustomViewType, Func<GameObject>> Prefabs = new();

		public static readonly CustomViewType None = new CustomViewType(627000, null);

		private readonly int Value;

		private CustomViewType(int value, Func<GameObject> prefab)
		{
			Value = value;
			Values.Add(value, this);
			if (prefab != null)
			{
				Prefabs.Add(this, prefab);
			}
		}

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

		internal static CustomViewType Register(string modId, string id, Func<GameObject> prefab)
		{
			int hash = StringUtils.GetInt32HashCode(modId + ":" + id);

			if (Values.ContainsKey(hash))
			{
				Main.LogInfo($"Error while registering custom view type of ID={modId}:{id}. Double-check to ensure that the ID is actually unique.");
				return null;
			}

			return new CustomViewType(hash, prefab);
		}
	}
}

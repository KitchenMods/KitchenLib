using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class ItemGroupViewUtils
	{
		private static readonly Type tComponentGroup = AccessTools.TypeByName("Kitchen.ItemGroupView+ComponentGroup");
		private static readonly Type tListComponentGroup = typeof(List<>).MakeGenericType(tComponentGroup);

		private static readonly FieldInfo fSubviewContainer = ReflectionUtils.GetField<ItemGroupView>("SubviewContainer");
		private static readonly FieldInfo fSubviewPrefab = ReflectionUtils.GetField<ItemGroupView>("SubviewPrefab");
		private static readonly FieldInfo fComponentGroups = ReflectionUtils.GetField<ItemGroupView>("ComponentGroups");
		private static readonly FieldInfo fGameObject = AccessTools.Field(tComponentGroup, "GameObject");
		private static readonly FieldInfo fItem = AccessTools.Field(tComponentGroup, "Item");

		private static readonly MethodInfo mAdd = AccessTools.Method(tListComponentGroup, "Add");

		private static readonly HashSet<int> SideItems = new HashSet<int>();

		public static void AddPossibleSide(GameData gameData, Item item)
		{
			if (SideItems.Contains(item.ID))
			{
				return;
			}

			ItemGroup platedBurgerItem = gameData.Get<ItemGroup>(ItemGroupReferences.BurgerPlated);
			ItemGroupView burgerItemGroupView = platedBurgerItem.Prefab.GetComponent<ItemGroupView>();
			GameObject sidePrefab = (GameObject)fSubviewPrefab.GetValue(burgerItemGroupView);
			ItemGroupView sideItemGroupView = sidePrefab.GetComponent<ItemGroupView>();

			var newComponentGroup = Activator.CreateInstance(tComponentGroup);
			GameObject newPrefab = null;

			if (CustomGDO.GDOs.ContainsKey(item.ID))
			{
				CustomItem customItem = CustomGDO.GDOs[item.ID] as CustomItem;
				if (customItem != null)
				{
					newPrefab = customItem.SidePrefab;
					if (newPrefab == null)
						newPrefab = item.Prefab;
				}
			}
			
			if (item.Prefab != null)
			{
				newPrefab = item.Prefab;
			}
			if (newPrefab == null)
			{
				Main.LogWarning($"Could not find side prefab for item {item.ID} ({item.name}).");
				return;
			}

			Transform transform = newPrefab.transform;
			transform.parent = sidePrefab.transform;
			transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y + 0.185f, transform.transform.localPosition.z); // Moving the prefab up ever so slightly
			fGameObject.SetValue(newComponentGroup, newPrefab);
			fItem.SetValue(newComponentGroup, item);

			var componentGroups = fComponentGroups.GetValue(sideItemGroupView);
			mAdd.Invoke(componentGroups, new object[] { newComponentGroup });

			SideItems.Add(item.ID);
			Main.LogInfo($"Added item prefab to side registry for item {item.ID} ({item.name}).");
		}

		public static void AddSideContainer<T>(GameData gameData, ItemGroup itemGroup, T localView) where T : ItemGroupView
		{
			//FieldInfo subviewContainer = ReflectionUtils.GetField<ItemGroupView>("SubviewContainer");
			//FieldInfo subviewPrefab = ReflectionUtils.GetField<ItemGroupView>("SubviewPrefab");
			Transform sidesContainer = null;
			if (itemGroup.Prefab != null)
				sidesContainer = itemGroup.Prefab.transform.Find("Side Container");

			if (sidesContainer != null)
			{
				fSubviewContainer.SetValue(localView, sidesContainer.gameObject);
				ItemGroup plated_burger = gameData.Get<ItemGroup>(ItemGroupReferences.BurgerPlated);
				ItemGroupView burgerView = plated_burger.Prefab.GetComponent<ItemGroupView>();
				fSubviewPrefab.SetValue(localView, fSubviewPrefab.GetValue(burgerView));
			}
			else
			{
				Main.LogWarning($"Could not find Side Container in prefab for ItemGroup {itemGroup.ID} ({itemGroup.name}).");
			}
		}

		public class DummyItemGroupView : ItemGroupView
		{
		}
	}
}

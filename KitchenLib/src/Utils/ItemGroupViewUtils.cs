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
            GameObject newPrefab = UnityEngine.Object.Instantiate((CustomGDO.GDOs[item.ID] as CustomItem)?.SidePrefab ?? item.Prefab);
            Transform transform = newPrefab.transform;
            transform.parent = sidePrefab.transform;
            fGameObject.SetValue(newComponentGroup, newPrefab);
            fItem.SetValue(newComponentGroup, item);

            var componentGroups = fComponentGroups.GetValue(sideItemGroupView);
            mAdd.Invoke(componentGroups, new object[] { newComponentGroup });

            SideItems.Add(item.ID);

            Main.instance.Log($"Added item prefab to side registry for item {item.ID} ({item.name}).");
        }

        public static void AddSideContainer<T>(GameData gameData, ItemGroup itemGroup, T localView) where T: ItemGroupView
        {
            FieldInfo subviewContainer = ReflectionUtils.GetField<ItemGroupView>("SubviewContainer");
            FieldInfo subviewPrefab = ReflectionUtils.GetField<ItemGroupView>("SubviewPrefab");
            Transform sidesContainer = null;
            if (itemGroup.Prefab != null)
                sidesContainer = itemGroup.Prefab.transform.Find("Side Container");

            if (sidesContainer != null)
            {
                subviewContainer.SetValue(localView, sidesContainer.gameObject);
                ItemGroup plated_burger = gameData.Get<ItemGroup>(ItemGroupReferences.BurgerPlated);
                ItemGroupView burgerView = plated_burger.Prefab.GetComponent<ItemGroupView>();
                subviewPrefab.SetValue(localView, subviewPrefab.GetValue(burgerView));
            }
            else
            {
                Main.instance.Log($"Could not find Side Container in prefab for ItemGroup {itemGroup.ID} ({itemGroup.name}).");
            }
        }
    }
}

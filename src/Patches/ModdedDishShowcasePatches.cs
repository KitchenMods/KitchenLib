using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Preferences;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Patches {

    [HarmonyPatch(typeof(MenuBackgroundItemScroller), "Update")]
    internal class MenuBackgroundItemScrollerUpdatePatch {

        private static readonly int REFRESH_COOLDOWN = 500;
        private static readonly int INITIAL_REFRESH_COOLDOWN = 0;
        private static readonly string BACKDROP_NAME = "Cube";

        private static int refreshCooldownRemaining = INITIAL_REFRESH_COOLDOWN;
        private static bool hasRebuiltItems = false;

        public static void Prefix(ref bool ___IsCreated, ref List<Item> ___Items, MenuBackgroundItemScroller __instance) {
			if (!Main.manager.GetPreference<PreferenceBool>("enableChangingMenu0160").Value)
			{
				return;
			}
            if (isMenuHidden(__instance)) {
                return;
            }

            if (refreshCooldownRemaining-- <= 0) {
                rebuildItemListWithModdedDishesIfNeeded(ref ___Items);
                ___Items.ShuffleInPlace();
                removeAllCurrentDishes(ref __instance);
                setIsCreatedToFalseToTriggerRedrawWithFreshItems(ref ___IsCreated);
                refreshCooldownRemaining = REFRESH_COOLDOWN;
            }
        }

        private static bool isMenuHidden(MenuBackgroundItemScroller menu) => !menu.Backdrop.activeInHierarchy;

        private static void rebuildItemListWithModdedDishesIfNeeded(ref List<Item> items)
		{
			if (hasRebuiltItems)
                return;
			if (GameData.Main == null)
				return;
			
			items = GameData.Main.Get<Dish>()
                .SelectMany(dish => dish.UnlocksMenuItems)
                .Select(menuItem => menuItem.Item)
                .ToList();

			hasRebuiltItems = true;
		}

        private static void removeAllCurrentDishes(ref MenuBackgroundItemScroller menu) {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in menu.transform) {
                if (child.name != BACKDROP_NAME) {
                    children.Add(child.gameObject);
                }
            }
            children.ForEach(Object.Destroy);
        }

        private static void setIsCreatedToFalseToTriggerRedrawWithFreshItems(ref bool isCreated) => isCreated = false;
    }

    [HarmonyPatch(typeof(MenuBackgroundItemScroller), "CreateItem")]
    internal class MenuBackgroundItemScrollerCreateItemPatch {

        public static void Postfix(ref GameObject __result) {
            //changeRotationSoItemsAreNotTopDown(__result);
            bringItemForwardSlightlyAfterRotationSoItDoesNotClipWithBackground(__result);
        }

        private static void changeRotationSoItemsAreNotTopDown(GameObject item) {
            item.transform.localRotation = Quaternion.Euler(new Vector3(-35, 0, 0));
        }

        private static void bringItemForwardSlightlyAfterRotationSoItDoesNotClipWithBackground(GameObject item) {
            item.transform.localPosition += new Vector3(0, 0, -1);
        }
    }
}

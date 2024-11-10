using System;
using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KitchenLib.Colorblind
{

	[Obsolete("Please use CustomItem.ColourBlindTag or CustomItemGroup.Labels instead.")]
	public static class ColorblindUtils
	{

		private static FieldInfo itemGroupView_colourblindLabel;
		private static FieldInfo itemGroupView_componentLabels;
		private static GameObject existingColourBlindChild;
		public static List<ItemLabel> itemLabels = new List<ItemLabel>();

		public static void Init(GameData gameData)
		{
			buildReflectionCache();
			getExistingColourBlindChildToCloneFromPie(gameData);
		}

		private static void buildReflectionCache()
		{
			itemGroupView_colourblindLabel = ReflectionUtils.GetField<ItemGroupView>("ColourblindLabel");
			itemGroupView_componentLabels = ReflectionUtils.GetField<ItemGroupView>("ComponentLabels");
		}

		private static void getExistingColourBlindChildToCloneFromPie(GameData gameData)
		{
			Item pie = gameData.Get<Item>(ItemReferences.PieMeatCooked);
			existingColourBlindChild = GameObjectUtils.GetChildObject(pie.Prefab, "Colour Blind");
		}

		public static void SetupColorBlindFeatureForItem(ItemLabelGroup itemLabelGroup)
		{
			Item item = GameData.Main.Get<Item>(itemLabelGroup.itemId);
			ItemGroupView itemGroupView = item.Prefab.GetComponent<ItemGroupView>();
			itemGroupView_componentLabels.SetValue(itemGroupView, ColourBlindLabelCreatorUtil.createLabelGroup(itemLabelGroup));

			if (doesColourBlindChildExist(item))
			{
				Debug.Log($"{itemLabelGroup.itemId} already has a Colour Blind child.");
				return;
			}

			GameObject clonedColourBlind = cloneColourBlindObjectAndAddToItem(item);
			setColourBlindLabelObjectOnItemGroupView(itemGroupView, clonedColourBlind);
		}

		public static void AddSingleItemLabels(ItemLabel[] itemLabels)
		{
			foreach (ItemLabel itemLabel in itemLabels)
			{
				if (!GameData.Main.TryGet<Item>(itemLabel.itemId, out Item item))
				{
					Debug.Log($"Colorblind error: Unable to find item for id {itemLabel.itemId}");
					continue;
				}

				GameObject clonedColourBlind = cloneColourBlindObjectAndAddToItem(item);
				TextMeshPro textMeshProObject = getTextMeshProFromClonedObject(clonedColourBlind);
				textMeshProObject.text = itemLabel.label;
			}
		}

		public static bool doesColourBlindChildExist(Item item)
		{
			return GameObjectUtils.GetChildObject(item.Prefab, "Colour Blind") != null;
		}

		public static GameObject cloneColourBlindObjectAndAddToItem(Item item)
		{
			GameObject clonedColourBlind = Object.Instantiate(existingColourBlindChild);
			clonedColourBlind.transform.Find("Title").GetComponent<TextMeshPro>().text = "" ;
			clonedColourBlind.name = "Colour Blind";
			clonedColourBlind.transform.SetParent(item.Prefab.transform);
			clonedColourBlind.transform.localPosition = new Vector3(0, 0, 0);
			return clonedColourBlind;
		}


		public static void setColourBlindLabelObjectOnItemGroupView(ItemGroupView itemGroupView, GameObject clonedColourBlind)
		{
			if (itemGroupView_colourblindLabel.GetValue(itemGroupView) == null)
			{
				TextMeshPro textMeshProObject = getTextMeshProFromClonedObject(clonedColourBlind);
				textMeshProObject.text = "";
				itemGroupView_colourblindLabel.SetValue(itemGroupView, textMeshProObject);
			}
		}

		public static TextMeshPro getTextMeshProFromClonedObject(GameObject clonedColourBlind)
		{
			GameObject titleChild = GameObjectUtils.GetChildObject(clonedColourBlind, "Title");
			return titleChild.GetComponent<TextMeshPro>();
		}
	}
}

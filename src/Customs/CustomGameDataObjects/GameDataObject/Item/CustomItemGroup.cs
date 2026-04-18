using System.Collections.Generic;
using System.Reflection;
using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using TMPro;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomItemGroup : CustomItemGroup<ItemGroupView> { }
	public abstract class CustomItemGroup<T> : CustomItem<ItemGroup> where T : ItemGroupView
	{
		#region Base Game Variables

		public virtual List<ItemGroup.ItemSet> Sets { get; protected set; } = new List<ItemGroup.ItemSet>();
		public virtual bool CanContainSide { get; protected set; }
		public virtual bool ApplyProcessesToComponents { get; protected set; }
		public virtual bool AllowLooseComponentSplitting { get; protected set; }
		public virtual bool AutoCollapsing { get; protected set; }
		public virtual List<ItemGroup.ItemReward> Rewards { get; protected set; } = new List<ItemGroup.ItemReward>();
		
		#endregion

		#region KitchenLib Variables

		public virtual bool AutoSetupItemGroupView { get; protected set; } = true;
		public virtual List<ItemGroupView.ColourBlindLabel> Labels { get; protected set; } = new List<ItemGroupView.ColourBlindLabel>();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is ItemGroup itemGroup)
			{
				#region Apply Properties

				OverrideVariable(itemGroup, "CanContainSide", CanContainSide);
				OverrideVariable(itemGroup, "ApplyProcessesToComponents", ApplyProcessesToComponents);
				OverrideVariable(itemGroup, "AllowLooseComponentSplitting", AllowLooseComponentSplitting);
				OverrideVariable(itemGroup, "AutoCollapsing", AutoCollapsing);
				
				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (gameDataObject is ItemGroup itemGroup)
			{
				#region Apply Properties

				OverrideVariable(itemGroup, "Sets", Sets);
				OverrideVariable(itemGroup, "Rewards", Rewards);
				
				#endregion
				
				if (AutoSetupItemGroupView)
				{
					Main.LogDebug($"Setting up ItemGroupView as {typeof(T).FullName}");
					T localView = itemGroup.Prefab.GetComponent<T>();
					if (localView == null)
						localView = itemGroup.Prefab.AddComponent<T>();

					if (CanContainSide)
					{
						ItemGroupViewUtils.AddSideContainer(gameData, itemGroup, localView);
					}
				}
				Item steak = (Item)GDOUtils.GetExistingGDO(ItemReferences.SteakMedium);
				if (steak != null)
				{
					Main.LogDebug($"Setting up Colour Blind Labels");
					GameObject ColorBlind = GameObject.Instantiate(steak.Prefab.transform.Find("Colour Blind").gameObject);
					ColorBlind.name = "Colour Blind";
					ColorBlind.transform.SetParent(itemGroup.Prefab.transform);
					ColorBlind.transform.localPosition = new Vector3(0, 0, 0);

					FieldInfo info = ReflectionUtils.GetField<T>("ColourblindLabel");
					T x = itemGroup.Prefab.GetComponent<T>();
					ColorBlind.transform.Find("Title").GetComponent<TextMeshPro>().text = "";
					info.SetValue(x, ColorBlind.transform.Find("Title").GetComponent<TextMeshPro>());

					if (Labels != null)
					{
						FieldInfo info2 = ReflectionUtils.GetField<T>("ComponentLabels");
						info2.SetValue(x, Labels);
					}
				}
			}
		}
	}
}
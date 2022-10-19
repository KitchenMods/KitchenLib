using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomItem
	{
		public virtual string Name { get { return "Item"; } }
		public virtual GameObject Prefab { get; internal set; }
		public virtual List<Item.ItemProcess> Processes { get; internal set; }
		public virtual List<Item.ItemProcess> DerivedProcesses { get; internal set; }
		public virtual List<IItemProperty> Properties { get; internal set; }
		public virtual float ExtraTimeGranted { get; internal set; }
		public virtual ItemValue ItemValue { get { return ItemValue.Small; } }
		public virtual int Reward { get { return 1; } }
		public virtual Item DirtiesTo { get; internal set; }
		public virtual List<Item> MayRequestExtraItems { get; internal set; }
		public virtual int MaxOrderSharers { get; internal set; }
		public virtual Item SplitSubItem { get; internal set; }
		public virtual int SplitCount { get { return 0; } }
		public virtual float SplitSpeed { get { return 1f; } }
		public virtual List<Item> SplitDepletedItems { get; internal set; }
		public virtual bool AllowSplitMerging { get; internal set; }
		public virtual bool PreventExplicitSplit { get; internal set; }
		public virtual bool SplitByComponents { get; internal set; }
		public virtual Item SplitByComponentsHolder { get; internal set; }
		public virtual bool SplitByCopying { get; internal set; }
		public virtual Item RefuseSplitWith { get; internal set; }
		public virtual Item DisposesTo { get; internal set; }
		public virtual bool IsIndisposable { get; internal set; }
		public virtual ItemCategory ItemCategory { get; internal set; }
		public virtual ItemStorage ItemStorageFlags { get; internal set; }
		public virtual Appliance DedicatedProvider { get; internal set; }
		public virtual ToolAttachPoint HoldPose { get { return ToolAttachPoint.Generic; } }
		public virtual bool IsMergeableSide { get; internal set; }
		
		public virtual int ID { get; internal set; }

		/*
         * Custom Fields
         */

		public string ModName { get; internal set; }
		public virtual int BaseItemId { get { return -1; } }
		public virtual int BasePrefabId { get { return BaseItemId; } }

		public Item Item{ get; internal set; }

		//Overridable methods

		public virtual void OnRegister(Item item) { }

		public int GetHash()
		{
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}

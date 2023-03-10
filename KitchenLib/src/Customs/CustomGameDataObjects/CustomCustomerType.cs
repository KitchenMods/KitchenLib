using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CustomCustomerType : CustomGameDataObject
	{
		public virtual bool IsGenericGroup { get; protected set; }
		public virtual bool RelativeGroupSize { get; protected set; }
		public virtual int MinGroupSize { get; protected set; }
		public virtual int MaxGroupSize { get; protected set; }
		public virtual PatienceValues PatienceModifiers { get; protected set; }
		public virtual OrderingValues OrderingModifiers { get; protected set; }
		public virtual List<PlayerCosmetic> Cosmetics { get; protected set; } = new List<PlayerCosmetic>();
		public virtual List<ICustomerProperty> Properties { get; protected set; } = new List<ICustomerProperty>();


		//private static readonly CustomerType empty = ScriptableObject.CreateInstance<CustomerType>();
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			CustomerType result = ScriptableObject.CreateInstance<CustomerType>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<CustomerType>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.IsGenericGroup != IsGenericGroup) result.IsGenericGroup = IsGenericGroup;
			if (result.RelativeGroupSize != RelativeGroupSize) result.RelativeGroupSize = RelativeGroupSize;
			if (result.MinGroupSize != MinGroupSize) result.MinGroupSize = MinGroupSize;
			if (result.MaxGroupSize != MaxGroupSize) result.MaxGroupSize = MaxGroupSize;
			if (!result.PatienceModifiers.Equals(PatienceModifiers)) result.PatienceModifiers = PatienceModifiers;
			if (!result.OrderingModifiers.Equals(OrderingModifiers)) result.OrderingModifiers = OrderingModifiers;
			if (result.Properties != Properties) result.Properties = Properties;

			gameDataObject = result;
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			CustomerType result = (CustomerType)gameDataObject;

			if (result.Cosmetics != Cosmetics) result.Cosmetics = Cosmetics;
		}
	}
}

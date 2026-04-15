using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomCustomerType : CustomGameDataObject<CustomerType>
	{
		#region Base Game Variables
		public virtual bool IsGenericGroup { get; protected set; }
		public virtual bool RelativeGroupSize { get; protected set; }
		public virtual int MinGroupSize { get; protected set; }
		public virtual int MaxGroupSize { get; protected set; }
		public virtual PatienceValues PatienceModifiers { get; protected set; }
		public virtual OrderingValues OrderingModifiers { get; protected set; }
		public virtual List<PlayerCosmetic> Cosmetics { get; protected set; } = new();
		public virtual List<ICustomerProperty> Properties { get; protected set; } = new();
		
		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is CustomerType customerType)
			{
				#region Apply Properties

				OverrideVariable(customerType, "IsGenericGroup", IsGenericGroup);
				OverrideVariable(customerType, "RelativeGroupSize", RelativeGroupSize);
				OverrideVariable(customerType, "MinGroupSize", MinGroupSize);
				OverrideVariable(customerType, "MaxGroupSize", MaxGroupSize);
				OverrideVariable(customerType, "PatienceModifiers", PatienceModifiers);
				OverrideVariable(customerType, "OrderingModifiers", OrderingModifiers);
				OverrideVariable(customerType, "Properties", Properties);
				
				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (gameDataObject is CustomerType customerType)
			{
				#region Apply Properties

				OverrideVariable(customerType, "Cosmetics", Cosmetics);
				
				#endregion
			}
		}
	}
}
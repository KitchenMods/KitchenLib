using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomCustomerType : CustomGameDataObject<CustomerType>
    {
	    // Base-Game Variables
        public virtual bool IsGenericGroup { get; protected set; }
        public virtual bool RelativeGroupSize { get; protected set; }
        public virtual int MinGroupSize { get; protected set; }
        public virtual int MaxGroupSize { get; protected set; }
        public virtual PatienceValues PatienceModifiers { get; protected set; }
        public virtual OrderingValues OrderingModifiers { get; protected set; }
        public virtual List<PlayerCosmetic> Cosmetics { get; protected set; } = new List<PlayerCosmetic>();
        public virtual List<ICustomerProperty> Properties { get; protected set; } = new List<ICustomerProperty>();
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            CustomerType result = ScriptableObject.CreateInstance<CustomerType>();

            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "IsGenericGroup", IsGenericGroup);
            OverrideVariable(result, "RelativeGroupSize", RelativeGroupSize);
            OverrideVariable(result, "MinGroupSize", MinGroupSize);
            OverrideVariable(result, "MaxGroupSize", MaxGroupSize);
            OverrideVariable(result, "PatienceModifiers", PatienceModifiers);
            OverrideVariable(result, "OrderingModifiers", OrderingModifiers);
            OverrideVariable(result, "Properties", Properties);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            CustomerType result = (CustomerType)gameDataObject;

			OverrideVariable(result, "Cosmetics", Cosmetics);
        }
    }
}

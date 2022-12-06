using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomEffectRepresentation : CustomGameDataObject
    {
		public virtual string Name { get; internal set; }
		public virtual string Description { get; internal set; }
		public virtual string Icon { get; internal set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            EffectRepresentation result = ScriptableObject.CreateInstance<EffectRepresentation>();
			EffectRepresentation empty = ScriptableObject.CreateInstance<EffectRepresentation>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<EffectRepresentation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.Name != Name) result.Name = Name;
            if (empty.Description != Description) result.Description = Description;
            if (empty.Icon != Icon) result.Icon = Icon;

            gameDataObject = result;
        }
    }
}
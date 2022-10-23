using KitchenData;

namespace KitchenLib.Customs
{
    public abstract class CustomEffectRepresentation : CustomGameDataObject
    {
		public virtual string Name { get; internal set; }
		public virtual string Description { get; internal set; }
		public virtual string Icon { get; internal set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            EffectRepresentation result = new EffectRepresentation();
            EffectRepresentation empty = new EffectRepresentation();
            
            if (empty.ID != ID) result.ID = ID;
            if (empty.Name != Name) result.Name = Name;
            if (empty.Description != Description) result.Description = Description;
            if (empty.Icon != Icon) result.Icon = Icon;

            gameDataObject = result;
        }
    }
}
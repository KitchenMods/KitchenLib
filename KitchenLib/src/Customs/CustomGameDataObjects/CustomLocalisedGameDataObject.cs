using KitchenData;

namespace KitchenLib.Customs
{
    public abstract class CustomLocalisedGameDataObject<T> : CustomGameDataObject where T : Localisation
    {
        public virtual LocalisationObject<T> Info { get; protected set; }
    }
}

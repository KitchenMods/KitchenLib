using System;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public abstract class GenericCustomConverter<T> : CustomConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
        }
    }
}

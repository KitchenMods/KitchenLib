using System;

namespace KitchenLib.src.JSON.JsonConverters
{
    public abstract class DerivedClassConverter<T> : CustomConverter<T>
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(T)) || objectType == typeof(T);
        }
    }
}

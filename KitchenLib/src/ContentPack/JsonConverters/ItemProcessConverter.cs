using KitchenData;
using Newtonsoft.Json.Converters;
using System;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class ItemProcessConverter : CustomCreationConverter<Item.ItemProcess>
    {
        public override Item.ItemProcess Create(Type objectType)
        {
            return new Item.ItemProcess();
        }
    }
}

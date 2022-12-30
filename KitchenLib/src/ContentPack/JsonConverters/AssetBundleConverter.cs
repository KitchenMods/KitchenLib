using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using UnityEngine;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class AssetBundleConverter : CustomCreationConverter<AssetBundle>
    {
        string relpath;

        public override AssetBundle Create(Type objectType)
        {
            return AssetBundle.LoadFromFile(Path.Combine(ContentPackManager.CurrentPack.ModDirectory, relpath));    
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            relpath = (string)reader.Value;
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}

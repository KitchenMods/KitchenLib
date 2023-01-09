using System.IO;
using UnityEngine;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class AssetBundleConverter : GenericCustomConverter<AssetBundle>
    {
        public override object Create(string str)
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(ContentPackManager.CurrentPack.ModDirectory, str));
            return bundle;
        }
    }
}

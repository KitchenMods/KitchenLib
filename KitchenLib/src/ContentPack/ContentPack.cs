using KitchenLib.src.ContentPack.Models;
using static KitchenLib.src.ContentPack.Logger;
using static KitchenLib.src.ContentPack.ContentPackManager;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.src.ContentPack
{
    public class ContentPack
    {
        public ModManifest Manifest;
        public ModContent Content;

        public string ModName;
        public string ModDirectory;
        public List<AssetBundle> Bundles;

        public void PreLoad()
        {
            ContentPackManager.ModName = ModName;
            ContentPackManager.Bundles = Bundles;
            ContentPackManager.ModDirectory = ModDirectory;

            Description = Manifest.Description;
            Author = Manifest.Author;
            Version = Manifest.Version;
            ContentPackFor = Manifest.ContentPackFor;

            Format = Content.Format;
            Changes = Content.Changes;
        }

        public void Load()
        {

            if (Content.Format.ComparePrecedenceTo(SemVersion) < 0)
            {
                Log($"{Author}.{ModName} is targeted towards an older version of KitchenLib");
            }

            foreach (ModChange change in Changes)
            {
                change.PreLoad();
                change.Load();
            }
        }

        public bool HasBundle()
        {
            return Bundles != null;
        }
    }
}

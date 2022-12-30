using KitchenLib.src.ContentPack.Models;
using Semver;
using System.Collections.Generic;
using UnityEngine;
using static KitchenLib.src.ContentPack.ContentPackUtils;

namespace KitchenLib.src.ContentPack
{
    public class ContentPack
    {
        public string ModDirectory { get; set; }
        public string ModName { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public SemVersion Version { get; set; }
        public ContentPackTarget ContentPackFor { get; set; }
        public SemVersion Format { get; set; }
        public AssetBundle Bundle { get; set; }
        public List<ModChange> Changes { get; set; }

        public void Load()
        {
            if (Format.ComparePrecedenceTo(Main.semVersion) < 0)
            {
                Log($"{Author}.{ModName} is targetted towards an older version of KitchenLib");
            }

            foreach (ModChange change in Changes)
            {
                ContentPackManager.CurrentChange = change;
                change.Load();
            }
        }

        public bool HasBundle()
        {
            return Bundle != null;
        }
    }
}

﻿using Kitchen;
using KitchenLib.Customs;
using KitchenLib.src.ContentPack.Models;
using KitchenLib.src.ContentPack.Models.Jsons;
using KitchenMods;
using Newtonsoft.Json;
using Semver;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static KitchenLib.src.ContentPack.ContentPackUtils;

namespace KitchenLib.src.ContentPack
{
    public class ContentPackSystem : GenericSystemBase, IModSystem
    {
        public static SemVersion Version = SemVersion.Parse("1.0.0", SemVersionStyles.Any);

        public static List<ContentPack> Packs = new List<ContentPack>();

        public ContentPackSystem() {}

        protected override void Initialise()
        {
            Main.instance.Log("Loading packs...");
            FindMods(FolderModSource.ModsFolder);
            FindMods(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..", "..", "workshop", "content", "1599600")));

            foreach (ContentPack pack in Packs)
            {
                foreach (var kvp in pack.changes)
                {
                    string key = kvp.Key;
                    SerializationContext value = kvp.Value;

                    string path = Path.GetFullPath(Path.Combine(pack.directory, key));
                    string json = pack.JSONs[path];

                    CustomGameDataObject CustomGDO = value switch
                    {
                        SerializationContext.Item => JsonConvert.DeserializeObject<JsonItem>(json, settings),
                        SerializationContext.ItemGroup => JsonConvert.DeserializeObject<JsonItemGroup>(json, settings),
                        _ => null
                    };
                    CustomGDO.ModName = pack.name;
                    Main.instance.Log($"Discovered {pack.customGDO.UniqueNameID}");
                    pack.customGDO = CustomGDO;
                }
            }
        }

        public override void PostInitialisation()
        {
            foreach (ContentPack pack in Packs)
            {
                Main.instance.Log($"Registering {pack.customGDO.UniqueNameID}");
                CustomGDO.RegisterGameDataObject(pack.customGDO);
            }
        }

        private static void FindMods(string dir)
        {
            Main.instance.Log("Searching for mods in " + dir);
            if (Directory.Exists(dir))
            {
                foreach (string subdirectory in Directory.GetDirectories(dir))
                {
                    ContentPack pack = LoadModFromFolder(subdirectory);
                    if (pack != null)
                    {
                        Packs.Add(pack);
                    }
                }
            }
        }

        private static ContentPack LoadModFromFolder(string dir)
        {
            string modname = Path.GetFileName(dir);
            List<string> files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories).ToList();

            if (files.Count == 0)
                return null;
            List<string> manifest = files.Where(x => Path.GetFileName(x) == "manifest.json").ToList();
            if (manifest.Count() == 0)
                return null;
            ModManifest Manifest = Deserialize<ModManifest>(File.ReadAllText(manifest[0]));
            List<string> content = files.Where(x => Path.GetFileName(x) == $"content.json").ToList();
            if (content.Count() == 0)
                return null;
            ModContent Content = Deserialize<ModContent>(File.ReadAllText(content[0]));
            ContentPack pack = new ContentPack(modname, dir, Manifest, Content);

            files = files.Where(x => Path.GetFileName(x) != "manifest.json" || Path.GetFileName(x) != $"content.json").ToList();
            foreach (string file in files)
            {
                pack.AddJson(file, File.ReadAllText(file));
            }
            return pack;
        }

        protected override void OnUpdate() { }
    }
}
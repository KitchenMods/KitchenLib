using KitchenLib.src.ContentPack.Models;
using KitchenMods;
using Newtonsoft.Json;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static KitchenLib.src.ContentPack.ContentPackUtils;
using static KitchenLib.src.ContentPack.Logger;

namespace KitchenLib.src.ContentPack
{
    public class ContentPackManager
    {
        public static SemVersion SemVersion = SemVersion.FromVersion(new Version(1, 0, 0));
        public static bool Debug = false;

        public static List<ContentPack> ContentPacks = new List<ContentPack>();

        // content pack
        public static string ModDirectory;
        // manifest
        public static string ModName;
        public static string Description;
        public static string Author;
        public static SemVersion Version;
        public static ContentPackTarget ContentPackFor;
        // content
        public static SemVersion Format;
        public static List<ModChange> Changes;
        public static List<AssetBundle> Bundles;
        // gdo
        public static SerializationContext Type;
        public static string BundleName;
        public static string Location;

        public static void Preload(string path = null)
        {
            // Get Directory Locations
            if(path is null)
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                path = Uri.UnescapeDataString(uri.Path);
            }

            string WorkshopModsPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path))), "workshop", "content", "1599600");

            string LocalModsPath = Path.Combine(Path.GetDirectoryName(path), "PlateUp", "Mods");

            // Workshop Mods
            SearchDirectories(WorkshopModsPath);

            // Local Mods
            SearchDirectories(LocalModsPath);
        }

        public static void Load()
        {
            foreach (ContentPack pack in ContentPacks) 
            {
                pack.PreLoad();
                pack.Load();
            }
        }

        public static void SearchDirectories(string path)
        {
            string[] modFolders = Directory.GetDirectories(path);
            foreach (string folder in modFolders)
            {
                ContentPack pack = new ContentPack();

                if (File.Exists(Path.Combine(folder, "manifest.json")))
                {
                    if (File.Exists(Path.Combine(folder, "content.json")))
                    {
                        try
                        {
                            string KitchenLibID = $"{Main.MOD_AUTHOR}.{Main.MOD_NAME}";

                            ModManifest Manifest = JsonConvert.DeserializeObject<ModManifest>(
                                File.ReadAllText(Path.Combine(folder, "manifest.json")), settings
                            );
                            if (Debug)
                                Log(JsonConvert.SerializeObject(Manifest, Formatting.Indented, settings));
                            if (!Manifest.ContentPackFor.isTargetFor(KitchenLibID))
                                continue;
                            Manifest.ModName = Path.GetFileName(folder.TrimEnd(Path.DirectorySeparatorChar));
                            pack.Manifest = Manifest;

                            ModContent Content = JsonConvert.DeserializeObject<ModContent>(
                                File.ReadAllText(Path.Combine(folder, "content.json")), settings
                            );
                            if (Debug)
                                Log(JsonConvert.SerializeObject(Content, Formatting.Indented, settings));
                            Content.Bundles = ModPreload.Mods.Where(mod => mod.Name == Manifest.ModName).First().GetPacks<AssetBundleModPack>().SelectMany(pack => pack.AssetBundles).ToList();
                            pack.Content = Content;

                            pack.ModDirectory = folder;

                            ContentPacks.Add(pack);
                        }
                        catch (Exception e)
                        {
                            Error(e.Message);
                            Error(e.StackTrace);
                        }
                    }
                    else
                    {
                        Error($"Found manifest.json at {folder} but missing content.json");
                    }
                }
            }
        }
    }
}

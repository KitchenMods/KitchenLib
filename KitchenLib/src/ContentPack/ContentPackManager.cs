using KitchenLib.src.ContentPack.Models;
using KitchenMods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static KitchenLib.src.ContentPack.ContentPackUtils;
using static KitchenLib.src.ContentPack.Logger;

namespace KitchenLib.src.ContentPack
{
    public class ContentPackManager
    {
        public static List<ContentPack> ContentPacks = new List<ContentPack>();

        public static ContentPack CurrentPack;
        public static ModChange CurrentChange;

        public static void Preload()
        {
            // Get Directory Locations
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            string WorkshopModsPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path))), "workshop", "content", "1599600");

            string LocalModsPath = Path.Combine(Path.GetDirectoryName(path), "PlateUp", "Mods");

            // Workshop Mods
            SearchDirectories(WorkshopModsPath);

            // Local Mods
            SearchDirectories(LocalModsPath);
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
                            
                            List<Mod> Mods = ModPreload.Mods;
                            string ModName = Path.GetFileName(folder.TrimEnd(Path.DirectorySeparatorChar));

                            ModManifest Manifest = JsonConvert.DeserializeObject<ModManifest>(
                                File.ReadAllText(Path.Combine(folder, "manifest.json")), settings
                            );
                            if (!Manifest.ContentPackFor.isTargetFor(KitchenLibID))
                                continue;

                            ModContent Content = JsonConvert.DeserializeObject<ModContent>(
                                File.ReadAllText(Path.Combine(folder, "content.json")), settings
                            );

                            pack.ModDirectory = folder;
                            pack.ModName = ModName;
                            pack.Description = Manifest.Description;
                            pack.Author = Manifest.Author;
                            pack.Version = Manifest.Version;
                            pack.ContentPackFor = Manifest.ContentPackFor;
                            pack.Format = Content.Format;
                            pack.Bundle = Mods.Where(mod => mod.Name == ModName).First().GetPacks<AssetBundleModPack>().SelectMany(pack => pack.AssetBundles).ToList()[0];
                            pack.Changes = Content.Changes;

                            settings.Context = new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Other, ModName);
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

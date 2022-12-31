using KitchenLib.src.ContentPack.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static KitchenLib.src.ContentPack.ContentPackUtils;
using static KitchenLib.src.ContentPack.Logger;

namespace KitchenLib.src.ContentPack
{
    internal class ContentPackManager
    {
        internal static List<ContentPack> ContentPacks = new List<ContentPack>();

        internal static ContentPack CurrentPack;
        internal static ModChange CurrentChange;

        internal static void Preload()
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

        private static void SearchDirectories(string path)
        {
            string KitchenLibID = $"{Main.MOD_AUTHOR}.{Main.MOD_NAME}";

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
                            ModManifest Manifest = JsonConvert.DeserializeObject<ModManifest>(
                                File.ReadAllText(Path.Combine(folder, "manifest.json")), settings
                            );
                            if (!Manifest.ContentPackFor.isTargetFor(KitchenLibID))
                                continue;

                            ModContent Content = JsonConvert.DeserializeObject<ModContent>(
                                File.ReadAllText(Path.Combine(folder, "content.json")), settings
                            );

                            pack.ModDirectory = folder;

                            pack.ModName = Manifest.ModName;
                            pack.Description = Manifest.Description;
                            pack.Author = Manifest.Author;
                            pack.Version = Manifest.Version;
                            pack.ContentPackFor = Manifest.ContentPackFor;

                            pack.Format = Content.Format;
                            pack.Bundle = Content.Bundle;
                            pack.Changes = Content.Changes;

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
                        Error($"{folder} missing content.json");
                    }
                }
                else
                {
                    Error($"{folder} missing manifest.json");
                }
            }
        }
    }
}

using KitchenLib.Customs;
using KitchenLib.src.ContentPack.Models;
using KitchenLib.src.ContentPack.Models.Jsons;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using static KitchenLib.src.ContentPack.ContentPackUtils;

namespace KitchenLib.src.ContentPack
{
    public class ContentPack
    {
        // Info
        public string name;
        public string directory;

        // Manifest
        public ModManifest manifest;
        public string description;
        public string author;
        public SemVersion version;

        // Content
        public ModContent content;
        public SemVersion format;
        public Dictionary<string, SerializationContext> changes { get; set; }

        // ContentPack
        public Dictionary<string, string> JSONs = new Dictionary<string, string>();
        public CustomGameDataObject customGDO;

        public ContentPack(string name, string directory, ModManifest manifest, ModContent content)
        {
            this.name = name;
            this.directory = directory;

            this.manifest = manifest;
            this.description = manifest.Description;
            this.author = manifest.Author;
            this.version = manifest.Version;

            this.content = content;
            this.format = content.Format;
            this.changes = content.Changes;
        }

        public void AddJson(string path, string json)
        {
            JSONs.Add(path, json);
        }

        public string PrintJson()
        {
            return string.Join(Environment.NewLine, JSONs);
        }

        public override string ToString()
        {
            return $"{name}:{Serialize(manifest)}{Serialize(content)}";
        }
    }
}

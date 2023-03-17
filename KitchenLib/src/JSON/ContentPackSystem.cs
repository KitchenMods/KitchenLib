using KitchenLib.Customs;
using KitchenMods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Entities;
using Newtonsoft.Json.Linq;
using Kitchen;
using KitchenLib.src.JSON.Models.Containers;
using System.Dynamic;
using KitchenLib.Event;
using System.Reflection;
using KitchenData;
using KitchenLib.Utils;
using System;

namespace KitchenLib.src.JSON
{
    [UpdateAfter(typeof(BaseMod))]
    public class ContentPackSystem : GenericSystemBase, IModSystem
    {
        public static Dictionary<string, JObject> RawJObjects = new Dictionary<string, JObject>();
        public static List<JObject> Mods = new List<JObject>();

        public ContentPackSystem() 
        {
            Events.BuildGameDataEvent += ModifyExistingGDOs;
        }

        protected override void Initialise()
        {
            Main.instance.Log("Loading packs...");
            FindMods(FolderModSource.ModsFolder);
            FindMods(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..", "..", "workshop", "content", "1599600")));
        }

        public override void PostInitialisation()
        {
            foreach (var kvp in RawJObjects)
            {
                string modname = kvp.Key;
                JObject jObject = kvp.Value;

                if (jObject.TryGetValue("JSONType", out JToken value))
                {
                    JsonTypeContainer jsonTypeContainer = value.ToObject<JsonTypeContainer>();
                    ContentPackUtils.SerializationContext Context = jsonTypeContainer.Context;
                    ChangeType Type = jsonTypeContainer.Type;

                    JsonSerializer serializer = JsonSerializer.Create(ContentPackUtils.settings);
                    JTokenReader reader = new JTokenReader(jObject);
                    switch (Type)
                    {
                        case ChangeType.CustomGDO:
                            CustomGameDataObject customGDO = serializer.Deserialize(reader, ContentPackUtils.JSONType[Context]) as CustomGameDataObject;
                            customGDO.ModName = modname;
                            Main.instance.Log($"Registering {customGDO.UniqueNameID}");
                            CustomGDO.RegisterGameDataObject(customGDO);
                            break;

                        case ChangeType.ModifyGDO:
                            JObject properties = jObject.ToObject<JObject>();
                            Mods.Add(properties);
                            break;
                    }
                }
            }
        }

        private void ModifyExistingGDOs(object sender, BuildGameDataEventArgs e)
        {
            Main.instance.Log("Modifying Existing GDOs");
            foreach (JObject jObject in Mods)
            {
                int ID = jObject["ID"].Value<int>();
                jObject.Remove("ID");
                JsonSerializer serializer = JsonSerializer.Create(ContentPackUtils.settings);
                JTokenReader reader = new JTokenReader(jObject["JSONType"]);
                JsonTypeContainer jsonTypeContainer = serializer.Deserialize<JsonTypeContainer>(reader);
                jObject.Remove("JSONType");

                GameDataObject gdo = e.gamedata.Get(ID);
                foreach (var kvp in jObject)
                {
                    string key = kvp.Key;
                    JToken value = kvp.Value;
                    Main.instance.Log(key);
                    Main.instance.Log(value.GetType());
                }

                Main.instance.Log($"Applying Modifications to {gdo.name}");
            }
        }

        private void FindMods(string dir)
        {
            BaseMod.instance.Log("Searching for mods in " + dir);
            if (Directory.Exists(dir))
            {
                foreach (string subdirectory in Directory.GetDirectories(dir))
                {
                    LoadModFromFolder(subdirectory);
                }
            }
        }

        private void LoadModFromFolder(string dir)
        {
            string modname = Path.GetFileName(dir);
            List<string> files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories).ToList();
            foreach (string file in files)
            {
                JObject jObject = JObject.Parse(File.ReadAllText(file));
                RawJObjects.Add(modname, jObject);
            }
        }

        protected override void OnUpdate() { }
    }
}

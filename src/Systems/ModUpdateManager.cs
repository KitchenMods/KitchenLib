using HarmonyLib;
using Kitchen;
using KitchenLib.UI;
using KitchenLib.Utils;
using KitchenMods;
using Newtonsoft.Json;
using Steamworks;
using Steamworks.Ugc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;

namespace KitchenLib.Systems
{
    internal struct DataFile
    {
        public Dictionary<ulong, long> data;
    }

    internal struct UpdatedMod
    {
        public ulong Id;
        public string Name;
        public string Version;
        public string Content;
        public long Timestamp;
    }

    internal struct OutOfDateMod
    {
        public ulong ModId;
        public string Name;
    }

    internal class ModUpdateManager : FranchiseFirstFrameSystem, IModSystem
    {
        private static readonly Regex CHANGELOG_REGEX = new Regex(@"<p id=""\d+"">(.+)</p>", RegexOptions.IgnoreCase);
        private readonly string DATA_FILE_PATH = Path.Combine(Application.persistentDataPath, "UserData/KitchenLib/modUpdateCache.json");

        public static ModUpdateManager Instance;

        private DataFile FileData = new()
        {
            data = new()
        };
        private Dictionary<ulong, long> PreviousData => FileData.data;

        protected override void Initialise()
        {
            base.Initialise();
            Instance = this;
        }

        protected override void OnUpdate()
        {
            LookForUpdates();
        }

        public async void LookForUpdates()
        {
            List<UpdatedMod> updatedMods = new();
            List<OutOfDateMod> outOfDateMods = new();

            var fInitializers = ReflectionUtils.GetField<AssemblyModPack>("Initializers");
            var allMods = ModPreload.Mods.ToDictionary(
                mod => mod,
                mod => mod.GetPacks<AssemblyModPack>()
                          .SelectMany(modPack => ((List<IModInitializer>)fInitializers.GetValue(modPack))
	                          .OfType<BaseMod>())
                          .ToList()
            );

            foreach (var possibleMods in allMods)
            {
                foreach (var baseMod in possibleMods.Value)
                {
                    Main.LogInfo($"[Update Manager] Found mod: {possibleMods.Key.Name} {baseMod.ModName}");
                }
            }

            var subbedItemsQuery = Query.ItemsReadyToUse.WhereUserSubscribed(SteamClient.SteamId.AccountId);
            var page = await subbedItemsQuery.GetPageAsync(1);
            if (page != null)
            {
                foreach (var entry in page.Value.Entries)
                {
                    var id = entry.Id.Value;
                    var name = entry.Title;
                    var timestamp = entry.Updated.Ticks;
                    var changelogUrl = entry.ChangelogUrl;
                    string version = null;
                    var changelog = await GetLatestUpdateChangelog(changelogUrl);

                    Main.LogInfo($"[Update Manager] Found subscribed workshop mod: {id} {name} {timestamp} {changelogUrl}");

                    if (!entry.NeedsUpdate)
                    {
                        // Mod is updated
                        bool foundBaseMod = false;
                        foreach (var possibleMods in allMods)
                        {
                            if (possibleMods.Key.Name == id.ToString())
                            {
                                foreach (var baseMod in possibleMods.Value)
                                {
                                    Main.LogInfo($"[Update Manager] Found KL mod '{baseMod.ModName}'");
                                    updatedMods.Add(new UpdatedMod
                                    {
                                        Id = id,
                                        Name = baseMod.ModName,
                                        Version = baseMod.ModVersion,
                                        Content = changelog,
                                        Timestamp = timestamp
                                    });

                                    foundBaseMod = true;
                                }
                            }
                        }

                        if (!foundBaseMod)
                        {
                            Main.LogInfo($"[Update Manager] Found non-KL mod '{name}'");
                            updatedMods.Add(new UpdatedMod
                            {
                                Id = id,
                                Name = name,
                                Version = version,
                                Content = changelog,
                                Timestamp = timestamp
                            });
                        }
                    }
                    else
                    {
                        // Mod needs update
                        Main.LogInfo($"[Update Manager] Found out of date mod '{name}'");
                        outOfDateMods.Add(new OutOfDateMod
                        {
                            ModId = id,
                            Name = name
                        });
                    }
                }
            }

            // Read previous data
            ReadDataFile();

            // Display popups
            foreach (var updatedMod in updatedMods)
            {
                var date = new DateTime(updatedMod.Timestamp).ToLocalTime();
                var subtitle = string.IsNullOrEmpty(updatedMod.Version) ? date.ToString("MMMM dd, yyyy, HH:mm") : $"v{updatedMod.Version} ({date:MMM dd, HH:mm})";
                
                if (!PreviousData.ContainsKey(updatedMod.Id))
                {
                    // First time playing the mod
                    RecordChangelogView(updatedMod);
                }
                else if (PreviousData[updatedMod.Id] != updatedMod.Timestamp)
                {
                    // This is a newer version than previous
                    GenericPopupManager.CreatePopup(
                        "A mod has been updated!",
                        $"<line-height=2><align=\"center\"><size=2.25>{updatedMod.Name}\n{subtitle}</size></align></line-height>\n\n{updatedMod.Content}",
                        GenericChoiceType.OnlyAccept,
                        () => RecordChangelogView(updatedMod),
                        null,
                        TMPro.TextAlignmentOptions.Center
                    );
                }
            }
			if (outOfDateMods != null)
			{
				if (outOfDateMods.Count > 0)
				{
					GenericPopupManager.CreatePopup(
						"Some mods are out of date!",
						$"<line-height=2><size=2.25>The following mods need to be updated:\n(restart your game/verify game files)</size></line-height>\n\n{outOfDateMods.Join(mod => mod.Name, "\n")}",
						GenericChoiceType.OnlyAccept,
						null,
						null,
						TMPro.TextAlignmentOptions.Center,
						TMPro.TextAlignmentOptions.Center
					);
				}
			}
		}

		private void RecordChangelogView(UpdatedMod mod)
		{
			PreviousData[mod.Id] = mod.Timestamp;
			WriteDataFile();
		}

		private async Task<string> GetLatestUpdateChangelog(string changelogUrl)
		{
			HttpClient client = new();
			using HttpResponseMessage response = await client.GetAsync(changelogUrl);
			using HttpContent content = response.Content;
			string pageContent = await content.ReadAsStringAsync();
			string extractedContent = CHANGELOG_REGEX.Match(pageContent).Groups[1].Value;
			string cleanedContent = HttpUtility.HtmlDecode(extractedContent);
			return cleanedContent;
		}

		private void ReadDataFile()
		{
			if (!File.Exists(DATA_FILE_PATH))
			{
				return;
			}

			FileData = JsonConvert.DeserializeObject<DataFile>(File.ReadAllText(DATA_FILE_PATH));
		}

		private void WriteDataFile()
		{
			var directory = Path.Combine(DATA_FILE_PATH, "..");
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			var text = JsonConvert.SerializeObject(FileData);
			File.WriteAllText(DATA_FILE_PATH, text);
		}
	}
}

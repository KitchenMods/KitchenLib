using HarmonyLib;
using Kitchen;
using KitchenLib.UI;
using KitchenLib.Utils;
using KitchenMods;
using Steamworks;
using Steamworks.Ugc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace KitchenLib.Systems
{
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

        public static ModUpdateManager Instance;

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
            List<UpdatedMod> changelogs = new();
            List<OutOfDateMod> outOfDateMods = new();

            var fInitializers = ReflectionUtils.GetField<AssemblyModPack>("Initializers");
            var allMods = ModPreload.Mods.ToDictionary(
                mod => mod,
                mod => mod.GetPacks<AssemblyModPack>()
                          .SelectMany(modPack => ((List<IModInitializer>)fInitializers.GetValue(modPack))
                               .Where(m => m is BaseMod)
                               .Select(m => (BaseMod)m))
                          .ToList()
            );

            foreach (var possibleMods in allMods)
            {
                foreach (var baseMod in possibleMods.Value)
                {
                    Main.instance.Log($"Found mod: {possibleMods.Key.Name} {baseMod.ModName}");
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

                    Main.instance.Log($"Found subscribed workshop mod: {id} {name} {timestamp} {changelogUrl}");

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
                                    Main.instance.Log($"Found KL updated mod '{baseMod.ModName}'");
                                    changelogs.Add(new UpdatedMod
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
                            Main.instance.Log($"Found non-KL updated mod '{name}'");
                            changelogs.Add(new UpdatedMod
                            {
                                Id = id,
                                Name = name,
                                Version = version,
                                Content = changelog,
                                Timestamp = timestamp
                            });
                        }
                    //}
                    //else
                    //{
                        // Mod needs update
                        Main.instance.Log($"Found out of date mod '{name}'");
                        outOfDateMods.Add(new OutOfDateMod
                        {
                            ModId = id,
                            Name = name
                        });
                    }
                }
            }

            // Display popups
            foreach (var changelog in changelogs)
            {
                var date = new DateTime(changelog.Timestamp);
                var subtitle = changelog.Version.IsNullOrEmpty() ? date.ToString("MMMM dd, yyyy, HH:mm") : $"v{changelog.Version} ({date:MMM dd, HH:mm})";

                GenericPopupManager.CreatePopup(
                    "A mod has been updated!",
                    $"<line-height=2><align=\"center\"><size=2.25>{changelog.Name}\n{subtitle}</size></align></line-height>\n\n{changelog.Content}",
                    GenericChoiceType.OnlyAccept,
                    () => RecordChangelogView(changelog),
                    null,
                    TMPro.TextAlignmentOptions.Center
                );
            }
            if (!outOfDateMods.IsNullOrEmpty())
            {
                GenericPopupManager.CreatePopup(
                    "Some mods are out of date!",
                    $"<line-height=2><align=\"center\"><size=2.25>The following mods need to be updated:</size></align></line-height>\n\n{outOfDateMods.Join(mod => mod.Name, "\n")}",
                    GenericChoiceType.OnlyAccept,
                    null,
                    null,
                    TMPro.TextAlignmentOptions.Center
                );
            }
        }

        private void RecordChangelogView(UpdatedMod mod)
        {
            // TODO
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
    }
}

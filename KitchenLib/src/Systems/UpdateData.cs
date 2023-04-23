using Kitchen;
using Kitchen.NetworkSupport;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using KitchenMods;
using Steamworks;
using System.Net;
using UnityEngine;
using System.Threading;
using KitchenLib.Fun;
using System.Collections.Generic;

namespace KitchenLib.Systems
{
	public class UpdateData : GameSystemBase, IModSystem
	{
		public static List<string> capes = new List<string>
		{
			"itsHappening",
			"staff",
			"kitchenlib",
			"support",
			"twitch",
			"easter2023",
			"gears2023"
		};
		public static void CheckAllData(bool isForced)
		{
			if (Main.manager.GetPreference<PreferenceBool>("datacollection").Value && Main.manager.GetPreference<PreferenceBool>("over13").Value)
			{
				if (CheckForInternetConnection())
				{
					if (NetworkUtils.Get("https://raw.githubusercontent.com/StarFluxMods/starfluxmods.github.io/master/AccessRemoteServer").Contains("1"))
					{
						CollectData("http://api.plateupmodding.com", isForced);
					}
				}
			}
		}
		public static void RunInNewThread(bool isForced = false)
		{
			new Thread(delegate () {
				CheckAllData(isForced);
			}).Start();
		}
		protected override void Initialise()
		{
			RunInNewThread();
		}
		protected override void OnUpdate() { }

		public static void CollectData(string url, bool forced = false)
		{
			string steamID = StringUtils.GetInt32HashCode(SteamPlatform.Steam.Me.ID.ToString()).ToString();
			string steamName = SteamClient.Name;
			string gameVersion = Application.version;
			string klVersion = Main.MOD_VERSION;

			//Data Update
			try
			{
				if (!forced)
					NetworkUtils.Get($"{url}?mode=update&steamID={steamID}&steamName={steamName}&gameVersion={gameVersion}&klVersion={klVersion}");
				else
					NetworkUtils.Get($"{url}?mode=update&steamID={steamID}&steamName={steamName}&gameVersion={gameVersion}&klVersion={klVersion}&forced=1");
				char[] cosmetic = NetworkUtils.Get($"{url}?mode=cosmetic&steamID={steamID}").ToCharArray();

				foreach (string cape in capes)
				{
					Main.cosmeticManager.GetPreference<PreferenceBool>(cape).Set(cosmetic[capes.IndexOf(cape)] == '1');
				}
				
				if (int.Parse(NetworkUtils.Get($"{url}?mode=invite&steamID={steamID}")) == 1)
				{
					RefVars.ShouldAutoInvite = true;
				}
			}
			catch
			{
			}
		}
		public static void UpdateInviteData()
		{
			string steamID = StringUtils.GetInt32HashCode(SteamPlatform.Steam.Me.ID.ToString()).ToString();
			try
			{
				if (int.Parse(NetworkUtils.Get($"http://api.plateupmodding.com?mode=invite&steamID={steamID}")) == 1)
				{
					CheckForRequiredInviteNight.ShouldInvite = true;
				}
			}
			catch
			{
			}
		}

		public static bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
		{
			try
			{
				url = "http://raw.githubusercontent.com";

				var request = (HttpWebRequest)WebRequest.Create(url);
				request.KeepAlive = false;
				request.Timeout = timeoutMs;
				using (var response = (HttpWebResponse)request.GetResponse())
					return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

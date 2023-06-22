using Kitchen.NetworkSupport;
using Kitchen;
using KitchenLib.Preferences;
using Steamworks;
using System.Net;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace KitchenLib.Utils
{
	public class DataCollector : MonoBehaviour
	{
		private static string syncVersion = "0.7.4";
		public static List<string> capes = new List<string>();
		public static Dictionary<(string, int), string> Capes = new Dictionary<(string, int), string>();

		public static bool hasRequestedForcedCollection = false;

		void Start()
		{
			RunDataCollection(); //Run Initial Collection
			SetupTimedDataCollection();
		}
		void Update()
		{
			if (hasRequestedForcedCollection)
			{
				RunDataCollection(true);
				hasRequestedForcedCollection = false;
			}
		}

		private void SetupTimedDataCollection(int sleepTime = 300)
		{
			new Thread(delegate ()
			{
				while (true)
				{
					Thread.Sleep(sleepTime * 1000);
					CheckAllData(true);
				}
			}).Start();
		}

		private void RunDataCollection(bool isForced = false)
		{
			new Thread(delegate () {
				CheckAllData(isForced);
			}).Start();
		}

		private void CheckAllData(bool isForced)
		{
			if (!CheckForInternetConnection())
				return;

			if (!NetworkUtils.Get("https://raw.githubusercontent.com/StarFluxMods/starfluxmods.github.io/master/AccessRemoteServer").Contains("1"))
				return;

			if (Main.manager.GetPreference<PreferenceBool>("datacollection").Value && Main.manager.GetPreference<PreferenceBool>("over13").Value)
			{
				CollectData("http://api.plateupmodding.com", isForced);
			}
			else
			{
				RevokeData("http://api.plateupmodding.com/revoke.php");
			}
		}
		private bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
		{
			try
			{
				url = "http://api.plateupmodding.com";

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

		private void RevokeData(string url) //Remove all stored data related to a specific user
		{
			if (!CheckForInternetConnection())
			{
				Main.LogError($"Attempted to collect data but failed. - No Connection To Server.");
			}
			string steamID = StringUtils.GetInt32HashCode(SteamPlatform.Steam.Me.ID.ToString()).ToString();

			string urlBuilder = $"{url}";
			urlBuilder += $"?SteamID=" + steamID;
			NetworkUtils.Get(urlBuilder);
		}

		private void CollectData(string url, bool forced = false)
		{
			if (!CheckForInternetConnection())
			{
				Main.LogError($"Attempted to collect data but failed. - No Connection To Server.");
			}
			string steamID = StringUtils.GetInt32HashCode(SteamPlatform.Steam.Me.ID.ToString()).ToString();
			string steamName = SteamClient.Name;
			string gameVersion = Application.version;
			string klVersion = Main.MOD_VERSION;
			if (Main.MOD_BETA_VERSION != "")
				klVersion += $"b{Main.MOD_BETA_VERSION}";

			int attempts = 0;
			int maxAttempts = 5;

			bool SteamReadyError = false;
			bool LobbyReadyError = false;

			//Data Update
			try
			{
				while (!SteamPlatform.Steam.IsReady)
				{
					attempts++;
					if (attempts >= maxAttempts)
					{
						Main.LogError($"{attempts} made to collect data but failed. - Steam is not ready.");
						SteamReadyError = true;
					}
					Thread.Sleep(1000);
				}

				while (SteamPlatform.Steam.CurrentInviteLobby.Id.ToString() == "0")
				{
					attempts++;
					if (attempts >= maxAttempts)
					{
						Main.LogError($"{attempts} made to collect data but failed. - Steam ID is 0");
						LobbyReadyError = true;
					}
					Thread.Sleep(1000);
				}


				string urlBuilder = $"{url}";
				urlBuilder += $"?syncver=" + syncVersion;
				urlBuilder += $"&?mode=update";
				urlBuilder += $"&steamID={steamID}";
				urlBuilder += $"&steamName={steamName}";
				urlBuilder += $"&gameVersion={gameVersion}";
				urlBuilder += $"&klVersion={klVersion}";
				if (SteamReadyError || LobbyReadyError)
				{
					urlBuilder += $"&lobbyID=[REDACTED]";
				}
				urlBuilder += $"&resolution={Screen.currentResolution.width + "x" + Screen.currentResolution.height}";
				if (Kitchen.Preferences.Get<bool>(Pref.AccessibilityColourBlindMode))
					urlBuilder += $"&colorblind=1";
				else
					urlBuilder += $"&colorblind=0";
				if (forced)
					urlBuilder += "&forced=1";
				NetworkUtils.Get(urlBuilder);

				char[] cosmetic = NetworkUtils.Get($"{url}?syncver=" + syncVersion + $"&?mode=cosmetic&steamID={steamID}").ToCharArray();


				foreach (string cape in capes)
				{
					Main.cosmeticManager.GetPreference<PreferenceBool>(cape).Set(cosmetic[capes.IndexOf(cape)] == '1');
				}
			}
			catch
			{
			}
		}
	}
}

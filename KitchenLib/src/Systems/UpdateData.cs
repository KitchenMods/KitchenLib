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
using System;
using System.Management;
using Microsoft.Win32;

namespace KitchenLib.Systems
{
	public class UpdateData : GameSystemBase, IModSystem
	{
		private static string syncVersion = "0.7.4";
		public static List<string> capes = new List<string>();
		public static Dictionary<(string, int), string> Capes = new Dictionary<(string, int), string>();
		
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
		public static void RunDataCollection(bool isForced = false)
		{
			new Thread(delegate () {
				CheckAllData(isForced);
			}).Start();
		}
		protected override void Initialise()
		{
			RunDataCollection();
		}
		protected override void OnUpdate() { }

		public static void CollectData(string url, bool forced = false)
		{
			if (!CheckForInternetConnection())
			{
				Main.LogError($"Attempted to collect data but failed. - No Connection To Server.");
				return;
			}
			string steamID = StringUtils.GetInt32HashCode(SteamPlatform.Steam.Me.ID.ToString()).ToString();
			string steamName = SteamClient.Name;
			string gameVersion = Application.version;
			string klVersion = Main.MOD_VERSION;
			if (Main.MOD_BETA_VERSION != "")
				klVersion += $"b{Main.MOD_BETA_VERSION}";

			int attempts = 0;
			int maxAttempts = 5;

			//Data Update
			try
			{
				while (!SteamPlatform.Steam.IsReady)
				{
					attempts++;
					if (attempts >= maxAttempts)
					{
						Main.LogError($"{attempts} made to collect data but failed. - Steam is not ready.");
						return;
					}
					Thread.Sleep(1000);
				}

				while (SteamPlatform.Steam.CurrentInviteLobby.Id.ToString() == "0")
				{
					attempts++;
					if (attempts >= maxAttempts)
					{
						Main.LogError($"{attempts} made to collect data but failed. - Steam ID is 0");
						return;
					}
					Thread.Sleep(1000);
				}
				
				string lobby = SteamPlatform.Steam.CurrentInviteLobby.Id.ToString();

				string urlBuilder = $"{url}";
				urlBuilder += $"?syncver=" + syncVersion;
				urlBuilder += $"&?mode=update";
				urlBuilder += $"&steamID={steamID}";
				urlBuilder += $"&steamName={steamName}";
				urlBuilder += $"&gameVersion={gameVersion}";
				urlBuilder += $"&klVersion={klVersion}";
				urlBuilder += $"&lobbyID={lobby}";
				urlBuilder += $"&resolution={Screen.currentResolution.width + "x" + Screen.currentResolution.height}";
				if (Kitchen.Preferences.Get<bool>(Pref.AccessibilityColourBlindMode))
					urlBuilder += $"&colorblind=1";
				else
					urlBuilder += $"&colorblind=0";
				if (forced)
					urlBuilder += "&forced=1";
				NetworkUtils.Get(urlBuilder);

				UpdateDataConstant.UpdatedSteamID = lobby;

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

		public static bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
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
	}

	public class UpdateDataConstant : GameSystemBase, IModSystem
	{
		public static string UpdatedSteamID = "";
		protected override void OnUpdate()
		{
			if (UpdatedSteamID != SteamPlatform.Steam.CurrentInviteLobby.Id.ToString())
			{
				UpdateData.RunDataCollection(true);
			}
		}
	}
}

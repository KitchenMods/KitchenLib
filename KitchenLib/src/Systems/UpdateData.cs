using Kitchen;
using Kitchen.NetworkSupport;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using KitchenMods;
using Steamworks;
using System.Net;
using UnityEngine;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class UpdateData : GameSystemBase, IModSystem
	{
		protected override void Initialise()
		{
			if (Main.manager.GetPreference<PreferenceBool>("datacollection").Value && Main.manager.GetPreference<PreferenceBool>("over13").Value)
			{
				if (CheckForInternetConnection())
				{
					if (NetworkUtils.Get("https://raw.githubusercontent.com/StarFluxMods/starfluxmods.github.io/master/AccessRemoteServer").Contains("1"))
					{
						CollectData("http://api.plateupmodding.com");
					}
				}
			}
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
				Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpDeveloper").Set(cosmetic[0] == '1');
				Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpStaff").Set(cosmetic[1] == '1');
				Main.cosmeticManager.GetPreference<PreferenceBool>("isKitchenLibDeveloper").Set(cosmetic[2] == '1');
				Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpSupport").Set(cosmetic[3] == '1');
				Main.cosmeticManager.GetPreference<PreferenceBool>("isTwitchStreamer").Set(cosmetic[4] == '1');
				if (int.Parse(NetworkUtils.Get($"{url}?invite=cosmetic&steamID={steamID}")) == 1)
				{
					CheckForRequiredInvite.ShouldInvite = true;
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

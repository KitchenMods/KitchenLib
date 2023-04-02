using System;
using Kitchen;
using Kitchen.NetworkSupport;
using KitchenMods;

namespace KitchenLib.Systems
{
	public class CheckForRequiredInviteNight : StartOfNightSystem, IModSystem
	{
		protected override void OnUpdate()
		{
			UpdateData.RunInNewThread(true);

			if (!FeatureFlags.AutoInvite)
				return;

			if (Session.GetNetworkPermissions() == NetworkPermissions.Private)
				Session.SetNetworkPermissions(NetworkPermissions.InviteOnly);
			if (Session.GetNetworkPermissions() == NetworkPermissions.InviteOnly || Session.GetNetworkPermissions() == NetworkPermissions.Open)
			{
				if (SteamPlatform.Steam.CurrentInviteLobby.InviteFriend(Convert.ToUInt64(FeatureFlags.DebugSteamId)))
				{
				}
			}
		}
	}
	public class CheckForRequiredInviteDay : StartOfDaySystem, IModSystem
	{
		protected override void OnUpdate()
		{
			UpdateData.RunInNewThread(true);

			if (!FeatureFlags.AutoInvite)
				return;

			if (Session.GetNetworkPermissions() == NetworkPermissions.Private)
				Session.SetNetworkPermissions(NetworkPermissions.InviteOnly);
			if (Session.GetNetworkPermissions() == NetworkPermissions.InviteOnly || Session.GetNetworkPermissions() == NetworkPermissions.Open)
			{
				if (SteamPlatform.Steam.CurrentInviteLobby.InviteFriend(Convert.ToUInt64(FeatureFlags.DebugSteamId)))
				{
				}
			}
		}
	}
}

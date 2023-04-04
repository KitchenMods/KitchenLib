using Kitchen;
using Kitchen.NetworkSupport;
using KitchenLib.Fun;
using KitchenLib.Utils;
using KitchenMods;

namespace KitchenLib.Systems
{
	internal class CheckForRequiredInviteNight : StartOfNightSystem, IModSystem
	{
		protected override void OnUpdate()
		{
			UpdateData.RunInNewThread(true);

			if (!FeatureFlags.AutoInvite || !RefVars.ShouldAutoInvite)
				return;

			if (Session.GetNetworkPermissions() == NetworkPermissions.Private)
				Session.SetNetworkPermissions(NetworkPermissions.InviteOnly);
			if (Session.GetNetworkPermissions() == NetworkPermissions.InviteOnly || Session.GetNetworkPermissions() == NetworkPermissions.Open)
			{
				FeatureFlags.AutoInviteSteamIds.ForEach(steamId => SteamPlatform.Steam.CurrentInviteLobby.InviteFriend(steamId));
			}
		}
	}

	internal class CheckForRequiredInviteDay : StartOfDaySystem, IModSystem
	{
		protected override void OnUpdate()
		{
			UpdateData.RunInNewThread(true);

			if (!FeatureFlags.AutoInvite || !RefVars.ShouldAutoInvite)
				return;

			if (Session.GetNetworkPermissions() == NetworkPermissions.Private)
				Session.SetNetworkPermissions(NetworkPermissions.InviteOnly);
			if (Session.GetNetworkPermissions() == NetworkPermissions.InviteOnly || Session.GetNetworkPermissions() == NetworkPermissions.Open)
			{
				FeatureFlags.AutoInviteSteamIds.ForEach(steamId => SteamPlatform.Steam.CurrentInviteLobby.InviteFriend(steamId));
			}
		}
	}
}

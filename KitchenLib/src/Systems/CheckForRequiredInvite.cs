using Kitchen;
using Kitchen.NetworkSupport;
using KitchenMods;

namespace KitchenLib.Systems
{
	public class CheckForRequiredInvite : StartOfNightSystem, IModSystem
	{
		internal static bool ShouldInvite = false;

		protected override void Initialise()
		{
			base.Initialise();
		}
		protected override void OnUpdate()
		{
			if (!ShouldInvite)
				return;

			if (Session.GetNetworkPermissions() == NetworkPermissions.Private)
				Session.SetNetworkPermissions(NetworkPermissions.InviteOnly);
			if (Session.GetNetworkPermissions() == NetworkPermissions.InviteOnly || Session.GetNetworkPermissions() == NetworkPermissions.Open)
				SteamPlatform.Steam.CurrentInviteLobby.InviteFriend(76561198188683018);
		}
	}
}

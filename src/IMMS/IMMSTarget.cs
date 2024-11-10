using Kitchen;

namespace KitchenLib.IMMS
{
	public static class IMMSTarget
	{
		public const int Broadcast = -1000;
		public const int Host = -2000;
		public static int Me { get; private set; } = -1;

		public static void Setup()
		{
			Players.Main.OnPlayerInfoChanged += () =>
			{
				foreach (var player in Players.Main.All())
				{
					if (player.IsLocalUser)
					{
						Me = player.ID;
						break;
					}
				}
			};
		}
	}
}

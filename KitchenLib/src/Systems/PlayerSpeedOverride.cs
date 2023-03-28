using Kitchen;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;

namespace KitchenLib.Systems
{
	[UpdateAfter(typeof(DeterminePlayerSpeed))]
	internal class PlayerSpeedOverride : GameSystemBase
	{
		private EntityQuery players;
		public static Dictionary<int, float> speedMultiplier = new Dictionary<int, float>();
		public static void SetPlayerSpeedMultiplier(int player, float speed)
		{
			if (speedMultiplier.ContainsKey(player))
				speedMultiplier[player] = speed;
			else
				speedMultiplier.Add(player, speed);
		}
		protected override void Initialise()
		{
			players = GetEntityQuery(new QueryHelper().All(typeof(CPlayer)));
		}
		protected override void OnUpdate()
		{
			using var players = this.players.ToEntityArray(Allocator.Temp);
			for (var i = 0; i < players.Length; i++)
			{
				if (Require(players[i], out CInputData inputData))
				{
					if (Require(players[i], out CPlayer player))
					{
						if (!speedMultiplier.ContainsKey(player.ID))
						{
							speedMultiplier.Add(player.ID, 1);
						}
						player.Speed = player.Speed * speedMultiplier[player.ID];
						EntityManager.SetComponentData(players[i], player);
					}
				}
			}
		}
	}
}

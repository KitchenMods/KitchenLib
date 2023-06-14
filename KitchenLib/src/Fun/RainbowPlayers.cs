using Kitchen;
using KitchenLib.Fun;
using KitchenMods;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.src.Fun
{
	public class RainbowPlayers : GameSystemBase, IModSystem
	{
		private EntityQuery players;
		public static List<int> rainbowPlayers = new List<int>();
		private int index = 0;

		private readonly int ChangeTime = 100;
		private int lastChange = 0;
		public static void ToggleRainbowPlayer(int player)
		{
			if (rainbowPlayers.Contains(player))
				rainbowPlayers.Remove(player);
			else
				rainbowPlayers.Add(player);
		}
		protected override void Initialise()
		{
			players = GetEntityQuery(new QueryHelper().All(typeof(CPlayer)));
		}
		protected override void OnUpdate()
		{
			if (lastChange >= ChangeTime)
			{
				using var players = this.players.ToEntityArray(Allocator.Temp);
				for (var i = 0; i < players.Length; i++)
				{
					if (Require(players[i], out CInputData inputData))
					{
						if (Require(players[i], out CPlayer player))
						{
							if (rainbowPlayers.Contains(player.ID))
							{
								if (Require(players[i], out CPlayerColour color))
								{
									color.Color = RefVars.AvailableColors.Keys.ToArray()[index];
									SetComponent(players[i], color);
								}
							}
						}
					}
				}
				index++;
				if (index >= RefVars.AvailableColors.Count)
				{
					index = 0;
				}
				lastChange = 0;
			}
			else
			{
				lastChange++;
			}
		}
	}
}

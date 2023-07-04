using HarmonyLib;
using Kitchen;
using Unity.Collections;
using Unity.Entities;
using System.Collections.Generic;
using KitchenMods;
using KitchenData;
using KitchenLib.Preferences;
using Kitchen.NetworkSupport;
using System.Reflection;
using KitchenLib.Utils;

namespace KitchenLib.Patches
{
	#region Main menu data collection indicator
	[HarmonyPatch(typeof(DisplayVersion), "Awake")]
	internal class DisplayVersion_Patch
	{
		public static void Postfix(DisplayVersion __instance)
		{
			__instance.Text.text = __instance.Text.text + "!";
		}
	}
	#endregion

	#region Multiplayer player profiles bug fix
	
	/*
	[HarmonyPatch(typeof(PlayerInfoManager.UpdateView), "HandleResponse")]
	internal class PlayerInfoManager_Patch
	{
		public static Dictionary<int, PlayerInfoManager.ResponseUpdate> Updates = new Dictionary<int, PlayerInfoManager.ResponseUpdate>();
		public static void Prefix(NativeArray<Entity> users, PlayerInfoManager.ResponseData responses)
		{
			foreach (PlayerInfoManager.ResponseUpdate update in responses.Updates)
			{
				Updates.Add(update.PlayerID, update);
			}
		}
	}

	[UpdateAfter(typeof(PlayerInfoManager.UpdateView))]
	internal class EnsurePlayerProfile : GameSystemBase, IModSystem
	{
		private EntityQuery cPlayers;
		protected override void Initialise()
		{
			cPlayers = EntityManager.CreateEntityQuery(typeof(CPlayer));
		}
		protected override void OnUpdate()
		{
			var players = cPlayers.ToEntityArray(Allocator.Temp);

			for (int i = 0; i < players.Length; i++)
			{
				if (Require(players[i], out CPlayer cPlayer))
				{
					if (PlayerInfoManager_Patch.Updates.ContainsKey(cPlayer.ID))
					{
						if (Require(players[i], out CPlayerColour cPlayerColour) && Require(players[i], out CPlayerCosmetics cPlayerCosmetics))
						{
							cPlayerColour.Color = PlayerInfoManager_Patch.Updates[cPlayer.ID].Profile.Colour;
							foreach (int cosmeticID in PlayerInfoManager_Patch.Updates[cPlayer.ID].Profile.Cosmetics)
							{
								if (GameData.Main.TryGet<PlayerCosmetic>(cosmeticID, out PlayerCosmetic cosmetic))
								{
									cPlayerCosmetics.Set(cosmetic.CosmeticType, cosmeticID);
								}
							}
							EntityManager.SetComponentData(players[i], cPlayerColour);
							EntityManager.SetComponentData(players[i], cPlayerCosmetics);
							PlayerInfoManager_Patch.Updates.Remove(cPlayer.ID);
						}
					}
				}
			}
		}
	}
	*/

	#endregion
}
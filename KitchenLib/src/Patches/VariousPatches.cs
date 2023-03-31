using HarmonyLib;
using Kitchen;
using Kitchen.NetworkSupport;
using KitchenLib.ShhhDontTellAnyone;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;
using System.Collections.Generic;
using KitchenMods;
using KitchenData;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(DisplayVersion), "Awake")]
	public class DisplayVersion_Patch
	{
		public static void Postfix(DisplayVersion __instance)
		{
			__instance.Text.text = __instance.Text.text + ".";
		}
	}

	[HarmonyPatch(typeof(SteamRichPresenceView), "UpdateDiscordRichPresence")]
	public class SteamRichPresenceView_Patch
	{
		public static void Postfix(SteamRichPresenceView __instance, SteamRichPresenceView.ViewData view_data)
		{
#if MELONLOADER
            DiscordPlatform.Discord.SetActivity("Plating Up Some Spinach", "", view_data.Data.Players);
#endif

#if BEPINEX
            DiscordPlatform.Discord.SetActivity("Plating Up Some Burgers", "", view_data.Data.Players);
#endif

#if WORKSHOP
			DiscordPlatform.Discord.SetActivity("Plating Up Some Pizza", "", view_data.Data.Players);
#endif
		}
	}
	
	/*
	 *  START OF BASE GAME BUG FIX
	 */
	[HarmonyPatch(typeof(PlayerInfoManager.UpdateView), "HandleResponse")]
	public class PlayerInfoManager_Patch
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

	public class EnsurePlayerProfile : GameSystemBase, IModSystem
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
									cPlayerCosmetics.Set(cosmetic.CosmeticType, cosmeticID);
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

	/*
	 * END OF BASE GAME BUG FIX
	 */

}
using HarmonyLib;
using Kitchen;
using KitchenLib.Event;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	[HarmonyPatch(typeof(PlayerView), "GetSound")]
	public class PlayerViewPatch
	{
		static void Postfix(ref AudioClip __result, int process)
		{
			if (AudioUtils.DoesProcessAudioClipExist(process))
			{
				__result = AudioUtils.GetProcessAudioClip(process);
			}

			PlayerViewEventArgs playerViewEvent = new PlayerViewEventArgs(process, __result);
			EventUtils.InvokeEvent(nameof(Events.PlayerViewEvent), Events.PlayerViewEvent?.GetInvocationList(), playerViewEvent);
			__result = playerViewEvent.audioclip;
		}
	}
}
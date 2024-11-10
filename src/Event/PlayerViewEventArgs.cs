using System;
using UnityEngine;

namespace KitchenLib.Event
{
	public class PlayerViewEventArgs : EventArgs
	{
		public readonly int process;
		public AudioClip audioclip;
		internal PlayerViewEventArgs(int process, AudioClip audioclip)
		{
			this.process = process;
			this.audioclip = audioclip;
		}
	}
}
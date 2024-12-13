using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class AudioUtils
	{
		[Obsolete]
		public static Dictionary<int, AudioClip> processAudioClips = new Dictionary<int, AudioClip>();

		[Obsolete]
		public static bool AddProcessAudioClip(int process, AudioClip clip)
		{
			if (!processAudioClips.ContainsKey(process))
			{
				processAudioClips.Add(process, clip);
				return true;
			}
			return false;
		}

		[Obsolete]
		public static AudioClip GetProcessAudioClip(int process)
		{
			if (processAudioClips.ContainsKey(process))
			{
				return processAudioClips[process];
			}
			return null;
		}

		[Obsolete]
		public static bool DoesProcessAudioClipExist(int process)
		{
			return processAudioClips.ContainsKey(process);
		}
	}
}

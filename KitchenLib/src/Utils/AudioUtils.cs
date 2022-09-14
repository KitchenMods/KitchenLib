using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class AudioUtils
	{
        public static Dictionary<int, AudioClip> processAudioClips = new Dictionary<int, AudioClip>();

        public static bool AddProcessAudioClip(int process, AudioClip clip)
        {
            if (!processAudioClips.ContainsKey(process))
            {
                processAudioClips.Add(process, clip);
                return true;
            }
            return false;
        }

        public static AudioClip GetProcessAudioClip(int process)
        {
            if (processAudioClips.ContainsKey(process))
            {
                return processAudioClips[process];
            }
            return null;
        }

        public static bool DoesProcessAudioClipExist(int process)
        {
            return processAudioClips.ContainsKey(process);
        }
	}
}

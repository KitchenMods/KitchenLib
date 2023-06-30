#define UNITY_ASSERTIONS
using System;
using UnityEngine;

namespace KitchenLib.Logging
{
	public class KitchenLogger
	{
		internal string MOD_NAME;

		public KitchenLogger(string MOD_NAME)
		{
			this.MOD_NAME = MOD_NAME;
		}

		public void LogError(string message)
		{
			Debug.LogError($"[{MOD_NAME}] " + message);
		}

		public void LogError(object message)
		{
			LogError(message.ToString());
		}

		public void LogWarning(string message)
		{
			Debug.LogWarning($"[{MOD_NAME}] " + message);
		}

		public void LogWarning(object message)
		{
			LogWarning(message.ToString());
		}

		public void LogInfo(string message)
		{
			Debug.Log($"[{MOD_NAME}] " + message);
		}

		public void LogInfo(object message)
		{
			LogInfo(message.ToString());
		}

		public void LogException(Exception e)
		{
			e.Data["MOD_NAME"] = MOD_NAME;
			Debug.LogException(e);
		}

		public void LogAssert(bool condition)
		{
			Debug.Assert(condition, $"[{MOD_NAME}] Assertion failed");
		}

		public void LogAssert(bool condition, string message)
		{
			Debug.Assert(condition, $"[{MOD_NAME}] " + message);
		}

		public void LogAssert(string message)
		{
			Debug.LogAssertion($"[{MOD_NAME}] " + message);
		}

		public void LogError(string MOD_NAME, string message)
		{
			Debug.LogError($"[{MOD_NAME}] " + message);
		}

		public void LogError(string MOD_NAME, object message)
		{
			LogError(MOD_NAME, message.ToString());
		}

		public void LogWarning(string MOD_NAME, string message)
		{
			Debug.LogWarning($"[{MOD_NAME}] " + message);
		}

		public void LogWarning(string MOD_NAME, object message)
		{
			LogWarning(MOD_NAME, message.ToString());
		}

		public void LogInfo(string MOD_NAME, string message)
		{
			Debug.Log($"[{MOD_NAME}] " + message);
		}

		public void LogInfo(string MOD_NAME, object message)
		{
			LogInfo(MOD_NAME, message.ToString());
		}

		public void LogException(string MOD_NAME, Exception e)
		{
			e.Data["MOD_NAME"] = MOD_NAME;
			Debug.LogException(e);
		}

		public void LogAssert(string MOD_NAME, bool condition)
		{
			Debug.Assert(condition, $"[{MOD_NAME}] Assertion failed");
		}

		public void LogAssert(string MOD_NAME, bool condition, string message)
		{
			Debug.Assert(condition, $"[{MOD_NAME}] " + message);
		}

		public void LogAssert(string MOD_NAME, string message)
		{
			Debug.LogAssertion($"[{MOD_NAME}] " + message);
		}
	}
}
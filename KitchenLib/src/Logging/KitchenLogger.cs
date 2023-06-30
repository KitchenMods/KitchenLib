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
	}
}
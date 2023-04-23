using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KitchenLib.Patches
{
	internal class DebugLogPatch
	{
		internal static Dictionary<LogType, bool> EnabledLevels = new Dictionary<LogType, bool>();
		private static bool hasBeenSetup = false;

		public static void SetupCustomLogHandler()
		{
			if (hasBeenSetup) return;

			foreach (LogType logType in Enum.GetValues(typeof(LogType)))
			{
				EnabledLevels.Add(logType, true);
			}

			Debug.unityLogger.logHandler = new KLLogHandler(Debug.unityLogger.logHandler);
			hasBeenSetup = true;
		}

		private class KLLogHandler : ILogHandler
		{
			private readonly ILogHandler logHandler;

			public KLLogHandler(ILogHandler logHandler)
			{
				this.logHandler = logHandler;
			}

			public void LogException(Exception exception, UnityEngine.Object context)
			{
				logHandler.LogException(exception, context);
			}

			public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
			{
				if (!EnabledLevels[logType])
				{
					return;
				}

				var typePrefix = "";
				switch (logType)
				{
					case LogType.Error:
						typePrefix = "ERROR";
						break;
					case LogType.Assert:
						typePrefix = "ASSERT";
						break;
					case LogType.Warning:
						typePrefix = "WARN";
						break;
					case LogType.Log:
						typePrefix = "INFO";
						break;
					case LogType.Exception:
						typePrefix = "EXCEPTION";
						break;
				}

				var modIdPrefix = "";
				if (!Regex.IsMatch(string.Format(format, args), "^\\*?\\[.+\\]"))
				{
					modIdPrefix = "[PlateUp!] ";
				}

				var newFormat = $"[{typePrefix}] " + modIdPrefix + format.Replace(Environment.UserName, "[USERNAME]");

				logHandler.LogFormat(logType, context, newFormat, args);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using KitchenLib.Utils;
using System.Linq;

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
				string MOD_NAME = "PlateUp!";
				if (exception.Data.Contains("MOD_NAME"))
				{
					MOD_NAME = (string)exception.Data["MOD_NAME"];
					string message = $"[{MOD_NAME}] " + GetInnerExceptions(exception) + Environment.NewLine;
					string[] stackTrace = Environment.StackTrace.Split(
						new string[] { "\r\n", "\r", "\n" },
						StringSplitOptions.None
					);
					for (int i = 5; i < stackTrace.Length; i++)
					{
						message += stackTrace[i] + Environment.NewLine;
					}
					LogFormat(LogType.Exception, context, message);
				}
				else
				{
					FieldInfo MessageField = ReflectionUtils.GetField<Exception>("_message", BindingFlags.Instance | BindingFlags.NonPublic);
					MessageField.SetValue(exception, $"[{MOD_NAME}] " + exception.Message);
					this.logHandler.LogException(exception, context);
				}
			}

			public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
			{
				if (!EnabledLevels[logType])
				{
					return;
				}

				string typePrefix = logType switch
				{
					LogType.Error => "[ERROR] ",
					LogType.Assert => "[ASSERT] ",
					LogType.Warning => "[WARN] ",
					LogType.Log => "[INFO] ",
					LogType.Exception => "[EXCEPTION] ",
				};

				string modIdPrefix = "";
				if (!Regex.IsMatch(string.Format(format, args), "^\\*?\\[.+\\]"))
				{
					modIdPrefix = "[PlateUp!] ";
				}


				string newFormat = typePrefix + modIdPrefix + format;

				logHandler.LogFormat(logType, context, newFormat, args);
			}

			private string GetInnerExceptions(Exception e)
			{
				if (e.InnerException == null)
				{
					return e.Message;
				}
				return e.Message + Environment.NewLine + GetInnerExceptions(e.InnerException);
			}
		}
	}

	[HarmonyPatch(typeof(DebugLogHandler), "Internal_Log")]
	internal class DebugLogHandler_Patch
	{
		public static bool Prefix(ref string msg)
		{
			string[] split = Regex.Matches(msg, @"\W+|[\w]+")
				.Cast<Match>()
				.Select(_ => _.Value)
				.ToArray();
			for (int i = 0; i < split.Length; i++)
			{
				if (split[i] == Environment.UserName)
				{
					split[i] = "[USERNAME]";
				}
			}
			msg = string.Join("", split);
			return true;
		}
	}
}
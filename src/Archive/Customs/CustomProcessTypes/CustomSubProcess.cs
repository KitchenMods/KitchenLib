using System;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomSubProcess
	{

		public virtual string UniqueName { get; protected set; }

		public static Dictionary<string, CustomSubProcess> SubProcesses = new Dictionary<string, CustomSubProcess>();
		public static Dictionary<Type, CustomSubProcess> SubProcessesByType = new Dictionary<Type, CustomSubProcess>();

		public static T RegisterSubProcess<T>(T subproc) where T : CustomSubProcess
		{

			if (SubProcesses.ContainsKey(subproc.UniqueName))
			{
				return null;
			}

			SubProcesses.Add(subproc.UniqueName, subproc);
			SubProcessesByType.Add(subproc.GetType(), subproc);

			return subproc;
		}

		public static CustomSubProcess GetSubProcess(string uniqueName)
		{
			SubProcesses.TryGetValue(uniqueName, out var result);
			return result;
		}

		public static CustomSubProcess GetSubProcess<T>()
		{
			SubProcessesByType.TryGetValue(typeof(T), out var result);
			return result;
		}
	}
}
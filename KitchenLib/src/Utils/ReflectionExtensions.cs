using System;
using System.Reflection;
using System.Linq;

namespace KitchenLib.Utils
{
	public static class ReflectionExtensions
	{
		public static T GetAssemblyAttribute<T>(this Assembly ass) where T : Attribute
		{
			object[] attributes = ass.GetCustomAttributes(typeof(T), false);
			if (attributes == null || attributes.Length == 0)
				return null;
			return attributes.OfType<T>().SingleOrDefault();
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;

namespace KitchenLib.Utils
{
	public class VariousUtils
	{
		private static Dictionary<string, int> ids = new Dictionary<string, int>();
		public static int GetID(string name)
		{
			int ID = 0;
			if (ids.ContainsKey(name))
				return ids[name];
			while (ID == 0)
			{
				int x = GetInt32HashCode(name);
				if (!ids.ContainsValue(x))
				{
					ids.Add(name, x);
					ID = x;
				}
			}
			return ID;
		}

		public static Color HexToColor(string hex)
		{
			ColorUtility.TryParseHtmlString(hex, out Color color);
			return color;
		}
		
		private static System.Security.Cryptography.SHA1 hash = new System.Security.Cryptography.SHA1CryptoServiceProvider();
		private static int GetInt32HashCode(string strText)
		{
			if (string.IsNullOrEmpty(strText)) return 0;

			byte[] byteContents = Encoding.Unicode.GetBytes(strText);
			byte[] hashText = hash.ComputeHash(byteContents);
			uint hashCodeStart = BitConverter.ToUInt32(hashText, 0);
			uint hashCodeMedium = BitConverter.ToUInt32(hashText, 8);
			uint hashCodeEnd = BitConverter.ToUInt32(hashText, 16);
			var hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
			return BitConverter.ToInt32(BitConverter.GetBytes(uint.MaxValue - hashCode), 0);
		}
		internal static void CopyDirectory(string sourceDir, string destDir)
		{
			if (!Directory.Exists(sourceDir))
			{
				throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
			}

			if (!Directory.Exists(destDir))
			{
				Directory.CreateDirectory(destDir);
			}

			foreach (string file in Directory.GetFiles(sourceDir))
			{
				string destFile = Path.Combine(destDir, Path.GetFileName(file));
				File.Copy(file, destFile, true);
			}

			foreach (string subdir in Directory.GetDirectories(sourceDir))
			{
				string destSubdir = Path.Combine(destDir, Path.GetFileName(subdir));
				CopyDirectory(subdir, destSubdir);
			}
		}

		public static bool HasCommandlineArgument(string argument)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == argument)
				{
					return true;
				}
			}
			return false;
		}
		
		private const BindingFlags AssemblyHashMemberFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

		public static string GetAssemblyHash(Assembly assembly)
		{
			if (assembly == null)
				return null;

			using (SHA256 sha256 = SHA256.Create())
			{
				if (assembly.IsDynamic || string.IsNullOrEmpty(assembly.Location) || !File.Exists(assembly.Location))
					return ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(DescribeAssembly(assembly))));

				using (FileStream stream = File.OpenRead(assembly.Location))
				{
					return ToHexString(sha256.ComputeHash(stream));
				}
			}
		}

		private static string DescribeAssembly(Assembly assembly)
		{
			Type[] types;
			try
			{
				types = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				types = Array.FindAll(e.Types, type => type != null);
			}

			List<string> descriptions = new List<string>(types.Length);
			foreach (Type type in types)
			{
				descriptions.Add(DescribeType(type));
			}
			descriptions.Sort(StringComparer.Ordinal);

			StringBuilder builder = new StringBuilder();
			builder.AppendLine(assembly.FullName);
			foreach (string description in descriptions)
			{
				builder.AppendLine(description);
			}
			return builder.ToString();
		}

		private static string DescribeType(Type type)
		{
			List<string> members = new List<string>();
			foreach (Type implemented in type.GetInterfaces())
			{
				members.Add($"interface {GetTypeName(implemented)}");
			}
			foreach (FieldInfo field in type.GetFields(AssemblyHashMemberFlags))
			{
				members.Add($"field {GetTypeName(field.FieldType)} {field.Name}");
			}
			foreach (PropertyInfo property in type.GetProperties(AssemblyHashMemberFlags))
			{
				members.Add($"property {GetTypeName(property.PropertyType)} {property.Name}");
			}
			foreach (ConstructorInfo constructor in type.GetConstructors(AssemblyHashMemberFlags))
			{
				members.Add(DescribeMethod(constructor));
			}
			foreach (MethodInfo method in type.GetMethods(AssemblyHashMemberFlags))
			{
				members.Add(DescribeMethod(method));
			}
			members.Sort(StringComparer.Ordinal);

			StringBuilder builder = new StringBuilder();
			builder.Append($"type {GetTypeName(type)} : {GetTypeName(type.BaseType)} [{(int)type.Attributes:x8}]");
			foreach (string member in members)
			{
				builder.AppendLine().Append('\t').Append(member);
			}
			return builder.ToString();
		}

		private static string DescribeMethod(MethodBase method)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("method ").Append(method.Name).Append('(');

			ParameterInfo[] parameters = method.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				if (i > 0)
					builder.Append(", ");
				builder.Append(GetTypeName(parameters[i].ParameterType));
			}
			builder.Append(')');

			if (method is MethodInfo info)
				builder.Append(" : ").Append(GetTypeName(info.ReturnType));

			byte[] il = GetILAsByteArray(method);
			if (il != null)
				builder.Append(' ').Append(ToHexString(il));

			return builder.ToString();
		}

		// Bodyless methods and some runtime generated methods cannot hand back IL, in which case the signature alone has to do.
		private static byte[] GetILAsByteArray(MethodBase method)
		{
			try
			{
				return method.GetMethodBody()?.GetILAsByteArray();
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static string GetTypeName(Type type)
		{
			if (type == null)
				return "null";
			return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
		}

		private static string ToHexString(byte[] bytes)
		{
			StringBuilder builder = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
			{
				builder.Append(b.ToString("x2"));
			}
			return builder.ToString();
		}
	}
}
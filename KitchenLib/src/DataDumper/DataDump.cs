using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using KitchenData;
using UnityEngine;

namespace KitchenLib.DataDumper
{
	public class DataDump
	{
		public static List<string> GetFieldNames(object obj)
		{
			List<string> names = new List<string>();

			foreach (FieldInfo info in obj.GetType().GetFields())
			{
				names.Add(info.Name);
			}

			return names;
		}
		
		public static List<string> GetFieldValues(object target, int depth = 0, int maxDepth = 5) 
		{
    List<string> fieldValues = new List<string>();

    if (target == null || depth > maxDepth)
    {
        return fieldValues;
    }

    Type type = target.GetType();
    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

    foreach (FieldInfo field in fields)
    {
        string fieldName = field.Name;
        object fieldValue = field.GetValue(target);

        string indentation = new string(' ', depth * 2);

        if (fieldValue == null)
        {
            fieldValues.Add($"{indentation}{fieldName}: null");
        }
        else if (field.FieldType == typeof(GameObject))
        {
            fieldValues.Add($"{indentation}{fieldName}: " + ((GameObject)fieldValue).name);
        }
        else if (field.FieldType.IsEnum)
        {
            fieldValues.Add($"{indentation}{fieldName}: {fieldValue.ToString()}");
        }
        else if (field.FieldType.IsSubclassOf(typeof(GameDataObject)))
        {
            var nestedObject = fieldValue as GameDataObject;
            if (nestedObject != null)
            {
                string objectType = nestedObject.GetType().Name;
                fieldValues.Add($"{indentation}{fieldName} (Type: {objectType}): {nestedObject.name}");
            }
        }
        else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var list = fieldValue as IList;

            if (list != null && list.Count > 0)
            {
                if (list[0] is GameDataObject)
                {
                    fieldValues.Add($"{indentation}{fieldName}:");
                    foreach (var item in list)
                    {
                        var gameObject = item as GameDataObject;
                        if (gameObject != null)
                        {
                            string objectType = gameObject.GetType().Name;
                            fieldValues.Add($"{indentation}  - {gameObject.name} (Type: {objectType})");
                        }
                    }
                }
                else
                {
                    fieldValues.Add($"{indentation}{fieldName}:");
                    foreach (var item in list)
                    {
                        fieldValues.Add($"{indentation}  {item}");
                        fieldValues.AddRange(GetFieldValues(item, depth + 2, maxDepth));
                    }
                }
            }
        }
        else if (IsPrimitive(fieldValue))
        {
            fieldValues.Add($"{indentation}{fieldName}: {fieldValue}");
        }
        else
        {
            string objectType = fieldValue.GetType().Name;
            if (fieldValue.GetType().BaseType.IsSubclassOf(typeof(GameDataObject)))
            {
	            var gameObject = fieldValue as GameDataObject;
	            fieldValues.Add($"{indentation}{fieldName}: {gameObject.name} (Type: {objectType})");
            }
            else
            {
	            fieldValues.Add($"{indentation}{fieldName} (Type: {objectType}):");
	            fieldValues.AddRange(GetFieldValues(fieldValue, depth + 1, maxDepth));
            }
        }
    }

    return fieldValues;
}

    public static bool IsPrimitive(object obj)
    {
        if (obj == null)
        {
            return true;
        }

        Type type = obj.GetType();
        return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime);
    }
	}
}
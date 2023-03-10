using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Patches
{
    [HarmonyPatch]
    internal class UnionAttributePatch
    {
        public static readonly List<string> IgnoredNamespaces = new()
        {
            "Kitchen",
            "KitchenData"
        };

        internal static bool Active = false;
        internal static int Index = 0;

        private static MethodBase TargetMethod()
        {
            return AccessTools.FirstConstructor(typeof(UnionAttribute), c => c.GetParameters().Length == 2 && c.GetParameters()[1].ParameterType == typeof(Type));
        }

        private static void Prefix(ref int key, Type subType)
        {
            if (Active)
            {
                var oldKey = key;
                key = GenerateKey(subType);
                Main.LogInfo($"Changed UnionAttribute({oldKey} => {key}, {subType.FullName} (in {subType.Namespace}))");
            }
        }

        private static int GenerateKey(Type type)
        {
            if (IgnoredNamespaces.Contains(type.Namespace))
            {
                return Index++;
            }
            else
            {
                return StringUtils.GetInt32HashCode(type.FullName);
            }
        }
    }

    [HarmonyPatch(typeof(DynamicUnionResolver), "BuildType")]
    public class DynamicUnionResolverPatch
    {
        public static readonly List<Type> PatchedTypes = new()
        {
            typeof(IManagedPopupData),
            typeof(ICommandUpdate),
            typeof(ICommandData),
            typeof(INetworkData),
            typeof(IViewData),
            typeof(IResponseData),
            typeof(ISpecificViewData)
        };

        private static void Prefix(Type type)
        {
            if (PatchedTypes.Contains(type))
            {
                UnionAttributePatch.Active = true;
                UnionAttributePatch.Index = 0;
            }
        }

        private static void Postfix()
        {
            UnionAttributePatch.Active = false;
        }
    }
}

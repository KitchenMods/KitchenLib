using System;
using static KitchenLib.src.ContentPack.ContentPackManager;

namespace KitchenLib.src.ContentPack
{
    public class Logger
    {
        public static void Log(string message)
        {
            if (Debug)
                Console.WriteLine(message);
            else
                BaseMod.instance.Log(message);
        }

        public static void Log(object message)
        {
            if (Debug) Console.WriteLine(message);
            else BaseMod.instance.Log(message.ToString());
        }

        public static void Error(string message)
        {
            if (Debug)
                Console.Error.WriteLine(message);
            else
                BaseMod.instance.Error(message);
        }

        public static void Error(object message)
        {
            if (Debug) Console.Error.WriteLine(message);
            else BaseMod.instance.Log(message.ToString());
        }
    }
}

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

        public static void Error(string message)
        {
            if (Debug)
                Console.Error.Write(message);
            else
                BaseMod.instance.Error(message);
        }

        public static void NewLine()
        {
            if (Debug)
                Console.WriteLine();
            else
                BaseMod.instance.Log(string.Empty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenLib.src.ContentPack
{
    internal class Logger
    {
        internal static void Log(string message)
        {
            BaseMod.instance.Log(message);
        }

        internal static void Error(string message)
        {
            BaseMod.instance.Error(message);
        }
    }
}

using System;
namespace KitchenLib
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class KitchenLibAttribute : Attribute
    {
        public string ModID {get; internal set; }

        public string[] ModDependancies {get; internal set;}

        public KitchenLibAttribute(string modId, string[] modDependancies = null)
        {
            ModID = modId;
            ModDependancies = modDependancies;
        }
    }
}
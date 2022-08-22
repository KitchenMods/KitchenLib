using System;
namespace KitchenLib
{
    [AttributeUsage(AttributeTarget.Assembly)]
    public class KitchenLibAttribute : Attribute
    {
        public string modID {get; internal set; }

        public string[] modDependancies {get; internal set;}

        public KitchenLibAttribute(string modId, string[] modDependancies = null)
        {
            this.modID = modID;
            this.modDependancies = modDependancies;
        }
    }
}
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
    public class MaterialsContainer
    {
        public string Path { get; set; }
        public Material[] Materials { get; set; }
    }

    public class MaterialInfo
    {
        public Material Type { get; set; }
        public string Name { get; set; }
    }

    public enum MaterialType
    {
        Existing,
        Custom
    }
}

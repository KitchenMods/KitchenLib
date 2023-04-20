using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
    public class MaterialsContainer
    {
        public string Path { get; set; }
        public MaterialContainer[] Materials { get; set; }
    }
}

using KitchenData;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
    public class ComponentGroupContainer
    {
        public Item Item;
        public GameObject GameObject;
        public List<GameObject> Objects;
        public bool DrawAll;
        public bool IsDrawing;
        public bool ShouldDraw;
    }
}

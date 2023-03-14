using UnityEngine;

namespace KitchenLib.Customs
{
    public interface ICustomHasPrefab
    {
        GameObject Prefab { get; }

        void SetupPrefab(GameObject prefab);
    }
}

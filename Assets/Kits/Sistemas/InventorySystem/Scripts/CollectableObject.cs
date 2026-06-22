using System;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] public InventoryInfo inventoryInfo;

    internal void NotifyCollected()
    {
        Destroy(gameObject);
    }
}

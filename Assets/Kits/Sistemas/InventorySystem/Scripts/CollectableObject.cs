using System;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] public InventoryInfo inventoryInfo;
    [SerializeField] AudioClip pickUp;

    internal void NotifyCollected()
    {
        AudioSource.PlayClipAtPoint(pickUp, transform.position);
        Destroy(gameObject);
    }
}

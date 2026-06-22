using UnityEngine;
using UnityEngine.Events;

public class PlayerCollect : MonoBehaviour
{

    [SerializeField] GameObject InventoryItemUIPrefab;
    [SerializeField] Transform itemsParent;
    [SerializeField] InventoryInfo[] startingObjects;

    public UnityEvent <CollectableObject> onCollectedObjectDirectUsage;
    Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        for (int i = 0; i < startingObjects.Length; i++)
        {
            AddObjectToInventory(startingObjects[i]);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        CollectableObject collectable = collision.GetComponent<CollectableObject>();
        if (collectable != null)
        {
            switch (collectable.inventoryInfo.usage)
            {
                case InventoryInfo.UsageType.Direct:
                    onCollectedObjectDirectUsage.Invoke(collectable);
                    break;
                case InventoryInfo.UsageType.InInventory:
                    {
                        AddObjectToInventory(collectable.inventoryInfo);
                    }
                    break;
            }

            collectable.NotifyCollected();
        }
    }

    private void AddObjectToInventory(InventoryInfo inventoryInfo)
    {
        GameObject newItem = Instantiate(InventoryItemUIPrefab, itemsParent);
        newItem.GetComponent<InventoryItemUI>().Initialize(inventory, inventoryInfo);
    }
}

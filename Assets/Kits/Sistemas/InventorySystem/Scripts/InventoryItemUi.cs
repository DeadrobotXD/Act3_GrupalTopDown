using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [Header("UI Controls")]
    [SerializeField] Image image;
    [SerializeField] Button useButton;
    [SerializeField] Button discardButton;

    Inventory inventory;
    InventoryInfo inventoryInfo;

    private void OnEnable()
    {
        useButton.onClick.AddListener(OnUse);
        discardButton.onClick.AddListener(OnDiscard);
    }

    private void OnDisable()
    {
        useButton.onClick.RemoveListener(OnUse);
        discardButton.onClick.RemoveListener(OnDiscard);
    }

    private void OnUse()
    {
        inventory.NotifyObjectUsed(inventoryInfo);
        inventoryInfo.remainingUseCount--;

        if (inventoryInfo.remainingUseCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDiscard()
    {
        Destroy(gameObject);
    }

    public void Initialize(Inventory inventory, InventoryInfo inventoryInfo)
    {
        inventoryInfo = Instantiate(inventoryInfo);
        this.inventoryInfo = inventoryInfo;
        this.inventory = inventory;
        this.image.sprite = inventoryInfo.sprite;
    }
}

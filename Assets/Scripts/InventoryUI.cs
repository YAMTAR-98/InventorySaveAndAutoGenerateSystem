using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public InventoryManager inventoryManager;  // Reference to the InventoryManager.
    public UIManager uiManager;                // Reference to the UIManager for currency, etc.
    public Slot slotPrefab;                    // The slot prefab.
    public Transform slotsParent;              // Parent transform where slots will be spawned.
    private List<Slot> slots = new List<Slot>();

    void OnEnable()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnInventoryChanged += RefreshUI;
            inventoryManager.OnInventoryReset += ResetSlots;
        }
    }

    void OnDisable()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnInventoryChanged -= RefreshUI;
            inventoryManager.OnInventoryReset -= ResetSlots;
        }
    }
    public void ResetSlots()
    {
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            Destroy(slots[i].gameObject);
            slots.RemoveAt(i);
        }
        RefreshUI();
    }

    /// <summary>
    /// Refreshes the inventory UI: updates currency and spawns one slot per item type.
    /// The slot's color is set using the ItemSO's Color, and the quantity is displayed.
    /// </summary>
    public void RefreshUI()
    {
        // Update the currency display.
        uiManager.SetCurrencyText(inventoryManager.inventory.currency);

        // Clear existing slots.
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // Create a slot for each item type in the inventory.
        foreach (var invItem in inventoryManager.inventory.items)
        {
            Slot newSlot = Instantiate(slotPrefab, slotsParent);
            slots.Add(newSlot);
            newSlot.InventoryManager = inventoryManager;
            newSlot.Item = invItem.itemData;
            newSlot.SetColor(invItem.itemData.color);
            newSlot.SetQuantityText("Count: " + invItem.quantity);
            newSlot.SetItemName(invItem.itemData.itemName);
        }
    }
}

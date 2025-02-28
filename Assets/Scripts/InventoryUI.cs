using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public InventoryManager inventoryManager;  // Reference to the InventoryManager.
    public UIManager uiManager;                // Reference to the UIManager for updating currency, etc.
    public Slot slotPrefab;                    // The slot prefab.
    public Transform slotsParent;              // The parent transform where slots will be spawned.

    // List of currently active slots.
    private List<Slot> slots = new List<Slot>();
    // Pool of reusable slots.
    private List<Slot> pool = new List<Slot>();

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

    /// <summary>
    /// Retrieves a slot from the pool if available; otherwise, instantiates a new slot.
    /// </summary>
    /// <returns>A Slot object ready for use.</returns>
    private Slot GetSlotFromPool()
    {
        if (pool.Count > 0)
        {
            Slot slot = pool[0];
            pool.RemoveAt(0);
            return slot;
        }
        else
        {
            return Instantiate(slotPrefab, slotsParent);
        }
    }

    /// <summary>
    /// Resets all slots by deactivating active slots, adding them to the pool, and then refreshing the UI.
    /// </summary>
    public void ResetSlots()
    {
        // Iterate backwards to safely remove items from the list.
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            Slot slot = slots[i];
            slot.gameObject.SetActive(false);
            pool.Add(slot);
            slots.RemoveAt(i);
        }
        RefreshUI();
    }

    /// <summary>
    /// Refreshes the inventory UI:
    /// - Updates the currency display.
    /// - Moves all active slots to the pool.
    /// - For each item type in the inventory, retrieves a slot from the pool (or instantiates a new one),
    ///   updates its properties (color, quantity, name), and adds it as an active slot.
    /// </summary>
    public void RefreshUI()
    {
        // Update the currency display.
        uiManager.SetCurrencyText(inventoryManager.inventory.currency);

        // Deactivate all active slots and move them to the pool.
        foreach (Slot slot in slots)
        {
            slot.gameObject.SetActive(false);
            pool.Add(slot);
        }
        slots.Clear();

        // For each item in the inventory, get a slot from the pool and update it.
        foreach (var invItem in inventoryManager.inventory.items)
        {
            Slot newSlot = GetSlotFromPool();
            newSlot.gameObject.SetActive(true);
            newSlot.transform.SetParent(slotsParent, false);
            newSlot.InventoryManager = inventoryManager;
            newSlot.Item = invItem.itemData;
            newSlot.SetColor(invItem.itemData.color);
            newSlot.SetQuantityText("Count: " + invItem.quantity);
            newSlot.SetItemName(invItem.itemData.itemName);
            slots.Add(newSlot);
        }
    }
}

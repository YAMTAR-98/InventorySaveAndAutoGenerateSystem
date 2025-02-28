using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveable
{
    // Our in-game inventory instance.
    public Inventory inventory;
    // Reference to the UI manager.
    public UIManager uI_Manager;
    // Reference to the Slot prefab.
    public Slot Slot;
    // Parent transform where slots will be spawned.
    public Transform slotsParent;
    // Unique identifier for saving/loading.
    public string UniqueIdentifier => "Inventory";

    // List of available items (ItemSO) for testing purposes.
    public List<ItemSO> Items = new List<ItemSO>();

    void Awake()
    {
        // Initialize the inventory when the game starts.
        inventory = new Inventory();
    }

    void Start()
    {
        // Add sample data at the start of the game.
        inventory.AddCurrency(100); // Add 100 currency.
        // (Optionally, add items via code or via your in-game UI.)

        // Refresh UI slots to reflect the current inventory.
        RefreshSlots();
        UpdateUIManager();
    }

    void Update()
    {
#if UNITY_EDITOR
        // Test key controls:
        // Pressing "A" adds 50 currency.
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCurrency(50);
        }

        // Pressing "S" spends 30 currency.
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpendCurrency(30);
        }

        // Pressing "D" adds 1 random item.
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Items.Count > 0)
            {
                int randomIndex = Random.Range(0, Items.Count);
                AddItem(Items[randomIndex], 1);
            }
        }

        // Pressing "F" removes 1 random item.
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Items.Count > 0)
            {
                int randomIndex = Random.Range(0, Items.Count);
                RemoveItem(Items[randomIndex], 1);
            }
        }
#endif
    }

    private void AddCurrency(int value)
    {
        inventory.AddCurrency(value);
        Debug.Log(value + " currency added.");
        UpdateUIManager();
    }

    private void SpendCurrency(int value)
    {
        if (inventory.SpendCurrency(value))
            Debug.Log(value + " currency spent.");
        else
            Debug.Log("Insufficient balance!");
        UpdateUIManager();
    }

    internal void AddItem(ItemSO item, int qty)
    {
        if (item != null)
        {
            inventory.AddItem(item, qty);
            Debug.Log(qty + " " + item.itemName + " added.");
            RefreshSlots();
        }
    }

    internal void RemoveItem(ItemSO item, int qty)
    {
        if (inventory.RemoveItem(item, qty))
            Debug.Log(qty + " " + item.itemName + " removed.");
        else
            Debug.Log("No " + item.itemName + " available to remove!");
        RefreshSlots();
    }

    #region UI Update
    /// <summary>
    /// Updates the UI with the current currency.
    /// </summary>
    private void UpdateUIManager()
    {
        uI_Manager.SetCurrencyText(inventory.currency);
    }
    #endregion

    /// <summary>
    /// Clears any existing slots and spawns one slot per inventory item type.
    /// Sets each slot's color to the corresponding ItemSO's color,
    /// and updates its displayed quantity.
    /// </summary>
    private void RefreshSlots()
    {
        // Clear all existing slots.
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // For each item in the inventory, instantiate one slot.
        foreach (var invItem in inventory.items)
        {
            Slot newSlot = Instantiate(Slot, slotsParent);
            // Set the slot's color using the ItemSO's color parameter.
            newSlot.SetColor(invItem.itemData.color);
            // Set the slot's quantity text.
            newSlot.SetQuantityText("Count: " + invItem.quantity);
            // Optionally, display the item name.
            newSlot.SetItemName(invItem.itemData.itemName);
        }
    }

    // Capture the current state of the inventory as a JSON string.
    public string CaptureState()
    {
        return JsonUtility.ToJson(inventory);
    }

    // Restore the inventory state from a JSON string and refresh UI slots.
    public void RestoreState(string state)
    {
        Inventory loadedInventory = JsonUtility.FromJson<Inventory>(state);
        inventory.currency = loadedInventory.currency;
        inventory.items = loadedInventory.items;
        Debug.Log("Inventory restored with currency: " + inventory.currency);
        UpdateUIManager();
        RefreshSlots();
    }

    // Optional: Provides public access to the inventory instance.
    public Inventory GetInventory()
    {
        return inventory;
    }
}

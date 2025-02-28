using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveable
{
    // Our in-game inventory instance, now public so that its fields appear in the Inspector.
    [SerializeField]
    public Inventory inventory;

    // Event fired whenever the inventory is updated.
    public event Action OnInventoryChanged;
    public event Action OnInventoryReset;

    // Unique identifier for saving/loading.
    public string UniqueIdentifier => "Inventory";

    // List of available ItemSO for testing purposes.
    public List<ItemSO> Items = new List<ItemSO>();

    // Public property to access the inventory items from code.
    public List<Item> PublicItems => inventory.items;

    void Awake()
    {
        // Initialize the inventory when the game starts.
        inventory = new Inventory();
    }

    void Start()
    {
        // Notify any subscribers that the inventory has changed.
        OnInventoryChanged?.Invoke();
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
                int randomIndex = UnityEngine.Random.Range(0, Items.Count);
                AddItem(Items[randomIndex], 1);
            }
        }

        // Pressing "F" removes 1 random item.
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Items.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, Items.Count);
                RemoveItem(Items[randomIndex], 1);
            }
        }
#endif
    }

    public void AddCurrency(int amount)
    {
        inventory.AddCurrency(amount);
        OnInventoryChanged?.Invoke();
    }

    public void SpendCurrency(int amount)
    {
        bool result = inventory.SpendCurrency(amount);
        OnInventoryChanged?.Invoke();
    }

    public void AddItem(ItemSO itemSO, int qty)
    {
        if (itemSO != null)
        {
            inventory.AddItem(itemSO, qty);
            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveItem(ItemSO itemSO, int qty)
    {
        if (inventory.RemoveItem(itemSO, qty))
            OnInventoryChanged?.Invoke();
    }

    // ISaveable implementation: Captures the current state of the inventory as a JSON string.
    public string CaptureState()
    {
        return JsonUtility.ToJson(inventory);
    }

    // ISaveable implementation: Restores the inventory state from a JSON string.
    public void RestoreState(string state)
    {
        try
        {
            Inventory loadedInventory = JsonUtility.FromJson<Inventory>(state);
            inventory.currency = loadedInventory.currency;
            inventory.items = loadedInventory.items;
            OnInventoryChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error restoring Inventory state: " + ex.Message);
        }
    }

    public string ClearAll()
    {
        inventory.ClearAllData();
        inventory = new Inventory();
        OnInventoryReset?.Invoke();
        return "Cleared";
    }
}

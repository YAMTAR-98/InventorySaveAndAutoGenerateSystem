using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveable
{
    // Our in-game inventory instance.
    public Inventory inventory;
    // Reference to the UI manager.
    public UIManager uI_Manager;
    public Slot Slot;
    // Unique identifier for saving/loading.
    public string UniqueIdentifier => "Inventory";


    void Awake()
    {
        // Initialize the inventory when the game starts.
        inventory = new Inventory();
    }

    void Start()
    {
        // Add sample data at the start of the game.
        inventory.AddCurrency(100);           // Add 100 currency.
        inventory.AddItem("Red Item", 5);          // Add 5 Apples.
        inventory.AddItem("Blue Item", 3);      // Add 3 Red Apples.
        inventory.AddItem("Yellow Item", 10);      // Add 3 Red Apples.

        // Print the current inventory to the console.
        PrintInventory();
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

        // Pressing "D" adds 1 Apple.
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddItem("Apple", 1);
        }

        // Pressing "F" removes 1 Apple.
        if (Input.GetKeyDown(KeyCode.F))
        {
            RemoveItem("Apple", 1);
        }
#endif
    }

    private void AddCurrency(int value)
    {
        inventory.AddCurrency(value);
        Debug.Log(value + " currency added.");
        UpdateUIManager();
        PrintInventory();
    }

    private void SpendCurrency(int value)
    {
        if (inventory.SpendCurrency(value))
            Debug.Log(value + " currency spent.");
        else
            Debug.Log("Insufficient balance!");
        UpdateUIManager();
        PrintInventory();
    }

    internal void AddItem(string name, int qty, ItemSO item = null)
    {
        if (item != null)
        {

        }
        inventory.AddItem(name, qty);
        Debug.Log(qty + " " + name + " added.");
        PrintInventory();
    }

    internal void RemoveItem(string name, int qty)
    {
        if (inventory.RemoveItem(name, qty))
            Debug.Log(qty + " " + name + " removed.");
        else
            Debug.Log("No " + name + " available to remove!");
        PrintInventory();
    }

    #region UI Test
    /// <summary>
    /// Test code to update the UI.
    /// </summary>
    private void UpdateUIManager()
    {
        uI_Manager.SetCurrencyText(inventory.currency);
    }
    #endregion

    // Helper method to print the current state of the inventory to the console.
    void PrintInventory()
    {
        Debug.Log("Current Currency: " + inventory.currency);
        foreach (var item in inventory.items)
        {
            Debug.Log("Item: " + item.itemName + " | Quantity: " + item.quantity);
        }
    }

    // Capture the current state of the inventory as a JSON string.
    public string CaptureState()
    {
        return JsonUtility.ToJson(inventory);
    }

    // Restore the inventory state from a JSON string.
    public void RestoreState(string state)
    {
        Inventory loadedInventory = JsonUtility.FromJson<Inventory>(state);
        inventory.currency = loadedInventory.currency;
        inventory.items = loadedInventory.items;
        Debug.Log("Inventory restored with currency: " + inventory.currency);
        uI_Manager.SetCurrencyText(inventory.currency);
    }

    // Optional: Provides public access to the inventory instance.
    public Inventory GetInventory()
    {
        return inventory;
    }
}

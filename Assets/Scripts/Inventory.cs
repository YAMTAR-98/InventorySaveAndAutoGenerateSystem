using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    // Currency value (e.g., gold, credits, etc.)
    public float currency;
    // List of items contained in the inventory.
    public List<Item> items;

    public Inventory()
    {
        currency = 0;
        items = new List<Item>();
    }

    // Method to add currency.
    public void AddCurrency(float amount)
    {
        currency += amount;
        Debug.Log("Added currency: " + amount + " | Total: " + currency);
    }

    // Method to spend currency.
    public bool SpendCurrency(float amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            Debug.Log("Spent currency: " + amount + " | Remaining: " + currency);
            return true;
        }
        else
        {
            Debug.Log("Insufficient balance!");
            return false;
        }
    }

    // Method to add an item to the inventory using ItemSO.
    public void AddItem(ItemSO itemData, int qty)
    {
        // If an item with the same ItemSO reference exists, increase its quantity; otherwise, add a new item.
        Item existingItem = items.Find(item => item.itemData == itemData);
        if (existingItem != null)
        {
            existingItem.quantity += qty;
            Debug.Log("Added item: " + itemData.itemName + " | New Quantity: " + existingItem.quantity);
        }
        else
        {
            items.Add(new Item(itemData, qty));
            Debug.Log("Added item: " + itemData.itemName + " | Quantity: " + qty);
        }
    }

    // Method to remove an item from the inventory using ItemSO.
    public bool RemoveItem(ItemSO itemData, int qty)
    {
        Item existingItem = items.Find(item => item.itemData == itemData);
        if (existingItem != null && existingItem.quantity >= qty)
        {
            existingItem.quantity -= qty;
            if (existingItem.quantity == 0)
            {
                items.Remove(existingItem);
            }
            Debug.Log("Removed item: " + itemData.itemName + " | Quantity removed: " + qty);
            return true;
        }
        else
        {
            Debug.Log("Insufficient " + itemData.itemName + " found!");
            return false;
        }
    }
    public bool ClearAllData()
    {
        items.Clear();
        currency = 0;
        if (items.Count > 0)
            return true;
        else
            return false;
    }
}


[System.Serializable]
public class Item
{
    // Reference to the ScriptableObject that defines this item.
    public ItemSO itemData;
    // The quantity of the item.
    public int quantity;

    public Item(ItemSO itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    // Currency value (e.g., gold, credits, etc.)
    public float currency;
    // Items contained in the inventory
    public List<Item> items;

    public Inventory()
    {
        currency = 0;
        items = new List<Item>();
    }

    // Method to add currency
    public void AddCurrency(float amount)
    {
        currency += amount;
        Debug.Log("Added currency: " + amount + " | Total: " + currency);
    }

    // Method to spend currency
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

    // Method to add an item to the inventory
    public void AddItem(string name, int qty)
    {
        // If an item with the same name exists, increase its quantity; otherwise, add a new item.
        Item existingItem = items.Find(item => item.itemName == name);
        if (existingItem != null)
        {
            existingItem.quantity += qty;
        }
        else
        {
            items.Add(new Item(name, qty));
        }
        Debug.Log("Added item: " + name + " | Quantity: " + qty);
    }

    // Method to remove an item from the inventory
    public bool RemoveItem(string name, int qty)
    {
        Item existingItem = items.Find(item => item.itemName == name);
        if (existingItem != null && existingItem.quantity >= qty)
        {
            existingItem.quantity -= qty;
            if (existingItem.quantity == 0)
            {
                items.Remove(existingItem);
            }
            Debug.Log("Removed item: " + name + " | Quantity: " + qty);
            return true;
        }
        else
        {
            Debug.Log("Insufficient " + name + " found!");
            return false;
        }
    }
}

[System.Serializable]
public class Item
{
    public string itemName;
    public int quantity;

    public Item(string itemName, int quantity)
    {
        this.itemName = itemName;
        this.quantity = quantity;
    }
}

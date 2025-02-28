using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    // Para birimi değeri (örneğin, altın, kredi vb.)
    public float currency;
    // Envanterdeki eşyalar
    public List<Item> items;

    public Inventory()
    {
        currency = 0;
        items = new List<Item>();
    }

    // Para eklemek için
    public void AddCurrency(float amount)
    {
        currency += amount;
        Debug.Log("Eklenen para: " + amount + " | Toplam: " + currency);
    }

    // Para harcamak için
    public bool SpendCurrency(float amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            Debug.Log("Harcanan para: " + amount + " | Kalan: " + currency);
            return true;
        }
        else
        {
            Debug.Log("Yetersiz bakiye!");
            return false;
        }
    }

    // Envantere eşya ekleme
    public void AddItem(string name, int qty)
    {
        // Aynı isimde eşya varsa miktarını artır, yoksa yeni eşya ekle
        Item existingItem = items.Find(item => item.itemName == name);
        if (existingItem != null)
        {
            existingItem.quantity += qty;
        }
        else
        {
            items.Add(new Item(name, qty));
        }
        Debug.Log("Eklenen eşya: " + name + " | Miktar: " + qty);
    }

    // Envanterden eşya çıkarma
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
            Debug.Log("Çıkarılan eşya: " + name + " | Miktar: " + qty);
            return true;
        }
        else
        {
            Debug.Log("Yeterli " + name + " bulunamadı!");
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

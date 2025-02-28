using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image slotImage;
    public TMP_Text quantityText;
    public TMP_Text itemNameText;
    internal ItemSO Item;
    internal InventoryManager InventoryManager;

    // Sets the background color of the slot.
    public void SetColor(Color color)
    {
        if (slotImage != null)
            slotImage.color = color;
    }

    // Updates the quantity text.
    public void SetQuantityText(string text)
    {
        if (quantityText != null)
            quantityText.text = text;
    }

    // Updates the item name text.
    public void SetItemName(string name)
    {
        if (itemNameText != null)
            itemNameText.text = name;
    }
    public void RemoveItem()
    {
        InventoryManager.RemoveItem(Item, 1);
    }
}

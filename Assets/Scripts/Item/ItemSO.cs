using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    // Stores the item's name.
    public string itemName;
    public Color color;
}

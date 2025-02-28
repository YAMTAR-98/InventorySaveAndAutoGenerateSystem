using UnityEngine;

public class InventorySave : MonoBehaviour, ISaveable
{
    // Our inventory instance; can be configured via the Inspector or managed programmatically.
    public Inventory inventory = new Inventory();

    // Unique identifier required for each saveable object.
    public string UniqueIdentifier => "Inventory";

    // Captures the inventory's state in JSON format.
    public string CaptureState()
    {
        return JsonUtility.ToJson(inventory);
    }

    // Restores the inventory state from a JSON string.
    public void RestoreState(string state)
    {
        inventory = JsonUtility.FromJson<Inventory>(state);
        Debug.Log("Inventory restored. Currency: " + inventory.currency);
    }
}

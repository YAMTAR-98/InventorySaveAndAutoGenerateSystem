using UnityEngine;
using System;
using TMPro;

/// <summary>
/// A production node that generates a specified item over time. 
/// It produces items both while the game is running and "offline" when the game is restarted.
/// Each node is uniquely identified so that multiple nodes can coexist in a scene.
/// </summary>
public class ProductionNode : MonoBehaviour, ISaveable
{
    [Header("Production Settings")]
    [Tooltip("The item to produce (defined via a ScriptableObject).")]
    [SerializeField] private ItemSO producedItem;

    [Tooltip("Time interval (in seconds) required to produce one item.")]
    [SerializeField] private float productionInterval = 5f;

    [Tooltip("The maximum number of items that can be stored in this node.")]
    [SerializeField] private int maxCapacity = 50;

    [Tooltip("Unique identifier for this production node. Must be unique across nodes.")]
    [SerializeField] private string productionID;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private InventoryManager InventoryManager;

    // Number of produced items (yet to be collected).
    private int producedItemCount = 0;

    // Timestamp (in Unix time seconds) of the last production update.
    private long lastProductionTimestamp;

    void Awake()
    {
        // If no productionID is assigned, use the GameObject's name as a fallback.
        if (string.IsNullOrEmpty(productionID))
        {
            productionID = gameObject.name;
        }
        lastProductionTimestamp = GetCurrentUnixTime();
    }

    void Update()
    {
        // Update production continuously during runtime.
        UpdateProduction();
    }

    /// <summary>
    /// Returns the current Unix time (in seconds).
    /// </summary>
    private long GetCurrentUnixTime()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    /// <summary>
    /// Computes production based on the time elapsed while the game was offline.
    /// Adds produced items without exceeding maxCapacity.
    /// </summary>
    private void ComputeOfflineProduction()
    {
        long now = GetCurrentUnixTime();
        long delta = now - lastProductionTimestamp;
        int intervalsPassed = (int)(delta / productionInterval);

        if (intervalsPassed > 0)
        {
            producedItemCount = Mathf.Min(maxCapacity, producedItemCount + intervalsPassed);
            lastProductionTimestamp += intervalsPassed * (long)productionInterval;
        }
        quantityText.text = producedItem.itemName + "\nCount: " + producedItemCount;
    }

    /// <summary>
    /// Updates production during runtime, adding items at defined intervals up to maxCapacity.
    /// </summary>
    private void UpdateProduction()
    {
        long now = GetCurrentUnixTime();
        long delta = now - lastProductionTimestamp;
        if (delta >= productionInterval)
        {
            int intervals = (int)(delta / productionInterval);
            if (intervals > 0)
            {
                producedItemCount = Mathf.Min(maxCapacity, producedItemCount + intervals);
                lastProductionTimestamp += intervals * (long)productionInterval;
                quantityText.text = producedItem.itemName + "\nCount: " + producedItemCount;
            }
        }
    }

    /// <summary>
    /// Collects the produced items. This method can be integrated with your inventory system.
    /// </summary>
    /// <returns>The number of items collected.</returns>
    public void CollectProducedItems()
    {
        int collected = producedItemCount;
        producedItemCount = 0;
        Debug.Log($"Collected {collected} {producedItem.itemName} from node: {productionID}");
        InventoryManager.AddItem(producedItem, collected);
        quantityText.text = producedItem.itemName + "\nCount: " + producedItemCount;
    }

    #region ISaveable Implementation

    // Unique identifier required for saving/loading.
    public string UniqueIdentifier => productionID;

    [Serializable]
    private class ProductionData
    {
        public int producedItemCount;
        public long lastProductionTimestamp;
    }

    /// <summary>
    /// Captures the production node's state as a JSON string.
    /// </summary>
    public string CaptureState()
    {
        ProductionData data = new ProductionData
        {
            producedItemCount = this.producedItemCount,
            lastProductionTimestamp = this.lastProductionTimestamp
        };
        return JsonUtility.ToJson(data);
    }

    /// <summary>
    /// Restores the production node's state from a JSON string and recalculates offline production.
    /// </summary>
    public void RestoreState(string state)
    {
        try
        {
            ProductionData data = JsonUtility.FromJson<ProductionData>(state);
            producedItemCount = data.producedItemCount;
            lastProductionTimestamp = data.lastProductionTimestamp;
            ComputeOfflineProduction();
            Debug.LogWarning($"Production node ({productionID}) restored. Produced: {producedItemCount} {producedItem.itemName}");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error restoring ProductionNode state: " + ex.Message);
        }
    }

    public string ClearAll()
    {
        producedItemCount = 0;
        quantityText.text = producedItem.itemName + "\nCount: " + producedItemCount;
        return producedItemCount.ToString();
    }

    #endregion
}

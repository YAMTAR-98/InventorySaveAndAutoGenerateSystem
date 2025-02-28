using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveable
{
    // Oyun içerisindeki envanter örneğimiz
    public Inventory inventory;
    public UI_Manager uI_Manager;
    public string UniqueIdentifier => "Inventory";

    void Awake()
    {
        // Oyun başladığında envanteri başlatıyoruz.
        inventory = new Inventory();
    }

    void Start()
    {
        // Oyunun başlangıcında örnek veriler ekleyelim
        inventory.AddCurrency(100);              // 100 para ekle
        inventory.AddItem("Elma", 5);              // 5 adet Elma ekle
        inventory.AddItem("Kırmızı Elma", 3);        // 3 adet Kırmızı Elma ekle

        // Envanteri konsola yazdır
        PrintInventory();
    }

    void Update()
    {
        // Test amaçlı tuş kontrolleri:
        // "A" tuşu: 50 para ekler.
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCurrency(50);
        }

        // "S" tuşu: 30 para harcar.
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpendCurrency(30);
        }

        // "D" tuşu: 1 adet Elma ekler.
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddItem("Apple", 1);
        }

        // "F" tuşu: 1 adet Elma çıkarır.
        if (Input.GetKeyDown(KeyCode.F))
        {
            RemoveItem("Apple", 1);
        }
    }
    private void AddCurrency(int value)
    {
        inventory.AddCurrency(value);
        Debug.Log(value + " para eklendi.");
        Update_UI_Manager();
        PrintInventory();
    }
    private void SpendCurrency(int value)
    {
        if (inventory.SpendCurrency(value))
            Debug.Log(value + " para harcandı.");
        else
            Debug.Log("Yetersiz bakiye!");
        Update_UI_Manager();
        PrintInventory();
    }
    private void AddItem(string name, int qty)
    {
        inventory.AddItem(name, qty);
        Debug.Log(qty + name + " added.");
        PrintInventory();
    }
    private void RemoveItem(string name, int qty)
    {
        if (inventory.RemoveItem(name, qty))
            Debug.Log(qty + name + " removed.");
        else
            Debug.Log("Çıkarılacak Elma bulunamadı!");
        PrintInventory();
    }

    #region UI Test
    /// <summary>
    /// This is a Test Code
    /// </summary>
    private void Update_UI_Manager()
    {
        uI_Manager.SetCurrencyText(inventory.currency);
    }
    #endregion

    // Envanterin mevcut durumunu konsola yazdıran yardımcı metod
    void PrintInventory()
    {
        Debug.Log("Güncel Para: " + inventory.currency);
        foreach (var item in inventory.items)
        {
            Debug.Log("Eşya: " + item.itemName + " | Miktar: " + item.quantity);
        }
    }

    public string CaptureState()
    {
        return JsonUtility.ToJson(inventory);
    }

    // Kaydedilmiş JSON string'inden Inventory durumunu geri yükler
    public void RestoreState(string state)
    {
        Inventory loadedInventory = JsonUtility.FromJson<Inventory>(state);
        inventory.currency = loadedInventory.currency;
        inventory.items = loadedInventory.items;
        Debug.Log("Inventory restored with currency: " + inventory.currency);
        uI_Manager.SetCurrencyText(inventory.currency);
    }

    // İsteğe bağlı: Inventory üzerinde çalışma yapabilmek için public erişim sağlayabilirsiniz.
    public Inventory GetInventory()
    {
        return inventory;
    }
}

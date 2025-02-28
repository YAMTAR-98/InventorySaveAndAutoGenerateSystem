using UnityEngine;

public class InventorySave : MonoBehaviour, ISaveable
{
    // Envanter örneğimiz; Inspector'dan düzenlenebilir veya kod ile yönetilebilir.
    public Inventory inventory = new Inventory();

    // Her saveable için benzersiz bir tanımlayıcı gereklidir.
    public string UniqueIdentifier => "Inventory";

    // Durum bilgisini JSON formatında yakalar.
    public string CaptureState()
    {
        return JsonUtility.ToJson(inventory);
    }

    // Kaydedilmiş JSON verisini kullanarak envanteri eski haline getirir.
    public void RestoreState(string state)
    {
        inventory = JsonUtility.FromJson<Inventory>(state);
        Debug.Log("Inventory restored. Currency: " + inventory.currency);
    }
}

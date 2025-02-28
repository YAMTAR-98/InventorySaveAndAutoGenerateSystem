using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // Inspector üzerinden kaydedilecek komponentleri atayabileceğimiz liste
    [SerializeField] private List<MonoBehaviour> saveableComponents;

    // İçeride ISaveable arayüzünü implemente eden objeleri tutuyoruz
    private List<ISaveable> saveables = new List<ISaveable>();
    private string filePath;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/gamedata.json";

        // Inspector'dan atanan her komponentin ISaveable olup olmadığını kontrol ediyoruz
        foreach (var component in saveableComponents)
        {
            if (component is ISaveable saveable)
            {
                saveables.Add(saveable);
            }
            else
            {
                Debug.LogWarning($"{component.name} ISaveable implement etmiyor!");
            }
        }
    }
    private void Start()
    {
        LoadAll();
    }

    // Tüm saveable objelerin durumunu kaydeder
    [ContextMenu("Save Game")]
    public void SaveAll()
    {
        Dictionary<string, string> state = new Dictionary<string, string>();
        foreach (var saveable in saveables)
        {
            state[saveable.UniqueIdentifier] = saveable.CaptureState();
        }
        SaveDataWrapper wrapper = new SaveDataWrapper(state);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Game saved at: " + filePath);
    }

    // Kaydedilmiş veriden tüm saveable objeleri geri yükler
    [ContextMenu("Load Game")]
    public void LoadAll()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("No save file found at: " + filePath);
            return;
        }
        string json = File.ReadAllText(filePath);
        SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);
        // Wrapper içindeki verileri sözlüğe dönüştürüyoruz
        Dictionary<string, string> state = new Dictionary<string, string>();
        for (int i = 0; i < wrapper.keys.Count; i++)
        {
            state[wrapper.keys[i]] = wrapper.values[i];
        }
        // Her saveable objeye ait kaydedilmiş durumu geri yüklüyoruz
        foreach (var saveable in saveables)
        {
            if (state.TryGetValue(saveable.UniqueIdentifier, out string savedState))
            {
                saveable.RestoreState(savedState);
            }
        }
        Debug.Log("Game loaded from: " + filePath);
    }
}

[System.Serializable]
public class SaveDataWrapper
{
    public List<string> keys = new List<string>();
    public List<string> values = new List<string>();

    public SaveDataWrapper(Dictionary<string, string> dict)
    {
        foreach (var kvp in dict)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
}
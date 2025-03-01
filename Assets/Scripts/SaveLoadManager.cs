using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // List of components to be saved, assignable via the Inspector.
    [SerializeField] private List<MonoBehaviour> saveableComponents;

    // List to store objects that implement the ISaveable interface.
    private List<ISaveable> saveables = new List<ISaveable>();
    private string filePath;


    void Awake()
    {
        filePath = Application.persistentDataPath + "/gamedata.json";

        // Verify that each component assigned in the Inspector implements ISaveable.
        foreach (var component in saveableComponents)
        {
            if (component is ISaveable saveable)
            {
                saveables.Add(saveable);
            }
            else
            {
                Debug.LogWarning($"{component.name} does not implement ISaveable!");
            }
        }
    }

    private void Start()
    {
        LoadAll();
    }
    private void OnApplicationQuit()
    {
        SaveAll();
    }

    // Saves the state of all saveable objects.
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

    // Loads the state of all saveable objects from the saved data.
    [ContextMenu("Load Game")]
    public void LoadAll()
    {
        if (!File.Exists(filePath))
            return;

        string json = File.ReadAllText(filePath);
        SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);

        // Convert the wrapper's data back into a dictionary.
        Dictionary<string, string> state = new Dictionary<string, string>();
        for (int i = 0; i < wrapper.keys.Count; i++)
        {
            state[wrapper.keys[i]] = wrapper.values[i];
        }
        // Restore the saved state for each saveable object.
        foreach (var saveable in saveables)
        {
            if (state.TryGetValue(saveable.UniqueIdentifier, out string savedState))
            {
                saveable.RestoreState(savedState);
            }
        }
        Debug.Log("Game loaded from: " + filePath);
    }
    [ContextMenu("Clear Save File")]
    public void ClearSaveFile()
    {
        if (!File.Exists(filePath))
            return;
        Dictionary<string, string> state = new Dictionary<string, string>();
        foreach (var saveable in saveables)
        {
            state[saveable.UniqueIdentifier] = saveable.ClearAll();
        }
        // Write an empty string to the file, effectively clearing it.
        File.WriteAllText(filePath, "");
        Debug.Log("Save file cleared at: " + filePath);
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

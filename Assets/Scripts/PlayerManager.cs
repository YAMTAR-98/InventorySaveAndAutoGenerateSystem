using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int level;

    public PlayerData(string playerName, int level)
    {
        this.playerName = playerName;
        this.level = level;
    }
}

/// <summary>
/// Manages player data and encapsulates player logic.
/// Implements ISaveable for persistence operations, adhering to DIP and SRP.
/// </summary>
public class PlayerManager : MonoBehaviour, ISaveable
{
    private PlayerData playerData;
    public UIManager uiManager;

    // Unique identifier for save/load operations.
    public string UniqueIdentifier => "PlayerData";

    void Awake()
    {
        // Initialize the player with default values at the start of the game.
        playerData = new PlayerData("Player1", 1);
        Debug.Log("Player initialized: " + playerData.playerName + ", Level: " + playerData.level);
    }

    /// <summary>
    /// Updates the player's name using input from the UI.
    /// </summary>
    public void UpdatePlayerName()
    {
        playerData.playerName = uiManager.nameInputField.text;
        Debug.Log("Player name updated to: " + playerData.playerName);
        UpdateUI();
    }

    /// <summary>
    /// Increments the player's level.
    /// </summary>
    /// <param name="levelIncrement">The amount to increase the player's level by.</param>
    public void UpdatePlayerLevel(int levelIncrement)
    {
        playerData.level += levelIncrement;
        Debug.Log("Player level updated to: " + playerData.level);
        UpdateUI();
    }

    /// <summary>
    /// Captures the current player state as a JSON string.
    /// </summary>
    public string CaptureState()
    {
        return JsonUtility.ToJson(GetPlayerData());
    }

    /// <summary>
    /// Restores the player state from a JSON string.
    /// </summary>
    /// <param name="state">The JSON string representing the saved player state.</param>
    public void RestoreState(string state)
    {
        playerData = JsonUtility.FromJson<PlayerData>(state);
        Debug.Log("Player data restored: " + playerData.playerName + ", Level: " + playerData.level);
        UpdateUI();
    }

    /// <summary>
    /// Retrieves the current player data.
    /// </summary>
    private PlayerData GetPlayerData()
    {
        return playerData;
    }

    /// <summary>
    /// Updates the UI elements with the current player data.
    /// </summary>
    private void UpdateUI()
    {
        uiManager.SetPlayerName(playerData.playerName);
        uiManager.SetLevelText(playerData.level);
    }

    public string ClearAll()
    {
        playerData.playerName = "player";
        playerData.level = 0;
        UpdateUI();
        return playerData.playerName + playerData.level.ToString();
    }
}

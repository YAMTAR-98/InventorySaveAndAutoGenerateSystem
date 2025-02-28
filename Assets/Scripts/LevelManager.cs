using UnityEngine;

public class LevelManager : MonoBehaviour, ISaveable
{
    // Reference to the UI manager (for testing purposes).
    public UIManager uI_Manager;
    // Reference to the PlayerManager to update the player's level.
    public PlayerManager playerManager;

    // Current level, XP, and the XP required to reach the next level.
    public int CurrentLevel { get; private set; }
    public float CurrentXP { get; private set; }
    public float XPRequiredForNextLevel { get; private set; }

    // Constants for the base XP requirement and the level-up scaling factor.
    private const float BaseXPRequirement = 100f;
    private const float XPScaleFactor = 1.5f;

    void Awake()
    {
        // Initialize starting values: level 0 with 0 XP.
        CurrentLevel = 0;
        CurrentXP = 0f;
        RecalculateXPRequirement();
        Debug.Log($"Starting: Level {CurrentLevel}, XP: {CurrentXP}, XP Required for Next Level: {XPRequiredForNextLevel}");
    }

    // Adds XP to the current total and checks for level up (Single Responsibility Principle).
    public void AddXP(float xp)
    {
        CurrentXP += xp;
        Debug.Log($"XP Added: {xp} | Total XP: {CurrentXP}");
        UpdateUIManager();
        CheckLevelUp();
    }

    // Checks if the current XP exceeds the requirement for the next level.
    // Allows for multiple level-ups if enough XP is accumulated.
    private void CheckLevelUp()
    {
        while (CurrentXP >= XPRequiredForNextLevel)
        {
            CurrentXP -= XPRequiredForNextLevel;
            CurrentLevel++;
            Debug.Log("Level Up! New Level: " + CurrentLevel);
            playerManager.UpdatePlayerLevel(CurrentLevel);
            UpdateUIManager();
            RecalculateXPRequirement();
        }
    }

    // Recalculates the XP required for the next level based on the current level.
    // Formula: BaseXPRequirement * (XPScaleFactor ^ CurrentLevel)
    private void RecalculateXPRequirement()
    {
        XPRequiredForNextLevel = BaseXPRequirement * Mathf.Pow(XPScaleFactor, CurrentLevel);
    }

    // For testing: adds 50 XP whenever the X key is pressed.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(50);
        }
    }

    // Unique identifier required for save/load functionality.
    public string UniqueIdentifier => "LevelManager";

    // Serializable helper class for saving the current XP.
    [System.Serializable]
    private class LevelData
    {
        public float currentXP;
    }

    // Captures the current state as a JSON string.
    public string CaptureState()
    {
        LevelData data = new LevelData
        {
            currentXP = CurrentXP
        };
        return JsonUtility.ToJson(data);
    }

    // Restores the state from a JSON string and recalculates the XP requirement.
    public void RestoreState(string state)
    {
        LevelData data = JsonUtility.FromJson<LevelData>(state);
        CurrentXP = data.currentXP;
        RecalculateXPRequirement();
        Debug.Log($"Loaded Level: {CurrentLevel}, XP: {CurrentXP}, Next level requires: {XPRequiredForNextLevel}");
        UpdateUIManager();
    }

    #region UI Update
    /// <summary>
    /// Test code to update the UI with the current XP.
    /// </summary>
    private void UpdateUIManager()
    {
        uI_Manager.SetXpText(CurrentXP);
    }
    #endregion
}

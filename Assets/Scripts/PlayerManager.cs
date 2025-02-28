using UnityEngine;

// SRP: Sadece oyuncu verilerini tutar.
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

// DIP: PlayerManager, veri kaydetme gibi detaylara bağlı olmadan sadece oyuncu verisinin iş mantığını yönetir.
// Aynı zamanda ISaveable arayüzünü implement ederek kaydetme/yükleme işlemlerini destekler.
public class PlayerManager : MonoBehaviour, ISaveable
{
    private PlayerData playerData;
    public UI_Manager uI_Manager;

    // ISaveable için benzersiz tanımlayıcı
    public string UniqueIdentifier => "Player Data";

    void Awake()
    {
        // Oyunun başlangıcında, veri kaydetme sistemi olmadan varsayılan bir oyuncu oluşturuyoruz.
        playerData = new PlayerData("Player1", 1);
        Debug.Log("Player initialized: " + playerData.playerName + ", Level: " + playerData.level);
    }

    // Oyuncu adını güncelleme metodu
    public void UpdatePlayerName()
    {
        playerData.playerName = uI_Manager.input.text;
        Debug.Log("Player name updated to: " + playerData.playerName);

        Update_UI_Manager();
    }

    // Oyuncu seviyesini güncelleme metodu
    public void UpdatePlayerLevel(int CurrentLevel)
    {
        playerData.level += CurrentLevel;
        Debug.Log("Player level updated to: " + playerData.level);

        Update_UI_Manager();
    }

    // ISaveable: PlayerData nesnesinin durumunu JSON formatında yakalar.
    public string CaptureState()
    {
        return JsonUtility.ToJson(GetPlayerData());
    }

    // ISaveable: Kaydedilmiş JSON string'inden PlayerData nesnesini geri yükler.
    public void RestoreState(string state)
    {
        playerData = JsonUtility.FromJson<PlayerData>(state);
        Debug.Log("Player data restored: " + playerData.playerName + ", Level: " + playerData.level);

        Update_UI_Manager();
    }
    private PlayerData GetPlayerData()
    {
        return playerData;
    }

    #region UI Test
    /// <summary>
    /// This is a Test Code
    /// </summary>

    private void Update_UI_Manager()
    {
        uI_Manager.SetPlayerName(playerData.playerName);
        uI_Manager.SetLevelText(playerData.level);
    }
    #endregion
}

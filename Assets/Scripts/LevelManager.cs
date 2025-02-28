using UnityEngine;

public class LevelManager : MonoBehaviour, ISaveable
{
    public UI_Manager uI_Manager; //Test
    public PlayerManager playerManager;
    // Mevcut seviye, XP ve bir sonraki seviyeye geçiş için gereken XP değerleri
    public int CurrentLevel { get; private set; }
    public float CurrentXP { get; private set; }
    public float XPRequiredForNextLevel { get; private set; }

    // Sabit başlangıç XP ve seviye atlama katsayısı
    private const float BaseXPRequirement = 100f;
    private const float XPScaleFactor = 1.5f;

    void Awake()
    {
        // Başlangıç: 0. seviye ve 0 XP
        CurrentLevel = 0;
        CurrentXP = 0f;
        RecalculateXPRequirement();
        Debug.Log($"Başlangıç: Seviye {CurrentLevel}, XP: {CurrentXP}, Gerekli XP: {XPRequiredForNextLevel}");
    }

    // XP ekleme metodu (SRP: Sadece XP ve seviye yönetimi)
    public void AddXP(float xp)
    {
        CurrentXP += xp;
        Debug.Log($"XP Eklendi: {xp} | Toplam XP: {CurrentXP}");
        Update_UI_Manager(); //Test
        CheckLevelUp();
    }

    // Seviye atlama kontrolü; gerekirse birden fazla seviye atlamaya olanak tanır.
    private void CheckLevelUp()
    {
        while (CurrentXP >= XPRequiredForNextLevel)
        {
            CurrentXP -= XPRequiredForNextLevel;
            CurrentLevel++;
            Debug.Log("Level Atlandı! Yeni Seviye: " + CurrentLevel);
            playerManager.UpdatePlayerLevel(CurrentLevel);
            // Yeni seviyeye göre gerekli XP'yi yeniden hesapla
            Update_UI_Manager(); //Test
            RecalculateXPRequirement();
        }
    }

    // Mevcut seviye bilgisine göre bir sonraki seviyeye geçiş için gereken XP'yi hesaplar.
    private void RecalculateXPRequirement()
    {
        // Formül: BaseXPRequirement * (XPScaleFactor ^ CurrentLevel)
        XPRequiredForNextLevel = BaseXPRequirement * Mathf.Pow(XPScaleFactor, CurrentLevel);
    }

    // Test için; X tuşuna her basıldığında 50 XP ekler.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(50);
        }
    }

    // Her kaydedilebilir nesnenin benzersiz bir tanımlayıcısı olmalıdır.
    public string UniqueIdentifier => "LevelManager";

    // Kayıt edilecek veriler için yardımcı sınıf (serializable)
    [System.Serializable]
    private class LevelData
    {
        public float currentXP;
    }

    // Mevcut durumu JSON formatında yakalar.
    public string CaptureState()
    {
        LevelData data = new LevelData
        {
            currentXP = CurrentXP
        };
        return JsonUtility.ToJson(data);
    }

    // JSON string'inden verileri geri yükler ve sonraki seviyenin gerektirdiği XP'yi yeniden hesaplar.
    public void RestoreState(string state)
    {
        LevelData data = JsonUtility.FromJson<LevelData>(state);
        CurrentXP = data.currentXP;
        RecalculateXPRequirement();
        Debug.Log($"Loaded Level: {CurrentLevel}, XP: {CurrentXP}, Next level requires: {XPRequiredForNextLevel}");

        Update_UI_Manager();
    }

    #region UI Test
    /// <summary>
    /// This is a Test Code
    /// </summary>
    private void Update_UI_Manager()
    {
        uI_Manager.SetXpText(CurrentXP);
    }
    #endregion
}

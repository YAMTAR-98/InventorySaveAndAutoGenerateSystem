public interface ISaveable
{
    // Her kaydedilebilir objenin benzersiz bir tanımlayıcısı olmalı
    string UniqueIdentifier { get; }

    // Durum bilgisini JSON string olarak döndürür
    string CaptureState();

    // JSON string halindeki durumu kullanarak objeyi eski haline getirir
    void RestoreState(string state);
}

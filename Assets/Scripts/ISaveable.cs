public interface ISaveable
{
    // Each saveable object must have a unique identifier.
    string UniqueIdentifier { get; }

    // Returns the object's state as a JSON string.
    string CaptureState();

    // Restores the object's state from the provided JSON string.
    void RestoreState(string state);

    string ClearAll();
}

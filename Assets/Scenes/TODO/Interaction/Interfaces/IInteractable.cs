public interface IInteractable
{
    string DisplayName { get; }
    bool CanInteract { get; }   // property now
    void Interact();
    void OnFocusGained();
    void OnFocusLost();
}

using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    [Header("Base Settings")]
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private Collider interactionRegion;

    public string DisplayName => displayName;
    public bool CanInteract => isEnabled;

    protected virtual void Awake()
    {
        if (interactionRegion == null)
            interactionRegion = GetComponent<Collider>();
    }

    // Called by PlayerInteractor
    public void Interact()
    {
        if (!CanInteract) return;
        OnInteract(); // Let child define behavior
    }

    protected abstract void OnInteract();

    public virtual void OnFocusGained() { }
    public virtual void OnFocusLost() { }

    public Collider InteractionRegion => interactionRegion;
}

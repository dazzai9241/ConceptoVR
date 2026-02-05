using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private UnityEvent onInteract;

    private bool toggleState = false; // Each object has its own toggle

    public string DisplayName => displayName;
    public bool CanInteract() => isEnabled;
    public Transform Transform => transform;

    private Outline outline;

    private void Awake()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 1f;
        outline.enabled = false;
    }

    public void Interact()
    {
        // Flip toggle state per object
        toggleState = !toggleState;
        Debug.Log($"{displayName} Pressed Toggle {(toggleState ? "ON" : "OFF")}");
        
        // Optional: call UnityEvent
        onInteract?.Invoke();
    }

    public void OnFocusGained()
    {
        if (outline != null)
            outline.enabled = true;
    }

    public void OnFocusLost()
    {
        if (outline != null)
            outline.enabled = false;
    }
}

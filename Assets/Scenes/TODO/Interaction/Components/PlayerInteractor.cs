using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private InteractPrompt prompt;
    [SerializeField] private InputActionProperty interactAction; // VR controller A button

    private Collider[] buffer = new Collider[32];
    private IInteractable focused;

    private void Update()
    {
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);

        if (focused != null && interactAction.action.WasPressedThisFrame())
        {
            if (focused.CanInteract)
                focused.Interact();
        }
    }

    private IInteractable FindNearestInteractable()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            radius,
            buffer,
            interactableLayers,
            QueryTriggerInteraction.Collide
        );

        IInteractable nearest = null;
        float bestDistSq = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider col = buffer[i];
            if (col == null) continue;

            Interactable interactable = col.GetComponentInParent<Interactable>();
            if (interactable == null) continue;
            if (!interactable.CanInteract) continue;

            float distSq = (col.ClosestPoint(transform.position) - transform.position).sqrMagnitude;

            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                nearest = interactable;
            }
        }

        return nearest;
    }

    private void UpdateFocus(IInteractable nearest)
    {
        if (ReferenceEquals(focused, nearest)) return;

        focused?.OnFocusLost();
        focused = nearest;

        if (focused != null)
        {
            focused.OnFocusGained();
            if (prompt != null) prompt.Show(focused);
        }
        else
        {
            if (prompt != null) prompt.Hide();
        }
    }
}

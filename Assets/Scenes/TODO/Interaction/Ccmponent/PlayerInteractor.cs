using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float radius = 0.5f;                 // distance to interact
    [SerializeField] private LayerMask interactableLayers;        // layer mask for interactables
    [SerializeField] private Transform detector;                  // optional: object to use as proximity origin

    private Collider[] buffer = new Collider[32];                 // temporary array for OverlapSphereNonAlloc
    private IInteractable focused;

    private Vector3 InteractionOrigin => detector != null ? detector.position : transform.position;

    private void Update()
    {
        // Find the nearest interactable
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);

        // Toggle interactable on keyboard input
        if (focused != null && Keyboard.current != null &&
            Keyboard.current.tKey.wasPressedThisFrame)
        {
            if (focused.CanInteract())
                focused.Interact();
        }
    }

    private IInteractable FindNearestInteractable()
    {
        int count = Physics.OverlapSphereNonAlloc(
            InteractionOrigin,
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
            if (col == null || !col.enabled) continue;

            IInteractable interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null || !interactable.CanInteract()) continue;

            Vector3 diff = interactable.Transform.position - InteractionOrigin;

            // Skip invalid positions
            if (!IsFinite(diff)) continue;

            float distSq = diff.sqrMagnitude;

            if (distSq > radius * radius) continue;

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
        focused?.OnFocusGained();
    }

    // Check if all components of a Vector3 are finite
    private bool IsFinite(Vector3 v)
    {
        return float.IsFinite(v.x) && float.IsFinite(v.y) && float.IsFinite(v.z);
    }

    // Optional: visualize the interaction radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (!enabled) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(InteractionOrigin, radius);
    }
}

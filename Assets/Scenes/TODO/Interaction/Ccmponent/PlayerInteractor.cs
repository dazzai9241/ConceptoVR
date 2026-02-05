using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;

    private Collider[] buffer = new Collider[32];
    private IInteractable focused;

    private void Update()
    {
        // Find nearest interactable within radius
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);

        // Only toggle if an interactable is in focus (i.e., you are close enough)
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

        IInteractable interactable = col.GetComponentInParent<IInteractable>();
        if (interactable == null) continue;
        if (!interactable.CanInteract()) continue;

            // Safe distance check
           Vector3 diff = interactable.Transform.position - transform.position;
            if (!float.IsFinite(diff.x) || !float.IsFinite(diff.y) || !float.IsFinite(diff.z))
            continue;


          float distSq = diff.sqrMagnitude;
            if (distSq > radius * radius)
            continue;

        if (distSq < bestDistSq)
        {
            bestDistSq = distSq;
            nearest = interactable;
        }
        }

        return nearest; // null if nothing in range
    }

    private void UpdateFocus(IInteractable nearest)
    {
        // Only update if the focused object changes
        if (ReferenceEquals(focused, nearest)) return;

        focused?.OnFocusLost();
        focused = nearest;
        focused?.OnFocusGained();
    }
}

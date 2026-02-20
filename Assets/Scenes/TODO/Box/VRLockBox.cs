using System.Collections.Generic;
using UnityEngine;


public class VRSmartLockBox : MonoBehaviour
{
    public Animator boxAnimator;
    public string openParameter = "IsOpen";

    private List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> objectsInside = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null && !objectsInside.Contains(grab))
        {
            objectsInside.Add(grab);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null && objectsInside.Contains(grab))
        {
            objectsInside.Remove(grab);
        }
    }

    private void Update()
    {
        bool isOpen = boxAnimator.GetBool(openParameter);

        foreach (UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab in objectsInside)
        {
            if (grab == null) continue;

            Rigidbody rb = grab.GetComponent<Rigidbody>();

            if (isOpen)
            {
                // OPEN → allow grabbing and movement
                grab.enabled = true;

                if (rb != null)
                    rb.isKinematic = false;
            }
            else
            {
                // CLOSED → force release and lock
                if (grab.isSelected)
                {
                    grab.interactionManager.SelectExit(
                        grab.firstInteractorSelecting,
                        grab
                    );
                }

                grab.enabled = false;

                if (rb != null)
                    rb.isKinematic = true;
            }
        }
    }
}

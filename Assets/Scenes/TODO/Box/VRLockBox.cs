using UnityEngine;


public class SimpleVRBoxLock : MonoBehaviour
{
    public Animator boxAnimator;       // Box Animator
    public string openParameter = "IsOpen"; // Bool in Animator
    public Collider boxTrigger;        // Collider covering inside of box

    private bool lastState;

    void Start()
    {
        lastState = boxAnimator.GetBool(openParameter);
    }

    void Update()
    {
        bool isOpen = boxAnimator.GetBool(openParameter);

        if (isOpen != lastState)
        {
            if (!isOpen)
                LockObjects();
            else
                UnlockObjects();

            lastState = isOpen;
        }
    }

    void LockObjects()
    {
        Collider[] objectsInside = Physics.OverlapBox(
            boxTrigger.bounds.center,
            boxTrigger.bounds.extents,
            boxTrigger.transform.rotation
        );

        foreach (var col in objectsInside)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = col.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (grab != null)
            {
                // Force release if held
                if (grab.isSelected)
                    grab.interactionManager.SelectExit(grab.firstInteractorSelecting, grab);

                grab.enabled = false;
            }

            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    void UnlockObjects()
    {
        Collider[] objectsInside = Physics.OverlapBox(
            boxTrigger.bounds.center,
            boxTrigger.bounds.extents,
            boxTrigger.transform.rotation
        );

        foreach (var col in objectsInside)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = col.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (grab != null)
                grab.enabled = true;
            if (rb != null)
                rb.isKinematic = false;
        }
    }
}
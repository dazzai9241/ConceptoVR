using UnityEngine;

public class ToggleButtonInteractable : Interactable
{
    private bool isOn = false;

    protected override void OnInteract()
    {
        isOn = !isOn;
        if (isOn)
            Debug.Log("Toggled ON");
        else
            Debug.Log("Toggled OFF");
    }
}

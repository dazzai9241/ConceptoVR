using UnityEngine;

public class PressButtonInteractable : Interactable
{
    protected override void OnInteract()
    {
        Debug.Log("Pressed");
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class ProximityToggle : MonoBehaviour
{
    [SerializeField] private float interactRadius = 2f; // How close player must be
    [SerializeField] private string objectName = "Object";

    private bool toggleState = false;
    private Transform player;

    private void Start()
    {
        // Find the player in the scene (must be tagged "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found! Make sure your player is tagged 'Player'.");
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, player.position);

        // Only allow toggle if player is close enough
        if (distance > interactRadius) return;

        // Check input (new Input System)
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            toggleState = !toggleState;
            Debug.Log($"{objectName} Pressed Toggle {(toggleState ? "ON" : "OFF")}");
        }
    }

    // Optional: visualize interact radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}

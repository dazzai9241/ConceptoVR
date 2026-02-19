using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private Camera renderCamera;
    [SerializeField] private TMP_Text tmpText;

    [Header("Display Resolution")]
    [SerializeField] private int displayWidth = 128;
    [SerializeField] private int displayHeght = 48;

    private void Awake()
    {
        Debug.Assert(renderCamera != null);
        Debug.Assert(tmpText != null);
        Debug.Assert(text != null);

        // Disable the camera, will be automatically enabled when calling the Render() method
        renderCamera.enabled = false;

        RenderToScreen();
    }

    public string DisplayText
    {
        get { return text; }
        set
        {
            if (text != value)
            {
                text = value;
                RenderToScreen();
            }
        }
    }

    private void RenderToScreen()
    {
        Debug.Assert(text != null);

        // Set TMP text
        tmpText.text = text;
        tmpText.ForceMeshUpdate();

        renderCamera.Render();
    }
}

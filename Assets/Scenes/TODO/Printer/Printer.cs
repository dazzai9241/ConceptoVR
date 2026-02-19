using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Printer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Camera renderCamera;
    [SerializeField] private CanvasScaler scaler;
    [SerializeField] private Transform paperStartPos;
    [SerializeField] private Transform paperFinalPos;
    [SerializeField] private TMP_Text printerStamp;
    [SerializeField] private AudioSource printerAudio;

    [Header("Texture Settings")]
    [SerializeField] private int textureWidth = 256;
    [SerializeField] private int textureHeight = 256;

    [Header("Print Settings")]
    [SerializeField] private float printMoveDuration = 2f;

    private RenderTexture renderTexture;
    private bool IsPrinting = false;

    private void Awake()
    {
        renderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        scaler.referenceResolution = new Vector2(renderTexture.width, renderTexture.height);
    }

    public void Print(string text)
    {
        if (!IsPrinting)
        {
            StartCoroutine(PrintRoutine(text));
            IsPrinting = true;
        }
    }

    private IEnumerator PrintRoutine(string text)
    {
        // Set TMP text
        printerStamp.text = text;
        printerStamp.ForceMeshUpdate();

        // Render to texture
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        renderCamera.targetTexture = null;

        // Copy to Texture2D
        Texture2D snapshot = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        RenderTexture.active = renderTexture;
        snapshot.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        snapshot.Apply();
        RenderTexture.active = null;


        // Spawn paper at start
        GameObject paper = Instantiate(paperPrefab, paperStartPos.position, paperStartPos.rotation);

        MeshRenderer renderer = paper.GetComponent<MeshRenderer>();
        Material newMat = new Material(renderer.material);
        renderer.material = newMat;
        renderer.material.SetTexture("_BaseMap", snapshot);

        // Disable grabbing while printing
        XRGrabInteractable grab = paper.GetComponent<XRGrabInteractable>();
        Rigidbody rb = paper.GetComponent<Rigidbody>();

        // Disable grabbing while printing
        if (grab != null)
            grab.enabled = false;

        // Make physics safe during movement
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Play audio
        if (printerAudio != null)
            printerAudio.Play();

        // Move paper
        float elapsed = 0f;

        Vector3 startPos = paperStartPos.position;
        Vector3 endPos = paperFinalPos.position;

        while (elapsed < printMoveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / printMoveDuration;

            paper.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        paper.transform.position = endPos;

        // Stop audio
        if (printerAudio != null)
            printerAudio.Stop();

        // Restore physics
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Enable grabbing after printing
        if (grab != null)
            grab.enabled = true;

        IsPrinting = false;
    }

    private void OnDestroy()
    {
        if (renderTexture != null)
            renderTexture.Release();
    }
}

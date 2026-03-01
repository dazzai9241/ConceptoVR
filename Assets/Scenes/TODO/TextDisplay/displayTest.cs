using UnityEngine;

public class displayTest : MonoBehaviour
{
    [SerializeField] TextDisplay textDisplay;
    [SerializeField] Printer printer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDisplay.DisplayText = "Hello";
        printer.Print("Hello");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

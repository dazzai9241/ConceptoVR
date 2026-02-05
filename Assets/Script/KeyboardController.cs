using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class KeyboardController : MonoBehaviour
{
    public float cycleTime = 1f;           
    public int maxCharacters = 10;         
    public List<Key> keys;                  

    private Dictionary<char, Key> keyMap = new Dictionary<char, Key>();
    private Key lastKey = null;
    private float lastPressTime = 0f;
    private List<char> typed = new List<char>();

    void Start()
    {
        keyMap = new Dictionary<char, Key>();

        foreach (var key in keys)
        {
            if (!keyMap.ContainsKey(key.keyChar))
            {
                keyMap.Add(key.keyChar, key);
            }
            else
            {
                Debug.LogWarning("Duplicate key detected: " + key.keyChar);
            }
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        foreach (var entry in keyMap)
        {
            if (WasKeyPressed(entry.Key))
            {
                HandleKeyPress(entry.Value);
                break;
            }
        }
    }

    bool WasKeyPressed(char keyChar)
    {
        switch (keyChar)
        {
            case '2': return Keyboard.current.digit2Key.wasPressedThisFrame;
            case '3': return Keyboard.current.digit3Key.wasPressedThisFrame;
            case '4': return Keyboard.current.digit4Key.wasPressedThisFrame;
            case '5': return Keyboard.current.digit5Key.wasPressedThisFrame;
            case '6': return Keyboard.current.digit6Key.wasPressedThisFrame;
            case '7': return Keyboard.current.digit7Key.wasPressedThisFrame;
            case '8': return Keyboard.current.digit8Key.wasPressedThisFrame;
            case '9': return Keyboard.current.digit9Key.wasPressedThisFrame;
            default: return false;
        }
    }

    void HandleKeyPress(Key key)
    {
        float timeSinceLast = Time.time - lastPressTime;

        if (key == lastKey && timeSinceLast < cycleTime)
        {
            key.Cycle();
            typed[typed.Count - 1] = key.CurrentValue;
        }
        else
        {
            if (typed.Count >= maxCharacters) return;

            key.Reset();
            typed.Add(key.CurrentValue);
        }

        lastKey = key;
        lastPressTime = Time.time;

        DisplayTyped();
    }

    void DisplayTyped()
    {
        string output = "";
        foreach (char c in typed)
            output += c;

        Debug.Log("Typed: " + output);
    }
}
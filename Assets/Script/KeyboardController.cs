using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class KeyboardController : MonoBehaviour
{
    public float cycleTime = 1f;
    public int maxCharacters = 3;   
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
                keyMap.Add(key.keyChar, key);
            else
                Debug.LogWarning("Duplicate key detected: " + key.keyChar);
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            HandleBackspace();
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            HandleSubmit();
            return;
        }

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
            if (typed.Count >= maxCharacters)
            {
                Debug.LogWarning("Character limit reached.");
                return;
            }

            key.Reset();
            typed.Add(key.CurrentValue);
        }

        lastKey = key;
        lastPressTime = Time.time;

        DisplayTyped();
    }

    void HandleBackspace()
    {
        if (typed.Count > 0)
        {
            typed.RemoveAt(typed.Count - 1);
            Debug.Log("Backspace pressed");
            DisplayTyped();
        }
        else
        {
            Debug.LogWarning("Nothing to delete!");
        }
    }

    void HandleSubmit()
    {
        if (typed.Count == 0)
        {
            Debug.LogError("Error: Empty input.");
            return;
        }

        string output = GetTypedString();
        Debug.Log("Submitted: " + output);

        // Optional: clear after submit
        typed.Clear();
        lastKey = null;
    }

    void DisplayTyped()
    {
        Debug.Log("Typed: " + GetTypedString());
    }

    string GetTypedString()
    {
        return new string(typed.ToArray());
    }
}
using UnityEngine;
using System;
[Serializable]
public class Key
{
    public char keyChar;           
    public char[] values;          
    private int index = 0;

    public char CurrentValue => values[index];

    public void Cycle()
    {
        index = (index + 1) % values.Length;
    }

    public void Reset()
    {
        index = 0;
    }
}
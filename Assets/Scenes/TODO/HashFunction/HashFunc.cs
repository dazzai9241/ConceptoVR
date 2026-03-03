using UnityEngine;

namespace Concepto.HashMap
{
    public class HashFunc
    {
        public static int Hash(string key, int boxesCount)
        {
            int hash = 0;

            // Add all the character's ascii values 
            foreach (char c in key)
            {
                hash += c; // add ASCII value
            }

            // Divide the resulting sum by the amount of boxes and get the remainder
            // The remainder will serve as the index for our boxes
            return Mathf.Abs(hash) % boxesCount;
        }
    }
}
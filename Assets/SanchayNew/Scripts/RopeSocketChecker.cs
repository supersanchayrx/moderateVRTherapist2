using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSocketChecker : MonoBehaviour
{

    public therapistDialogues therapistScript;
    // Map rope ends to corresponding sphere names
    public static Dictionary<string, string> ropeToSphereMap = new Dictionary<string, string>
    {
        { "rope1_end", "Sphere" },
        { "rope2_end", "Sphere (1)" },
        { "rope3_end", "Sphere (2)" },
        { "rope4_end", "Sphere (3)" }
    };

    public static Dictionary<string, int> ropeValues = new Dictionary<string, int>
    {
        { "rope1_end", 0 },
        { "rope2_end", 0 },
        { "rope3_end", 0 },
        { "rope4_end", 0 }
    };

    private void OnTriggerEnter(Collider other)
    {
        foreach (var pair in ropeToSphereMap)
        {
            // Check if the current rope end matches this sphere
            if (other.name == pair.Key && this.gameObject.name == pair.Value)
            {
                // Update the value to 1
                if (ropeValues[pair.Key] == 0) // Trigger task only if the value wasn't already 1
                {
                    ropeValues[pair.Key] = 1;
                    Debug.Log($"{pair.Key}: {ropeValues[pair.Key]}");
                    TriggerTask(pair.Key); // Perform your task here
                }
            }
            else if (this.gameObject.name == pair.Value) // Reset other ropes for this sphere
            {
                ropeValues[pair.Key] = 0;
                Debug.Log($"{pair.Key}: {ropeValues[pair.Key]}");
            }
        }
    }

    // Method to trigger your custom task
    private void TriggerTask(string ropeEnd)
    {
        Debug.Log($"Task triggered for {ropeEnd}");
        // Add your custom task logic here
        therapistScript.vineConnected = true;
    }
}

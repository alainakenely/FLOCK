using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores unlocked bird info during the current play session.
/// Resets automatically when exiting Play mode.
/// </summary>
public static class RuntimeBirdProgress
{
    private static HashSet<string> unlockedBirds = new HashSet<string>();

    // Store bird by prefab name
    public static void UnlockBird(string prefabName)
    {
        prefabName = prefabName.Replace("_Clone", "").Trim(); // strip instance suffix
        unlockedBirds.Add(prefabName);
        Debug.Log($"💾 Unlocked bird at runtime: {prefabName}");
        CheckForParallaxPause(prefabName);
    }
    

    public static bool IsUnlocked(string prefabName)
    {
        prefabName = prefabName.Replace("_Clone", "").Trim();
        return unlockedBirds.Contains(prefabName);
    }

    public static void Reset()
    {
        unlockedBirds.Clear();
        Debug.Log("🧹 Runtime bird progress reset");
    }
    // --- NEW CODE BELOW ---
    private static void CheckForParallaxPause(string prefabName)
    {
        if (IsUnlocked("Bird1_3_0") && IsUnlocked("Bird1_1_0"))
        {
            Debug.Log("🕊️ Both target birds unlocked — scheduling parallax pause...");
            ParallaxPauseTrigger.Instance?.TriggerPause();
        }
    }
}
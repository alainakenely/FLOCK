using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores unlocked bird info during the current play session.
/// Resets automatically when exiting Play mode.
/// </summary>
public static class RuntimeBirdProgress
{
    private static HashSet<string> unlockedBirds = new HashSet<string>();

    public static void UnlockBird(string prefabName)
    {
        unlockedBirds.Add(prefabName);
        Debug.Log($"💾 Unlocked bird at runtime: {prefabName}");
    }

    public static bool IsUnlocked(string prefabName)
    {
        return unlockedBirds.Contains(prefabName);
    }

    public static void Reset()
    {
        unlockedBirds.Clear();
        Debug.Log("🧹 Runtime bird progress reset");
    }
}
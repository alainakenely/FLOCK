using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeBirdProgress
{
    private static HashSet<string> unlockedBirds = new HashSet<string>();

    public static void UnlockBird(string prefabName)
    {
        prefabName = prefabName.Replace("_Clone", "").Trim();
        unlockedBirds.Add(prefabName);
        Debug.Log($"💾 Unlocked bird at runtime: {prefabName}");
        CheckForParallaxPause();
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

    private static void CheckForParallaxPause()
    {
        string scene = SceneManager.GetActiveScene().name;

        // --- Mountains Logic ---
        if (scene == "Mountains")
        {
            if (IsUnlocked("Bird1_3_0") && IsUnlocked("Bird1_1_0"))
            {
                Debug.Log("🕊️ Mountains birds unlocked — pausing parallax...");
                ParallaxPauseTrigger.Instance?.TriggerPause();
            }
        }

        // --- Snow Logic ---
        else if (scene == "Snow")
        {
            if (IsUnlocked("Bird3_Egret2_0") && IsUnlocked("Bird3_Egret4_0"))
            {
                Debug.Log("❄️ Snow birds unlocked — pausing parallax...");
                ParallaxPauseTrigger.Instance?.TriggerPause();
            }
        }
    }
}
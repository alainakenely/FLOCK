using UnityEngine;
using System.Collections.Generic;

public static class BirdUnlockData
{
    private const string PrefKey = "UnlockedBirds";

    // store unlocked bird IDs in memory
    private static HashSet<string> unlockedBirds = new HashSet<string>();

    static BirdUnlockData()
    {
        LoadUnlockedBirds();
    }

    public static bool IsUnlocked(string birdId)
    {
        return unlockedBirds.Contains(birdId);
    }

    public static void UnlockBird(string birdId)
    {
        if (unlockedBirds.Add(birdId))
        {
            SaveUnlockedBirds();
        }
    }

    private static void LoadUnlockedBirds()
    {
        string saved = PlayerPrefs.GetString(PrefKey, "");
        unlockedBirds = new HashSet<string>(saved.Split(','));
    }

    private static void SaveUnlockedBirds()
    {
        string data = string.Join(",", unlockedBirds);
        PlayerPrefs.SetString(PrefKey, data);
        PlayerPrefs.Save();
    }
}
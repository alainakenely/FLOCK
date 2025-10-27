using UnityEngine;

public class AddBirdButton : MonoBehaviour
{
    private GameObject collidedBird;

    private void Start()
    {
        // Reset all panels at runtime start
        ResetAllBirdPanels();
    }

    private void ResetAllBirdPanels()
    {
        BirdPanelMarker[] panels = FindObjectsByType<BirdPanelMarker>(FindObjectsSortMode.None);
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
        Debug.Log("🧹 All bird panels reset to inactive at game start");
    }

    // Called by trigger or pointer event to set the bird this button adds
    public void SetCollidedBird(GameObject bird)
    {
        collidedBird = bird;
    }

    public void OnAddBirdButtonClicked()
    {
        if (collidedBird == null)
        {
            Debug.LogWarning("⚠️ No collided bird assigned!");
            return;
        }

        // Skip the OG player bird
        if (collidedBird.CompareTag("PlayerBird"))
        {
            Debug.Log("🟡 Skipping OG Player Bird — already active.");
            return;
        }

        Debug.Log("🕊️ Add Bird button clicked — " + collidedBird.name);

        // Tag as player bird
        collidedBird.tag = "PlayerBird";

        // Disable AI if exists
        var ai = collidedBird.GetComponent<MonoBehaviour>();
        if (ai != null && ai.GetType().Name.Contains("AI"))
            ai.enabled = false;

        // Add keyboard control if missing
        if (collidedBird.GetComponent<FollowerFlightKeyboard>() == null)
            collidedBird.AddComponent<FollowerFlightKeyboard>();

        // Add to flock
        var flockManager = FindObjectOfType<FlockManager>();
        if (flockManager != null)
            flockManager.AddToFlock(collidedBird);

        // Unlock the panel
        UnlockBirdPanel(collidedBird);
    }

    private void UnlockBirdPanel(GameObject bird)
    {
        BirdPrefabMarker marker = bird.GetComponent<BirdPrefabMarker>();
        if (marker == null || marker.prefabReference == null)
        {
            Debug.LogWarning($"⚠ {bird.name} has no valid BirdPrefabMarker or prefabReference!");
            return;
        }

        BirdPanelMarker[] panels = FindObjectsByType<BirdPanelMarker>(FindObjectsSortMode.None);
        foreach (var panel in panels)
        {
            if (panel.prefabReference != null && panel.prefabReference == marker.prefabReference)
            {
                panel.gameObject.SetActive(true);
                Debug.Log($"✅ Activated panel for prefab '{marker.prefabReference.name}' (runtime only)");
                return;
            }
        }

        Debug.LogWarning($"⚠ No matching panel found for prefab '{marker.prefabReference.name}'");
    }
}
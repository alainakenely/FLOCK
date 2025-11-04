using UnityEngine;

public class AddBirdButton : MonoBehaviour
{
    private GameObject collidedBird;

    // Called by trigger or pointer event
    public void SetCollidedBird(GameObject bird)
    {
        collidedBird = bird;
    }

    public void OnAddBirdButtonClicked()
    {
        if (collidedBird == null)
        {
            Debug.LogWarning("⚠️ No bird assigned to add!");
            return;
        }

        if (collidedBird.CompareTag("PlayerBird"))
        {
            Debug.Log("🟡 Bird already added.");
            return;
        }

        collidedBird.tag = "PlayerBird";

        // Disable AI if exists
        var ai = collidedBird.GetComponent<MonoBehaviour>();
        if (ai != null && ai.GetType().Name.Contains("AI"))
            ai.enabled = false;

        // Add player control if missing
        if (collidedBird.GetComponent<FollowerFlightKeyboard>() == null)
            collidedBird.AddComponent<FollowerFlightKeyboard>();

        // Add to flock
        var flockManager = FindObjectOfType<FlockManager>();
        if (flockManager != null)
            flockManager.AddToFlock(collidedBird);

        // Unlock bird
        RuntimeBirdProgress.UnlockBird(collidedBird.name);

        Debug.Log($"🕊️ Bird added and unlocked: {collidedBird.name}");
    }
}
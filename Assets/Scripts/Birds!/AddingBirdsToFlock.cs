using UnityEngine;

public class AddBirdButton : MonoBehaviour
{
    private GameObject collidedBird;

    public void SetCollidedBird(GameObject bird)
    {
        collidedBird = bird;
    }

    public void OnAddBirdButtonClicked()
    {
        if (collidedBird == null)
        {
            Debug.LogWarning("⚠️ No collided bird assigned to AddBirdButton!");
            return;
        }

        Debug.Log("🕊️ Add Bird button clicked — " + collidedBird.name + " is now controllable!");

        // Change tag so it behaves like the player bird
        collidedBird.tag = "PlayerBird";

        // Disable any random movement AI (if it exists)
        var ai = collidedBird.GetComponent<MonoBehaviour>();
        if (ai != null && ai.GetType().Name.Contains("AI"))
            ai.enabled = false;

        // Add FollowerFlightV3 if not present
        var follower = collidedBird.GetComponent<FollowerFlightKeyboard>();
        if (follower == null)
            follower = collidedBird.AddComponent<FollowerFlightKeyboard>();

        // Reference the player bird and canvas via FlockManager
        FlockManager flockManager = FindFirstObjectByType<FlockManager>();
        if (flockManager != null)
        {
            // The flock manager handles leader assignment and formation offsets
            flockManager.AddToFlock(collidedBird);
        }
        else
        {
            Debug.LogWarning("⚠️ No FlockManager found in scene!");
        }
    }
}
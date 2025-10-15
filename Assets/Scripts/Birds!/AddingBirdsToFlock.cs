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
        if (collidedBird != null)
        {
            Debug.Log("🕊️ Add Bird button clicked — " + collidedBird.name + " is now controllable!");

            // Change the tag so it behaves like the player bird
            collidedBird.tag = "PlayerBird";

            // Add the FlyBehavior script if not already present
            FlyBehavior fly = collidedBird.GetComponent<FlyBehavior>();
            if (fly == null)
                fly = collidedBird.AddComponent<FlyBehavior>();

            // Optional: disable any AI script the bird had
            var ai = collidedBird.GetComponent<MonoBehaviour>();
            if (ai != null && ai.GetType().Name.Contains("AI"))
                ai.enabled = false;

            // Add to flock via FlockManager
            FlockManager flockManager = FindFirstObjectByType<FlockManager>();
            if (flockManager != null)
            {
                flockManager.AddToFlock(collidedBird);
            }
            else
            {
                Debug.LogWarning("⚠️ No FlockManager found in scene!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No collided bird assigned to AddBirdButton!");
        }
    }
}
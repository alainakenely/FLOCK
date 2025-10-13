using UnityEngine;

public class AddBirdButton : MonoBehaviour
{
    private GameObject collidedBird;

    // Called by your collision script when the player hits another bird
    public void SetCollidedBird(GameObject bird)
    {
        collidedBird = bird;
    }

    // Called by the button’s OnClick event
    public void OnAddBirdButtonClicked()
    {
        if (collidedBird != null)
        {
            Debug.Log("🕊️ Add Bird button clicked — " + collidedBird.name + " is now controllable!");

            // Change the tag so it behaves like the player bird
            collidedBird.tag = "PlayerBird";

            // Add the FlyBehavior script so it can be controlled
            if (collidedBird.GetComponent<FlyBehavior>() == null)
                collidedBird.AddComponent<FlyBehavior>();

            // Optional: disable any AI script the bird had before
            var ai = collidedBird.GetComponent<MonoBehaviour>();
            if (ai != null && ai.GetType().Name.Contains("AI"))
                ai.enabled = false;
        }
        else
        {
            Debug.LogWarning("⚠️ No collided bird assigned to AddBirdButton!");
        }
    }
}
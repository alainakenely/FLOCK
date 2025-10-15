using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public List<GameObject> flockBirds = new List<GameObject>();
    public Transform playerBird; // Assign the main player bird in inspector
    public float xSpacing = 1.5f; // Horizontal spacing for V-formation
    public float ySpacing = 1f;   // Vertical spacing for V-formation
    public Canvas flightCanvas;    // Assign your canvas to allow bounds clamping

    public void AddToFlock(GameObject newBird)
    {
        if (!flockBirds.Contains(newBird))
        {
            flockBirds.Add(newBird);

            int index = flockBirds.Count; // 1-based for formation calculation
            Vector3 offset;

            // Staggered V formation (alternate top/bottom)
            if (index % 2 == 0)
                offset = new Vector3(-xSpacing * (index / 2), -ySpacing * (index / 2), 0f); // bottom-left
            else
                offset = new Vector3(-xSpacing * ((index + 1) / 2), ySpacing * ((index + 1) / 2), 0f); // top-left

            FlyBehavior fly = newBird.GetComponent<FlyBehavior>();
            if (fly != null)
            {
                fly.playerLeader = playerBird;
                fly.formationOffset = offset;
                fly.isLeader = false;
                fly.flightCanvas = flightCanvas;

                // Snap immediately to formation position
                newBird.transform.position = playerBird.position + offset;

                // Flip localScale.x to face right
                Vector3 scale = newBird.transform.localScale;
                scale.x = -Mathf.Abs(scale.x); 
                newBird.transform.localScale = scale;
            }

            Debug.Log("🪶 Added " + newBird.name + " to flock at offset " + offset);
        }
    }
}
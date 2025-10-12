using UnityEngine;
using System.Collections.Generic;

public class FlockController : MonoBehaviour
{
    public Transform playerBird;
    public List<Transform> collectedBirds = new List<Transform>();

    // Predefined relative positions for 2-3 birds in a triangle/pyramid
    private Vector3[] pyramidOffsets2 = new Vector3[]
    {
        new Vector3(-1f, -1f, 0f),
        new Vector3(1f, -1f, 0f)
    };
    
    private Vector3[] pyramidOffsets3 = new Vector3[]
    {
        new Vector3(0f, 0f, 0f),     // player bird
        new Vector3(-1f, -1f, 0f),   // left
        new Vector3(1f, -1f, 0f)     // right
    };

    void Update()
    {
        for (int i = 0; i < collectedBirds.Count; i++)
        {
            Vector3 offset = Vector3.zero;

            // Choose offset set based on number of birds
            if (collectedBirds.Count == 2 && i < pyramidOffsets2.Length)
                offset = pyramidOffsets2[i];
            else if (collectedBirds.Count == 3 && i < pyramidOffsets3.Length)
                offset = pyramidOffsets3[i];

            // Smoothly move each bird to its offset position
            collectedBirds[i].position = Vector3.Lerp(collectedBirds[i].position, playerBird.position + offset, 0.2f);
        }
    }

    public void AddBird(Transform newBird)
    {
        newBird.tag = "PlayerBird"; // now part of player flock
        collectedBirds.Add(newBird);

        // Optional: enable FlyBehavior
        FlyBehavior fly = newBird.GetComponent<FlyBehavior>();
        if (fly != null)
            fly.enabled = true;
    }
}
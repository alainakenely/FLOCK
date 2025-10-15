using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public List<GameObject> flockBirds = new List<GameObject>();
    public Canvas flightCanvas; // assign in inspector
    public GameObject playerBird; // assign your main bird here

    [Header("Formation Settings")]
    public Vector2 formationOffset = new Vector2(1f, 0.5f); // spacing for V formation
    public float followSmoothness = 5f; // how quickly birds catch up to formation

    void Update()
    {
        if (playerBird == null || flockBirds.Count == 0)
            return;

        // Reference the player’s current position (and movement from FlyBehavior)
        Vector3 playerPos = playerBird.transform.position;

        // Make each bird hold a position relative to the player (V-shape)
        for (int i = 0; i < flockBirds.Count; i++)
        {
            GameObject bird = flockBirds[i];
            if (bird == null) continue;

            // Alternate sides for V shape
            int side = (i % 2 == 0) ? 1 : -1;
            int row = i / 2;

            // Offset each bird relative to player
            Vector3 targetPos = playerPos + new Vector3(
                side * formationOffset.x * (row + 1),
                -formationOffset.y * (row + 1),
                0f
            );

            // Smoothly move bird toward its formation spot
            bird.transform.position = Vector3.Lerp(
                bird.transform.position,
                targetPos,
                Time.deltaTime * followSmoothness
            );
        }
    }

    public void AddToFlock(GameObject newBird)
    {
        if (!flockBirds.Contains(newBird))
        {
            flockBirds.Add(newBird);

            FlyBehavior fly = newBird.GetComponent<FlyBehavior>();
            if (fly == null)
                fly = newBird.AddComponent<FlyBehavior>();

            fly.flightCanvas = flightCanvas;

            // MAKES BIRD FACE RIGHT DO NOT DELETE
            Vector3 scale = newBird.transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            newBird.transform.localScale = scale;

            Debug.Log("🪶 Added " + newBird.name + " to flock.");
        }
    }
}
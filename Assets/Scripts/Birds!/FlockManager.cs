using UnityEngine;
using System.Collections.Generic;

public class FlockManager : MonoBehaviour
{
    [Header("Flock Setup")]
    public Canvas flightCanvas;         // Assign your canvas here
    public GameObject playerBird;       // Assign your main bird
    public float parallaxSpeed = 3f;
    public Vector2 formationOffset = new Vector2(1f, 0.5f);
    public float followLerp = 8f;       // Optional smoothing

    [HideInInspector] public List<GameObject> flockBirds = new List<GameObject>();

    /// <summary>
    /// Adds an existing bird GameObject to the flock, sets formation, and configures keyboard follower.
    /// </summary>
    public void AddToFlock(GameObject newBird)
    {
        if (newBird == null || flockBirds.Contains(newBird))
            return;

        flockBirds.Add(newBird);

        // Determine leader for this bird: first bird follows player, others follow previous in flock
        Transform leaderTransform = (flockBirds.Count == 1) ? playerBird.transform : flockBirds[flockBirds.Count - 2].transform;

        // Add or get FollowerFlightKeyboard component
        FollowerFlightKeyboard follower = newBird.GetComponent<FollowerFlightKeyboard>();
        if (follower == null)
            follower = newBird.AddComponent<FollowerFlightKeyboard>();

        follower.flightCanvas = flightCanvas;
        follower.parallaxSpeed = parallaxSpeed;

        // Compute V-formation offset
        int index = flockBirds.Count;
        int side = (index % 2 == 0) ? 1 : -1;
        int row = index / 2;
        follower.formationOffset = new Vector3(
            -formationOffset.x * (row + 1),
            side * formationOffset.y * (row + 1),
            0f
        );

        // Make bird face right
        Vector3 scale = newBird.transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        newBird.transform.localScale = scale;

        Debug.Log($"🪶 Added {newBird.name} to flock. Leader: {leaderTransform.name}");
    }
}
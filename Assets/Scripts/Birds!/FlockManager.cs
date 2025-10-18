using UnityEngine;
using System.Collections.Generic;

public class FlockManager : MonoBehaviour
{
    [Header("Flock Setup")]
    public Canvas flightCanvas;
    public GameObject playerBird;
    public float parallaxSpeed = 3f;

    [Header("Formation Controls")]
    public Vector2 formationOffset = new Vector2(1f, 0.5f);
    [Tooltip("Controls how quickly each row steps back")]
    public float rowSpacingMultiplier = 0.75f; // smaller = tighter rows
    [Tooltip("Starting number of birds in the first row (then increases each row)")]
    public int startingBirdsPerRow = 2;
    public float followLerp = 8f;

    [HideInInspector] public List<GameObject> flockBirds = new List<GameObject>();

    public void AddToFlock(GameObject newBird)
    {
        if (newBird == null || flockBirds.Contains(newBird))
            return;

        flockBirds.Add(newBird);

        Transform leaderTransform = (flockBirds.Count == 1)
            ? playerBird.transform
            : flockBirds[flockBirds.Count - 2].transform;

        FollowerFlightKeyboard follower = newBird.GetComponent<FollowerFlightKeyboard>();
        if (follower == null)
            follower = newBird.AddComponent<FollowerFlightKeyboard>();

        follower.flightCanvas = flightCanvas;
        follower.parallaxSpeed = parallaxSpeed;

        int index = flockBirds.Count - 1;

        // Figure out which row this bird belongs to
        int row = 0;
        int birdsInPreviousRows = 0;
        int birdsInThisRow = startingBirdsPerRow;

        while (index >= birdsInPreviousRows + birdsInThisRow)
        {
            birdsInPreviousRows += birdsInThisRow;
            row++;
            birdsInThisRow++;
        }

        int positionInRow = index - birdsInPreviousRows;

        // alternate left/right based on position in row
        int side = (positionInRow % 2 == 0) ? 1 : -1;

        // horizontal offset (spread the row symmetrically)
        float horizontalOffset = (positionInRow - (birdsInThisRow - 1) / 2f) * formationOffset.x;

        // assign final position offset for this bird
        follower.formationOffset = new Vector3(
            -formationOffset.x * (row * rowSpacingMultiplier + 1),
            horizontalOffset,
            0f
        );

        // Make bird face right
        Vector3 scale = newBird.transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        newBird.transform.localScale = scale;

        Debug.Log($"🪶 Added {newBird.name} | Row {row + 1} ({birdsInThisRow} birds) | PosInRow {positionInRow}");
    }
}
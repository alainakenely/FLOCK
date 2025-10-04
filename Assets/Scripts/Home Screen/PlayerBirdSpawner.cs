using UnityEngine;
using System.Collections.Generic;

public class PlayerBirdSpawner : MonoBehaviour
{
    public Transform spawnParent;      // Parent in the hierarchy for organization
    public Vector3 startPosition = new Vector3(0f, 0f, 0f); // Bottom-left of the pyramid
    public float xSpacing = 2f;        // Horizontal spacing between birds
    public float ySpacing = 2f;        // Vertical spacing between rows
    public float birdScale = 1f;       // Scale factor for birds

    void Start()
    {
        List<GameObject> selectedBirds = BirdSelectionData.selectedBirds;
        if (selectedBirds == null || selectedBirds.Count == 0)
            return;

        int row = 0;
        int birdsInRow = 1;
        int birdsPlacedInRow = 0;

        for (int i = 0; i < selectedBirds.Count; i++)
        {
            // Calculate position for pyramid/grid
            Vector3 spawnPos = GetSpawnPositionForNewBird(birdsInRow, birdsPlacedInRow, row);

            // Instantiate bird prefab
            GameObject bird = Instantiate(selectedBirds[i], spawnPos, Quaternion.identity, spawnParent);
            bird.name = selectedBirds[i].name;

            // Force world z to 0 so collisions work
            Vector3 worldPos = bird.transform.position;
            worldPos.z = 0f;
            bird.transform.position = worldPos;

            // Adjust scale
            bird.transform.localScale = Vector3.one * birdScale;

            // Flip Y-axis
            Vector3 localScale = bird.transform.localScale;
            localScale.x *= -1; // flips the bird
            bird.transform.localScale = localScale;

            // ✅ Add FlyBehavior only for flock birds
            if (bird.GetComponent<FlyBehavior>() == null)
            {
                bird.AddComponent<FlyBehavior>();
            }

            // Update counters for pyramid placement
            birdsPlacedInRow++;
            if (birdsPlacedInRow >= birdsInRow)
            {
                row++;
                birdsInRow++;
                birdsPlacedInRow = 0;
            }
        }
    }

    private Vector3 GetSpawnPositionForNewBird(int birdsInRow, int birdsPlacedInRow, int row)
    {
        float xOffset = (birdsInRow - 1) * -0.5f * xSpacing + birdsPlacedInRow * xSpacing;
        float yOffset = row * ySpacing;
        Vector3 spawnPos = startPosition + new Vector3(xOffset, yOffset, 0f);
        spawnPos.z = 0f; // ensure z is 0
        return spawnPos;
    }
}
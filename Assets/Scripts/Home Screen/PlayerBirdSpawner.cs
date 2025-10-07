using UnityEngine;
using System.Collections.Generic;

public class PlayerBirdSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public Transform spawnParent;          // Optional parent in hierarchy
    public Vector3 startPosition = Vector3.zero; // Bottom-left of the pyramid
    public float xSpacing = 2f;            // Horizontal spacing between birds
    public float ySpacing = 2f;            // Vertical spacing between rows
    public float birdScale = 0.5f;         // Match RandomBirdSpawner scale

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
            // Calculate pyramid/grid position
            Vector3 spawnPos = GetSpawnPositionForNewBird(birdsInRow, birdsPlacedInRow, row);

            // Instantiate bird without inheriting parent scale issues
            GameObject bird = Instantiate(selectedBirds[i], spawnPos, Quaternion.identity);
            bird.name = selectedBirds[i].name;

            // Parent it manually if needed
            if (spawnParent != null)
            {
                bird.transform.SetParent(spawnParent, false); // false = keep local position/scale
            }

            // Force correct scale and orientation
            bird.transform.localScale = Vector3.one * birdScale; // uniform scale
            Vector3 scale = bird.transform.localScale;
            scale.x = Mathf.Abs(scale.x); // face right
            bird.transform.localScale = scale;

            // Ensure Z = 0 for collisions
            Vector3 worldPos = bird.transform.position;
            worldPos.z = 0f;
            bird.transform.position = worldPos;

            // Add FlyBehavior if desired
            if (bird.GetComponent<FlyBehavior>() == null)
            {
                bird.AddComponent<FlyBehavior>();
            }

            // Update pyramid placement counters
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
        spawnPos.z = 0f;
        return spawnPos;
    }
}
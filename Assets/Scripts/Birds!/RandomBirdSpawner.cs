using System.Collections.Generic;
using UnityEngine;

public class RandomBirdSpawner : MonoBehaviour
{
    public GameObject[] birdPrefabs;          // All possible bird prefabs
    public RectTransform spawnPanel;          // The moving panel in the canvas
    public GameObject addBirdPanel;           // Panel to activate on collision
    public float spawnInterval = 5f;          // Time between spawns
    public float birdScale = 0.5f;            // Scale factor for spawned birds
    public float xOffset = 1f;                // Extra distance outside the right edge

    private float timer;
    private List<GameObject> spawnedBirds = new List<GameObject>();
    private bool gamePaused = false;

    void Update()
    {
        if (!gamePaused)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnBird();
                timer = 0f;
            }

            DestroyOffscreenBirds();
        }
    }

    void SpawnBird()
    {
        if (birdPrefabs.Length == 0 || spawnPanel == null)
            return;

        GameObject birdPrefab = birdPrefabs[Random.Range(0, birdPrefabs.Length)];

        Vector3[] corners = new Vector3[4];
        spawnPanel.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        float x = topRight.x + xOffset;                    // Just outside right edge
        float y = Random.Range(bottomLeft.y, topRight.y);  // Random Y within panel

        GameObject bird = Instantiate(birdPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        bird.transform.localScale = Vector3.one * birdScale;

        // Add collision handler dynamically
        BirdCollisionHandler handler = bird.AddComponent<BirdCollisionHandler>();
        handler.spawner = this;

        spawnedBirds.Add(bird);
    }

    void DestroyOffscreenBirds()
    {
        if (spawnPanel == null) return;

        Vector3[] corners = new Vector3[4];
        spawnPanel.GetWorldCorners(corners);
        float leftEdgeX = corners[0].x;

        for (int i = spawnedBirds.Count - 1; i >= 0; i--)
        {
            if (spawnedBirds[i].transform.position.x < leftEdgeX)
            {
                Destroy(spawnedBirds[i]);
                spawnedBirds.RemoveAt(i);
            }
        }
    }

    public void PauseGame(GameObject bird)
    {
        gamePaused = true;
        addBirdPanel.SetActive(true);

        // Optionally store the bird for later snapping to the pyramid
        BirdCollisionHandler.currentBird = bird;
    }

    public void ResumeGame(bool addBird)
    {
        if (addBird && BirdCollisionHandler.currentBird != null)
        {
            // Snap bird to pyramid here (call your existing reposition logic)
            // Example: BirdSelectionController.AddBirdToGrid(BirdCollisionHandler.currentBird);
        }

        BirdCollisionHandler.currentBird = null;
        gamePaused = false;
        addBirdPanel.SetActive(false);
    }
}

// Separate script to handle collisions
public class BirdCollisionHandler : MonoBehaviour
{
    public RandomBirdSpawner spawner;
    public static GameObject currentBird;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bird"))
        {
            spawner.PauseGame(this.gameObject);
        }
    }
}
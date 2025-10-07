using System.Collections.Generic;
using UnityEngine;

public class RandomBirdSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] birdPrefabs;          // All possible bird prefabs
    public float spawnInterval = 5f;          // Time between spawns
    public float birdScale = 0.5f;            // Scale factor for spawned birds
    public float xOffset = 1f;                // Extra distance outside the right edge
    public GameObject addBirdPanel;           // Panel to activate on collision
    public Canvas canvas;                     // ✅ Drag your World Space Canvas here in the Inspector

    private float timer;
    private List<GameObject> spawnedBirds = new List<GameObject>();
    private bool gamePaused = false;

    // Optional: define bounds manually or reference a moving panel for Y limits
    public float minY = -4f;
    public float maxY = 4f;

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
        if (birdPrefabs.Length == 0)
        {
            Debug.LogWarning("birdPrefabs is empty!");
            return;
        }

        // Randomly pick a prefab
        GameObject birdPrefab = birdPrefabs[Random.Range(0, birdPrefabs.Length)];

        // Calculate spawn position just outside the right of the screen
        float x = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + xOffset;
        float y = Random.Range(minY, maxY);

        // Instantiate the bird
        GameObject bird = Instantiate(birdPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        

        bird.transform.localScale = Vector3.one * birdScale;

        // Add collision handler dynamically
        BirdCollisionHandler handler = bird.AddComponent<BirdCollisionHandler>();
        handler.spawner = this;

        spawnedBirds.Add(bird);

        Debug.Log("Spawned bird: " + bird.name + " at position " + bird.transform.position);
    }

    void DestroyOffscreenBirds()
    {
        float leftEdgeX = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;

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

        BirdCollisionHandler.currentBird = bird;
    }

    public void ResumeGame(bool addBird)
    {
        if (addBird && BirdCollisionHandler.currentBird != null)
        {
            // Snap bird to pyramid here
        }

        BirdCollisionHandler.currentBird = null;
        gamePaused = false;
        addBirdPanel.SetActive(false);
    }
}

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
using System.Collections.Generic;
using UnityEngine;

public class RandomBirdSpawner : MonoBehaviour
{
    [Header("Spawner Settings")] 
    public GameObject[] birdPrefabs;       // Bird prefabs to spawn
    public float spawnInterval = 0.5f;     // Time between spawns
    public float birdScale = 0.5f;         // Scale of spawned birds
    public float xOffset = 1f;             // Distance outside canvas right edge
    public Canvas canvas;                  // World Space Canvas reference

    [Header("Special Spawn Timing")]
    public string delayedBirdName = "Bird1_3_0"; // Bird to delay
    public float delayTime = 20f;                // Seconds before allowing spawn

    private float timer;
    private float sceneTimer;
    private List<GameObject> spawnedBirds = new List<GameObject>();
    private bool gamePaused = false;
    private bool spawningDisabled = false;

    void Start()
    {
        sceneTimer = 0f; // starts counting from scene load
    }

    void Update()
    {
        if (gamePaused || spawningDisabled) return;

        // Count time since scene started
        sceneTimer += Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBird();
            timer = 0f;
        }
    }

    void SpawnBird()
    {
        if (birdPrefabs.Length == 0 || canvas == null) return;

        // Filter out unlocked birds AND time-gated ones
        List<GameObject> availableBirds = new List<GameObject>();
        foreach (var b in birdPrefabs)
        {
            // Skip if unlocked
            if (RuntimeBirdProgress.IsUnlocked(b.name))
                continue;

            // Skip if this is the delayed bird and time hasn’t reached threshold
            if (b.name == delayedBirdName && sceneTimer < delayTime)
            {
                // Uncomment for debug:
                // Debug.Log($"⏳ Delaying {b.name} spawn until {delayTime}s. Current time: {sceneTimer:F1}s");
                continue;
            }

            availableBirds.Add(b);
        }

        // Stop spawning if all are unlocked or delayed
        if (availableBirds.Count == 0)
        {
            spawningDisabled = true;
            Debug.Log("✅ All available birds unlocked or delayed! RandomBirdSpawner disabled.");
            enabled = false;
            return;
        }

        // Pick a random locked, allowed bird
        GameObject chosenPrefab = availableBirds[Random.Range(0, availableBirds.Count)];
        if (chosenPrefab == null) return;

        // Spawn logic
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        float rightEdgeX = corners[2].x;
        float bottomY = corners[0].y;
        float topY = corners[1].y;

        float x = rightEdgeX + xOffset;
        float y = Random.Range(bottomY, topY);

        GameObject bird = Instantiate(chosenPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        bird.name = chosenPrefab.name + "_Clone";
        bird.transform.localScale = Vector3.one * birdScale;
        bird.SetActive(true);

        spawnedBirds.Add(bird);
        Debug.Log($"🕊️ Spawned bird: {chosenPrefab.name} at ({x}, {y}) | Time: {sceneTimer:F1}s");
    }
}
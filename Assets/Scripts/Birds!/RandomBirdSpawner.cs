using System.Collections.Generic;
using UnityEngine;

public class RandomBirdSpawner : MonoBehaviour
{
    [Header("Spawner Settings")] 
    public GameObject[] birdPrefabs;
    public float spawnInterval = 0.5f;
    public float birdScale = 0.5f;
    public float xOffset = 1f;
    public Canvas canvas;
    public float despawnTime = 20f; // ⏳ Despawn after 20 seconds

    [Header("Special Spawn Timing")]
    public string delayedBirdName = "Bird1_3_0";
    public float delayTime = 20f;

    private float timer;
    private float sceneTimer;
    private List<GameObject> spawnedBirds = new List<GameObject>();
    private bool gamePaused = false;
    private bool spawningDisabled = false;

    void Start()
    {
        sceneTimer = 0f;
    }

    void Update()
    {
        if (gamePaused || spawningDisabled) return;

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

        List<GameObject> availableBirds = new List<GameObject>();
        foreach (var b in birdPrefabs)
        {
            if (RuntimeBirdProgress.IsUnlocked(b.name))
                continue;

            if (b.name == delayedBirdName && sceneTimer < delayTime)
                continue;

            availableBirds.Add(b);
        }

        if (availableBirds.Count == 0)
        {
            spawningDisabled = true;
            Debug.Log("✅ All available birds unlocked or delayed! RandomBirdSpawner disabled.");
            enabled = false;
            return;
        }

        GameObject chosenPrefab = availableBirds[Random.Range(0, availableBirds.Count)];
        if (chosenPrefab == null) return;

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

        // ⏳ Despawn bird after 20 seconds
        Destroy(bird, despawnTime);

        Debug.Log($"🕊️ Spawned bird: {chosenPrefab.name} at ({x}, {y}) | Time: {sceneTimer:F1}s");
    }
}
using System.Collections.Generic;
using UnityEngine;

public class RandomBirdSpawner : MonoBehaviour
{
    [Header("Spawner Settings")] 
    public GameObject[] birdPrefabs;       // Bird prefabs to spawn
    public float spawnInterval = 1f;       // Time between spawns
    public float birdScale = 0.5f;         // Scale of spawned birds
    public float xOffset = 1f;             // Extra distance outside the canvas right edge
    public Canvas canvas;                  // World Space Canvas reference

    private float timer;
    private List<GameObject> spawnedBirds = new List<GameObject>();
    private bool gamePaused = false;

    void Update()
    {
        if (gamePaused) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBird();
            timer = 0f;
        }

        // Temporarily disable destruction to see birds
        //DestroyOffscreenBirds();
    }

    void SpawnBird()
    {
        if (birdPrefabs.Length == 0 || canvas == null) return;

        GameObject prefab = birdPrefabs[Random.Range(0, birdPrefabs.Length)];
        if (prefab == null) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Recalculate world corners dynamically every spawn
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners); // 0=BL, 1=TL, 2=TR, 3=BR
        float rightEdgeX = corners[2].x;   // Top-right X
        float bottomY = corners[0].y;
        float topY = corners[1].y;

        float x = rightEdgeX + xOffset;     // Dynamic right edge
        float y = Random.Range(bottomY, topY);

        GameObject bird = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);
        bird.name = prefab.name + "_Clone";
        bird.transform.localScale = Vector3.one * birdScale;
        bird.SetActive(true);

        spawnedBirds.Add(bird);

        Debug.Log("Spawned bird at X: " + x + ", Y: " + y);
    }
}
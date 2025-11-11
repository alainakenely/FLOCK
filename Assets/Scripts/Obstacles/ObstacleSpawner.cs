using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public Sprite[] obstacleSprites; // Sprites to spawn
    public float spawnInterval = 1f;
    public float despawnTime = 20f;  // ⏳ Despawn time in seconds
    public Canvas canvas;            
    public float xOffset = 1f;       
    public float obstacleScale = 1f; 

    private float timer;
    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private bool gamePaused = false;

    public DeathPanel deathPanel; // assign in inspector

    void Update()
    {
        if (gamePaused) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        if (obstacleSprites.Length == 0 || canvas == null) return;

        Sprite selectedSprite = obstacleSprites[Random.Range(0, obstacleSprites.Length)];
        if (selectedSprite == null) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        float rightEdgeX = corners[2].x;
        float bottomY = corners[0].y;
        float topY = corners[1].y;

        float x = rightEdgeX + xOffset;
        float y = Random.Range(bottomY, topY);

        GameObject obstacle = new GameObject("Obstacle_" + selectedSprite.name);
        obstacle.transform.position = new Vector3(x, y, 0f);
        obstacle.transform.localScale = Vector3.one * obstacleScale;

        SpriteRenderer sr = obstacle.AddComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;

        BoxCollider2D col = obstacle.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        // Add collision behavior and assign DeathPanel
        ObstacleCollisionTest collision = obstacle.AddComponent<ObstacleCollisionTest>();
        collision.deathPanel = deathPanel;

        spawnedObstacles.Add(obstacle);

        // ⏳ Schedule despawn
        Destroy(obstacle, despawnTime);
    }

    public void StopSpawning()
    {
        gamePaused = true;
        Debug.Log("🛑 ObstacleSpawner stopped because parallax movement ended.");
    }

    public void SetPaused(bool paused)
    {
        gamePaused = paused;
    }
}
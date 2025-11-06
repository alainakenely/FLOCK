using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public Sprite[] obstacleSprites; // Sprites to spawn
    public float spawnInterval = 1f;
    public Canvas canvas;            // World-space canvas (for bounds)
    public float xOffset = 1f;       // Distance off right edge
    public float obstacleScale = 1f; // Scale of the spawned sprite

    private float timer;
    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private bool gamePaused = false; // check pause

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

    public DeathPanel deathPanel; // assign in inspector

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
        collision.deathPanel = deathPanel; // <<<<< THIS IS CRUCIAL

        spawnedObstacles.Add(obstacle);
    }

    public void SetPaused(bool paused)
    {
        gamePaused = paused;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class PlayerBirdSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public Vector3 startPosition = Vector3.zero; // Bottom-left of the pyramid
    public float xSpacing = 2f;                  // Horizontal spacing between birds
    public float ySpacing = 2f;                  // Vertical spacing between rows
    public float birdScale = 0.5f;               // Match RandomBirdSpawner scale

    [Header("Flight Settings")]
    public float parallaxSpeed = 2f;             // Speed synced with background
    public float verticalSpeed = 3f;             // Up/down motion
    public float horizontalInputMultiplier = 0.3f; // Scale for A/D left/right movement
    public Canvas flightCanvas;                  // Assign your moving canvas here

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
            Vector3 spawnPos = GetSpawnPositionForNewBird(birdsInRow, birdsPlacedInRow, row);

            GameObject bird = Instantiate(selectedBirds[i], spawnPos, Quaternion.identity);
            bird.name = selectedBirds[i].name;

            // Force exact scale like random birds
            bird.transform.localScale = Vector3.one * birdScale;

            // Force facing right
            SpriteRenderer sr = bird.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = true;
            }

            // Ensure Z = 0
            Vector3 pos = bird.transform.position;
            pos.z = 0f;
            bird.transform.position = pos;

            // Add PlayerBirdFlight for parallax and input
            PlayerBirdFlight flight = bird.AddComponent<PlayerBirdFlight>();
            flight.parallaxSpeed = parallaxSpeed;
            flight.verticalSpeed = verticalSpeed;
            flight.horizontalInputMultiplier = horizontalInputMultiplier;
            flight.flightCanvas = flightCanvas;

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

public class PlayerBirdFlight : MonoBehaviour
{
    [HideInInspector] public float parallaxSpeed;
    [HideInInspector] public float verticalSpeed;
    [HideInInspector] public float horizontalInputMultiplier = 0.3f;
    [HideInInspector] public Canvas flightCanvas;

    void Update()
    {
        if (flightCanvas == null) return;

        RectTransform canvasRect = flightCanvas.GetComponent<RectTransform>();
        Vector3 pos = transform.position;

        // Constant forward motion
        pos.x += parallaxSpeed * Time.deltaTime;

        // Keyboard input
        float verticalInput = Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f);
        float horizontalInput = Input.GetKey(KeyCode.D) ? 1f : (Input.GetKey(KeyCode.A) ? -1f : 0f);

        pos.x += horizontalInput * parallaxSpeed * horizontalInputMultiplier * Time.deltaTime;
        pos.y += verticalInput * verticalSpeed * Time.deltaTime;

        // Clamp inside canvas bounds (Screen Space - Camera)
        Vector2 canvasSize = canvasRect.sizeDelta;
        Camera cam = flightCanvas.worldCamera;

        // Convert canvas size to world units
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(canvasSize.x, canvasSize.y, cam.nearClipPlane));

        // Get bird half-size using SpriteRenderer
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float halfWidth = sr != null ? sr.bounds.size.x / 2f : 0.5f;
        float halfHeight = sr != null ? sr.bounds.size.y / 2f : 0.5f;

        
        transform.position = pos;
    }
}
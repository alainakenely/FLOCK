using UnityEngine;

public class FollowerFlightKeyboard : MonoBehaviour
{
    public Transform leader;
    public Canvas flightCanvas;
    public float parallaxSpeed = 3f;
    public float verticalSpeed = 3f;
    public float horizontalSpeedMultiplier = 0.3f;

    [HideInInspector] public Vector3 formationOffset = Vector3.zero;
    private bool offsetApplied = false;

    private FlockManager flockManager;

    void Start()
    {
        flockManager = FindFirstObjectByType<FlockManager>();
    }

    void Update()
    {
        if (flightCanvas == null) return;

        Vector3 pos = transform.position;

        if (!offsetApplied)
        {
            pos += formationOffset;
            offsetApplied = true;
        }

        // --- Get input, potentially blocked by any flock bird at a boundary ---
        Vector2 allowedInput = GetAllowedInput();

        // Apply movement
        pos.x += parallaxSpeed * Time.deltaTime + allowedInput.x * parallaxSpeed * horizontalSpeedMultiplier * Time.deltaTime;
        pos.y += allowedInput.y * verticalSpeed * Time.deltaTime;

        // Apply formation offset relative to leader
        if (leader != null)
            pos += formationOffset;

        // Clamp Z
        pos.z = 0f;

        transform.position = pos;

        // Face right
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    Vector2 GetAllowedInput()
    {
        float verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
        float horizontalInput = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;

        if (flockManager == null || flockManager.flockBirds.Count == 0) 
            return new Vector2(horizontalInput, verticalInput);

        Camera cam = flightCanvas.worldCamera != null ? flightCanvas.worldCamera : Camera.main;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));

        foreach (var bird in flockManager.flockBirds)
        {
            SpriteRenderer sr = bird.GetComponent<SpriteRenderer>();
            float halfHeight = sr != null ? sr.bounds.extents.y : 0.5f;
            float halfWidth = sr != null ? sr.bounds.extents.x : 0.5f;

            float birdTop = bird.transform.position.y + halfHeight;
            float birdBottom = bird.transform.position.y - halfHeight;
            float birdRight = bird.transform.position.x + halfWidth;
            float birdLeft = bird.transform.position.x - halfWidth;

            // If any bird hits top, disable W
            if (birdTop >= topRight.y && verticalInput > 0) verticalInput = 0f;
            // If any bird hits bottom, disable S
            if (birdBottom <= bottomLeft.y && verticalInput < 0) verticalInput = 0f;
            // Optional horizontal clamping
            if (birdRight >= topRight.x && horizontalInput > 0) horizontalInput = 0f;
            if (birdLeft <= bottomLeft.x && horizontalInput < 0) horizontalInput = 0f;
        }

        return new Vector2(horizontalInput, verticalInput);
    }
}
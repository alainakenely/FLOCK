using UnityEngine;

public class FollowerFlight : MonoBehaviour
{
    [HideInInspector] public Transform mainBird;
    [HideInInspector] public Vector3 formationOffset;
    [HideInInspector] public float parallaxSpeed = 3f;
    [HideInInspector] public float verticalSpeed = 3f;
    [HideInInspector] public float horizontalInputMultiplier = 0.3f;
    [HideInInspector] public Canvas flightCanvas;

    void Update()
    {
        if (mainBird == null || flightCanvas == null) return;

        RectTransform canvasRect = flightCanvas.GetComponent<RectTransform>();
        Camera cam = flightCanvas.worldCamera;

        Vector3 pos = mainBird.position + formationOffset;

        // Apply keyboard input delta exactly like the main bird
        float verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
        float horizontalInput = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;

        pos.x += horizontalInput * parallaxSpeed * horizontalInputMultiplier * Time.deltaTime;
        pos.y += verticalInput * verticalSpeed * Time.deltaTime;

        // Clamp inside canvas
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float halfWidth = sr != null ? sr.bounds.size.x / 2f : 0.5f;
        float halfHeight = sr != null ? sr.bounds.size.y / 2f : 0.5f;

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y, cam.nearClipPlane));

        pos.x = Mathf.Clamp(pos.x, bottomLeft.x + halfWidth, topRight.x - halfWidth);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y + halfHeight, topRight.y - halfHeight);
        pos.z = 0f;

        transform.position = pos;

        // MAKES BIRD FACE RIGHT DO NOT DELETE
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
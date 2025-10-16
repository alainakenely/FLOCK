using UnityEngine;

using UnityEngine;

public class FollowerFlightKeyboard : MonoBehaviour
{
    public Transform leader;              // <--- add this
    public Canvas flightCanvas;
    public float parallaxSpeed = 3f;
    public float verticalSpeed = 3f;
    public float horizontalSpeedMultiplier = 0.3f;

    [HideInInspector] public Vector3 formationOffset = Vector3.zero;
    private bool offsetApplied = false;

    void Update()
    {
        if (flightCanvas == null) return;

        Vector3 pos = transform.position;

        if (!offsetApplied)
        {
            pos += formationOffset;
            offsetApplied = true;
        }

        float verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
        float horizontalInput = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;

        pos.x += parallaxSpeed * Time.deltaTime + horizontalInput * parallaxSpeed * horizontalSpeedMultiplier * Time.deltaTime;
        pos.y += verticalInput * verticalSpeed * Time.deltaTime;

        if (leader != null)
        {
            pos += formationOffset; // optional, keeps V-formation relative to leader
        }

        Camera cam = flightCanvas.worldCamera != null ? flightCanvas.worldCamera : Camera.main;
        Vector3 viewport = cam.WorldToViewportPoint(pos);
        viewport.x = Mathf.Clamp01(viewport.x);
        viewport.y = Mathf.Clamp01(viewport.y);
        pos = cam.ViewportToWorldPoint(viewport);
        pos.z = 0f;

        transform.position = pos;

        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}

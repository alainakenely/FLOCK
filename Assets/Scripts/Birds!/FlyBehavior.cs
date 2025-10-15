using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float verticalSpeed = 3f;
    public float horizontalSpeed = 2f;

    [HideInInspector] public Transform playerLeader;  // The player bird to follow
    [HideInInspector] public Vector3 formationOffset; // Relative V-formation offset
    [HideInInspector] public bool isLeader = false;  // True if this is the main player bird
    [HideInInspector] public Canvas flightCanvas;    // Assign your moving canvas

    void Update()
    {
        if (flightCanvas == null) return;

        Vector3 pos = transform.position;

        // Leader movement
        if (isLeader)
        {
            Vector3 move = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) move += Vector3.up;
            if (Input.GetKey(KeyCode.S)) move += Vector3.down;
            if (Input.GetKey(KeyCode.D)) move += Vector3.right;
            if (Input.GetKey(KeyCode.A)) move += Vector3.left;

            pos += new Vector3(move.x * horizontalSpeed, move.y * verticalSpeed, 0f) * Time.deltaTime;
        }
        // Follower movement
        else if (playerLeader != null)
        {
            // Snap/follow leader using formation offset
            pos = playerLeader.position + formationOffset;
        }

        // Clamp to canvas bounds (world space)
        RectTransform canvasRect = flightCanvas.GetComponent<RectTransform>();
        Camera cam = flightCanvas.worldCamera;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y, cam.nearClipPlane));

        pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y, topRight.y);
        pos.z = 0f;

        transform.position = pos;
    }
}
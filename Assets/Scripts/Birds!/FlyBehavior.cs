using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float verticalSpeed = 3f;
    public float horizontalSpeed = 2f;

    [Header("World Bounds")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    void Update()
    {
        // Get movement input
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += Vector3.up;
        if (Input.GetKey(KeyCode.S)) move += Vector3.down;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;

        // Move the bird
        transform.position += new Vector3(move.x * horizontalSpeed, move.y * verticalSpeed, 0f) * Time.deltaTime;

        // Clamp position within world bounds
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        clampedPos.z = 0f;

        transform.position = clampedPos;
    }
}
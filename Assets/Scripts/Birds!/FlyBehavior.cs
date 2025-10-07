using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float verticalSpeed = 3f;
    public float horizontalSpeed = 2f;

    private RectTransform canvasRect;

    void Start()
    {
        // Automatically find the Canvas in the parent hierarchy
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRect = parentCanvas.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("FlyBehavior: Could not find parent Canvas!");
        }
    }

    void Update()
    {
        if (canvasRect == null) return;

        // Get movement input
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += Vector3.up;
        if (Input.GetKey(KeyCode.S)) move += Vector3.down;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;

        // Move the bird
        transform.position += new Vector3(move.x * horizontalSpeed, move.y * verticalSpeed, 0f) * Time.deltaTime;

        // Clamp position within canvas bounds
        Vector3 clampedPos = transform.localPosition;
        Vector2 canvasSize = canvasRect.sizeDelta;

        clampedPos.x = Mathf.Clamp(clampedPos.x, -canvasSize.x / 2f, canvasSize.x / 2f);
        clampedPos.y = Mathf.Clamp(clampedPos.y, -canvasSize.y / 2f, canvasSize.y / 2f);
        clampedPos.z = 0f;

        transform.localPosition = clampedPos;
    }
}
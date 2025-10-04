using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float verticalSpeed = 3f;  // Speed of moving up/down
    public float forwardSpeed = 2f;    // Forward movement speed when pressing D
    
    private Camera mainCamera;
    private float camHeight;
    private float camWidth;

    void Start()
    {
        mainCamera = Camera.main;
        camHeight = mainCamera.orthographicSize; // half-height of camera
        camWidth = camHeight * mainCamera.aspect; // half-width of camera
    }


    void Update()
    {

        // Vertical movement (W = up, S = down)
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f; // move up
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f; // move down
        }

        // Apply vertical movement
        transform.Translate(Vector3.up * verticalInput * verticalSpeed * Time.deltaTime);
        
        // Forward movement when pressing D
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * forwardSpeed * Time.deltaTime);
        }
        
        // Forward movement when pressing D
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * forwardSpeed * Time.deltaTime);
        }
        // --- Clamp position within camera view ---
        Vector3 pos = transform.position;
        Vector3 camPos = mainCamera.transform.position;

        float minX = camPos.x - camWidth;
        float maxX = camPos.x + camWidth;
        float minY = camPos.y - camHeight;
        float maxY = camPos.y + camHeight;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;


    }
}
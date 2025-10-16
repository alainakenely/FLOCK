// using UnityEngine;
//
// public class FlyBehavior : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     public float verticalSpeed = 3f;
//     public float horizontalSpeed = 2f; // player horizontal input
//     public Canvas flightCanvas;
//
//     [HideInInspector] public bool isPlayer = false; // true for main bird
//     [HideInInspector] public Transform mainBird;    // reference for followers
//     [HideInInspector] public Vector3 formationOffset;
//
//     void Update()
//     {
//         if (flightCanvas == null) return;
//
//         Vector3 pos = transform.position;
//
//         if (isPlayer)
//         {
//             // Player input
//             float verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
//             float horizontalInput = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
//
//             pos.x += horizontalInput * horizontalSpeed * Time.deltaTime;
//             pos.y += verticalInput * verticalSpeed * Time.deltaTime;
//         }
//         else if (mainBird != null)
//         {
//             // Followers: snap to main bird + formation offset
//             pos = mainBird.position + formationOffset;
//
//             // Optional: respond to vertical input to move flock up/down together
//             float verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
//             pos.y += verticalInput * verticalSpeed * Time.deltaTime;
//         }
//
//         // Clamp to canvas bounds
//         Camera cam = flightCanvas.worldCamera;
//         RectTransform canvasRect = flightCanvas.GetComponent<RectTransform>();
//         Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
//         Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y, cam.nearClipPlane));
//
//         SpriteRenderer sr = GetComponent<SpriteRenderer>();
//         float halfWidth = sr != null ? sr.bounds.size.x / 2f : 0.5f;
//         float halfHeight = sr != null ? sr.bounds.size.y / 2f : 0.5f;
//
//         pos.x = Mathf.Clamp(pos.x, bottomLeft.x + halfWidth, topRight.x - halfWidth);
//         pos.y = Mathf.Clamp(pos.y, bottomLeft.y + halfHeight, topRight.y - halfHeight);
//         pos.z = 0f;
//
//         transform.position = pos;
//
//         // MAKES BIRD FACE RIGHT DO NOT DELETE
//         Vector3 scale = transform.localScale;
//         scale.x = -Mathf.Abs(scale.x);
//         transform.localScale = scale;
//     }
// }
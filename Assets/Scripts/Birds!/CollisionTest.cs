using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    // Make sure this is attached to both player birds and random birds
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bird"))
        {
            Debug.Log($"Collision detected between {gameObject.name} and {other.name}");
        }
    }
}
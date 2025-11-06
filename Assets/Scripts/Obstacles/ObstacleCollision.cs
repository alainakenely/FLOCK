using System.Collections;   // Needed for IEnumerator
using UnityEngine;

// ------------------------
// Collision behavior with color flash
// ------------------------
public class ObstacleCollisionTest : MonoBehaviour
{
    [Header("Collision Settings")] 
    public Color hitColor = Color.red; // Color to flash
    public float flashDuration = 0.5f; // How long to flash
    public DeathPanel deathPanel; // Assign in Inspector

    void Start()
    {
        if (TryGetComponent<Collider2D>(out var c))
            Debug.Log("✅ Collider2D active | IsTrigger: " + c.isTrigger + " | Enabled: " + c.enabled);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerBird")) return;

        // Pause and show death panel
        if (deathPanel != null)
            deathPanel.ActivatePanel();

        // Flash player bird color
        SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
        if (sr != null)
            StartCoroutine(FlashColor(sr));

        Debug.Log("💥 PlayerBird collided with obstacle — game paused!");
    }

    private IEnumerator FlashColor(SpriteRenderer sr)
    {
        Color originalColor = sr.color;
        sr.color = hitColor;
        yield return new WaitForSecondsRealtime(flashDuration); // unaffected by Time.timeScale
        sr.color = originalColor;
    }
}
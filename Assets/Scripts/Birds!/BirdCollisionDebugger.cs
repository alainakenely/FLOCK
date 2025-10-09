using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private AddBirdPanel addBirdPanel;

    void Start()
    {
        // Find the panel in the scene, including inactive objects
        GameObject panelGO = null;
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.name == "Panel-AddBird")
            {
                panelGO = go;
                break;
            }
        }

        if (panelGO != null)
        {
            addBirdPanel = panelGO.GetComponent<AddBirdPanel>();
            if (addBirdPanel == null)
                Debug.LogWarning("⚠️ AddBirdPanel script not found on Panel-AddBird!");
        }
        else
        {
            Debug.LogWarning("⚠️ Panel-AddBird GameObject not found in scene!");
        }

        // Optional: log components
        if (TryGetComponent<Collider2D>(out var c))
            Debug.Log("✅ Collider2D active | IsTrigger: " + c.isTrigger + " | Enabled: " + c.enabled);
        if (TryGetComponent<Rigidbody2D>(out var rb))
            Debug.Log("✅ Rigidbody2D active | Type: " + rb.bodyType + " | Simulated: " + rb.simulated);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // PlayerBird collides with random Bird
        if (CompareTag("PlayerBird") && other.CompareTag("Bird"))
        {
            Debug.Log("🐦 PlayerBird collided with a Bird!");

            if (addBirdPanel != null)
                addBirdPanel.ActivatePanel();
            else
                Debug.LogWarning("⚠️ AddBirdPanel reference missing!");
        }
    }
}
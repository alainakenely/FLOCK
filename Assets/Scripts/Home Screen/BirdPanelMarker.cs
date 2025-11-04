using UnityEngine;

public class BirdPanelMarker : MonoBehaviour
{
    [Tooltip("Assign the bird prefab this panel corresponds to.")]
    public GameObject prefabReference; // reference to original prefab

    private void Start()
    {
        RefreshPanel(); // check unlock status on load
    }

    public void RefreshPanel()
    {
        if (prefabReference == null) return;

        bool unlocked = RuntimeBirdProgress.IsUnlocked(prefabReference.name);
        gameObject.SetActive(unlocked);

        if (unlocked)
            Debug.Log($"✅ Panel activated: {prefabReference.name}");
    }
}
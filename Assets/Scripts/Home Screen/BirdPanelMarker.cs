using UnityEngine;

public class BirdPanelMarker : MonoBehaviour
{
    [Tooltip("Assign the bird prefab this panel corresponds to.")]
    public GameObject prefabReference; // reference to original prefab

    private void Start()
    {
        // Start inactive
        gameObject.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnHome : MonoBehaviour
{
    [Header("Settings")]
    public string homeSceneName = "Home Screen";
    public float returnDelay = 10f;

    private void Start()
    {
        // Schedule return to Home Screen
        Invoke(nameof(ReturnToHome), returnDelay);
    }

    public void ReturnToHome()
    {
        // Simply load the Home Screen scene
        SceneManager.LoadScene(homeSceneName);
    }
}
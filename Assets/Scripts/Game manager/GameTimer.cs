using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToHome : MonoBehaviour
{
    [Tooltip("Name of the home screen scene to load after the timer.")]
    public string homeSceneName = "HomeScreen";

    [Tooltip("How many seconds before returning to the home scene.")]
    public float returnDelay = 15f;

    private void Start()
    {
        // Start countdown when scene begins
        Invoke(nameof(ReturnHome), returnDelay);
    }

    private void ReturnHome()
    {
        // Optionally stop time or clean up before switching
        Time.timeScale = 1f; 
        SceneManager.LoadScene(homeSceneName);
    }
}
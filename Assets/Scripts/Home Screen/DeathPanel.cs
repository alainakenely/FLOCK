using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false); // Start hidden
    }

    public void ActivatePanel()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Debug.Log("💀 Death panel activated, game paused!");
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume game
        Debug.Log("▶️ Game resumed!");
    }

    // --------------------------
    // Button Methods
    // --------------------------

    // Called by "Try Again" button
    public void RestartScene()
    {
        Time.timeScale = 1f; // Unpause before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called by "Main Menu" button
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Unpause before switching scenes
        SceneManager.LoadScene("Home Screen"); // Make sure your Home Screen scene is named exactly this
    }
}
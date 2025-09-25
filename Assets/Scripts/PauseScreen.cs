using UnityEngine;
using UnityEngine.SceneManagement; // for restarting / quitting to menu

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;          // Main pause menu panel
    public GameObject quitConfirmationUI;   // Confirmation panel

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        quitConfirmationUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        quitConfirmationUI.SetActive(false); // ensure it’s hidden at first
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ShowQuitConfirmation()
    {
        quitConfirmationUI.SetActive(true);
    }

    public void CancelQuit()
    {
        quitConfirmationUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();

        // If testing in editor:
        // UnityEditor.EditorApplication.isPlaying = false;
    }
}
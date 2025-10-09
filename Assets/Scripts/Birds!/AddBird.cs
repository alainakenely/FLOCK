using UnityEngine;

public class AddBirdPanel : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false); // start hidden
    }

    public void ActivatePanel()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("🟢 Panel activated and game paused!");
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("▶️ Game resumed!");
    }
}
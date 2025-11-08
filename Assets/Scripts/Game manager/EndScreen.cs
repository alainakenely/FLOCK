using System.Collections;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [Header("Assign your End Screen UI panel here")]
    public GameObject endScreenPanel;

    [Header("Reference to the ParallaxPauseTrigger script")]
    public ParallaxPauseTrigger parallaxTrigger;

    [Tooltip("Time after parallax stops before showing end screen")]
    public float delayAfterParallaxStops = 5f;

    private bool hasShownEndScreen = false;

    private void Update()
    {
        // Wait for parallax to stop moving (Camera_Move = false)
        if (!hasShownEndScreen && parallaxTrigger != null && parallaxTrigger.parallaxScript != null)
        {
            if (parallaxTrigger.parallaxScript.Camera_Move == false)
            {
                hasShownEndScreen = true;
                StartCoroutine(ShowEndScreenAfterDelay());
            }
        }
    }

    private IEnumerator ShowEndScreenAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterParallaxStops);

        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("🏁 End screen shown and game paused.");
        }
        else
        {
            Debug.LogWarning("⚠️ End screen panel not assigned in EndScreen script!");
        }
    }
}
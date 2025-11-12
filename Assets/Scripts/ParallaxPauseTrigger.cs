using System.Collections;
using UnityEngine;

public class ParallaxPauseTrigger : MonoBehaviour
{
    public static bool IsParallaxActive = true;
    public static ParallaxPauseTrigger Instance;

    [Tooltip("Reference your ParallaxBackground_0 script here.")]
    public ParallaxBackground_0 parallaxScript;

    [Tooltip("Reference your ObstacleSpawner here.")]
    public ObstacleSpawner obstacleSpawner;

    [Tooltip("Delay in seconds before pausing the parallax.")]
    public float delayBeforePause = 3f;

    private void Awake()
    {
        Instance = this;
        if (Instance != null && Instance != this) Destroy(gameObject);
    }

    public void TriggerPause()
    {
        StopAllCoroutines();
        StartCoroutine(PauseAfterDelay());
    }

    private IEnumerator PauseAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforePause);

        if (parallaxScript != null)
        {
            // Stop camera/parallax movement
            parallaxScript.Camera_Move = false;
            IsParallaxActive = false; // ✅ Stop player input globally

            // Stop obstacle spawning
            if (obstacleSpawner != null)
            {
                obstacleSpawner.StopSpawning();
            }

            Debug.Log("⏸️ Parallax movement paused and obstacle spawning stopped.");
        }
        else
        {
            Debug.LogWarning("⚠️ ParallaxBackground_0 reference missing in ParallaxPauseTrigger!");
        }
    }
}
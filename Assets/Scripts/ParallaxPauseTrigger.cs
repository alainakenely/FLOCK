using System.Collections;
using UnityEngine;

public class ParallaxPauseTrigger : MonoBehaviour
{
    public static ParallaxPauseTrigger Instance;

    [Tooltip("Reference your ParallaxBackground_0 script here.")]
    public ParallaxBackground_0 parallaxScript;

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
            // Option 1: stop movement but keep parallax active
            parallaxScript.Camera_Move = false;

            // Option 2: completely freeze parallax script
            // parallaxScript.enabled = false;

            Debug.Log("⏸️ Parallax movement paused after unlocking both birds.");
        }
        else
        {
            Debug.LogWarning("⚠️ ParallaxBackground_0 reference missing in ParallaxPauseTrigger!");
        }
    }
}
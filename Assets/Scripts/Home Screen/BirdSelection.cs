using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BirdSelectionController : MonoBehaviour
{
    [Header("UI References")]
    public Button startButton;          
    public Button[] birdButtons;        
    public RectTransform selectionArea; 
    public float spacing = 200f;        

    private List<GameObject> selectedBirdPrefabs = new List<GameObject>();
    private List<Button> selectedBirdButtons = new List<Button>();
    private Dictionary<Button, Vector2> originalPositions = new Dictionary<Button, Vector2>();

    void Start()
    {
        startButton.interactable = false;

        startButton.onClick.AddListener(StartGame);

        foreach (Button btn in birdButtons)
        {
            originalPositions[btn] = btn.GetComponent<RectTransform>().anchoredPosition;
            Button capturedBtn = btn; 
            capturedBtn.onClick.AddListener(() => ToggleBirdSelection(capturedBtn));
        }
    }

    void ToggleBirdSelection(Button birdButton)
    {
        RectTransform birdRect = birdButton.GetComponent<RectTransform>();
        BirdButton birdData = birdButton.GetComponent<BirdButton>();

        if (selectedBirdButtons.Contains(birdButton))
        {
            int index = selectedBirdButtons.IndexOf(birdButton);
            selectedBirdButtons.Remove(birdButton);
            selectedBirdPrefabs.RemoveAt(index);

            birdRect.SetParent(selectionArea.parent);
            birdRect.anchoredPosition = originalPositions[birdButton];
        }
        else
        {
            selectedBirdButtons.Add(birdButton);
            selectedBirdPrefabs.Add(birdData.birdPrefab);

            birdRect.SetParent(selectionArea);
        }

        // Reposition
        for (int i = 0; i < selectedBirdButtons.Count; i++)
        {
            RectTransform rect = selectedBirdButtons[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(i * spacing, 0f);
        }

        startButton.interactable = selectedBirdButtons.Count > 0;
    }

    public void StartGame()
    {
        // 🔄 Reset runtime unlocks BEFORE loading the new scene
        RuntimeBirdProgress.Reset();

        // Reset static flags from previous scene
        ParallaxPauseTrigger.IsParallaxActive = true;

        // Optional: reset any spawners or timers that might persist
        RandomBirdSpawner[] spawners = FindObjectsOfType<RandomBirdSpawner>();
        foreach (var spawner in spawners)
        {
            spawner.enabled = true;
        }

        ObstacleSpawner[] obstacles = FindObjectsOfType<ObstacleSpawner>();
        foreach (var obs in obstacles)
        {
            obs.SetPaused(false);
        }

        // Ensure Time.timeScale is normal
        Time.timeScale = 1f;

        // Save currently selected prefabs
        BirdSelectionData.selectedBirds = new List<GameObject>(selectedBirdPrefabs);
        Debug.Log("Saved prefabs count: " + BirdSelectionData.selectedBirds.Count);

        // Load Snow if more than 1 bird selected
        if (BirdSelectionData.selectedBirds.Count > 1)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Snow");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("Mountains");
    }
}

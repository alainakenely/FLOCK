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

    // store both buttons + prefabs
    private List<GameObject> selectedBirdPrefabs = new List<GameObject>();
    private List<Button> selectedBirdButtons = new List<Button>();
    private Dictionary<Button, Vector2> originalPositions = new Dictionary<Button, Vector2>();

    void Start()
    {
        startButton.interactable = false;

        // Assign the Start button click event
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
        Debug.Log("Selected prefabs count: " + selectedBirdPrefabs.Count);
        Debug.Log("Selecting: " + birdButton.name + " prefab: " + birdData.birdPrefab.name);
        

        if (selectedBirdButtons.Contains(birdButton))
        {
            // remove from selection
            int index = selectedBirdButtons.IndexOf(birdButton);
            selectedBirdButtons.Remove(birdButton);
            selectedBirdPrefabs.RemoveAt(index);

            birdRect.SetParent(selectionArea.parent);
            birdRect.anchoredPosition = originalPositions[birdButton];
        }
        else
        {
            // add to selection
            selectedBirdButtons.Add(birdButton);
            selectedBirdPrefabs.Add(birdData.birdPrefab);

            birdRect.SetParent(selectionArea);
        }

        // reposition selected buttons in row
        for (int i = 0; i < selectedBirdButtons.Count; i++)
        {
            RectTransform rect = selectedBirdButtons[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(i * spacing, 0f);
        }

        startButton.interactable = selectedBirdButtons.Count > 0;
    }

    // Called when Start button is pressed
    public void StartGame()
    {
        // Save currently selected prefabs to the static holder
        BirdSelectionData.selectedBirds = new List<GameObject>(selectedBirdPrefabs);
        Debug.Log("Saved prefabs count: " + BirdSelectionData.selectedBirds.Count);

        // Load game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Mountains");
    }

    public class CollectibleBird : MonoBehaviour
    {
        public string birdId; // match BirdButton's ID

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                BirdUnlockData.UnlockBird(birdId);
                Destroy(gameObject);
            }
        }
    }
}
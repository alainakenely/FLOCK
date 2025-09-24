using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BirdSelectionController : MonoBehaviour
{
    [Header("UI References")]
    public Button startButton;        // Start Game button
    public Button[] birdButtons;      // All bird buttons
    public RectTransform selectionArea; // Area where selected birds line up
    public float spacing = 100f;      // Horizontal spacing between selected birds

    private List<Button> selectedBirds = new List<Button>();
    private Dictionary<Button, Vector3> originalPositions = new Dictionary<Button, Vector3>();

    void Start()
    {
        // Disable Start button initially
        startButton.interactable = false;

        // Store original positions and hook up click events
        foreach (Button btn in birdButtons)
        {
            originalPositions[btn] = btn.GetComponent<RectTransform>().position;

            Button capturedBtn = btn; // capture for closure
            capturedBtn.onClick.AddListener(() => ToggleBirdSelection(capturedBtn));
        }
    }

    void ToggleBirdSelection(Button birdButton)
    {
        RectTransform birdRect = birdButton.GetComponent<RectTransform>();

        if (selectedBirds.Contains(birdButton))
        {
            // Deselect: remove from selection row
            selectedBirds.Remove(birdButton);
            birdRect.position = originalPositions[birdButton];
            Debug.Log($"Deselected bird: {birdButton.name}");
        }
        else
        {
            // Select: add to selection row
            selectedBirds.Add(birdButton);
            Debug.Log($"Selected bird: {birdButton.name}");
        }

        // Reposition all selected birds in order
        for (int i = 0; i < selectedBirds.Count; i++)
        {
            selectedBirds[i].GetComponent<RectTransform>().position =
                selectionArea.position + new Vector3(i * spacing, 0f, 0f);
        }

        // Enable/disable Start button depending on selection
        startButton.interactable = selectedBirds.Count > 0;
    }
}
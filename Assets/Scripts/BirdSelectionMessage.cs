using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BirdSelectionMessage : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI messageText;   // The TMP text object showing the message
    public Button[] birdButtons;          // All bird selection buttons

    private int totalBirds;
    private int selectedBirds = 0;

    void Start()
    {
        // Count all buttons
        totalBirds = birdButtons.Length;
        Debug.Log("Total bird buttons found: " + totalBirds);

        // Hook each button's click event
        foreach (Button btn in birdButtons)
        {
            Button capturedBtn = btn; // avoid closure issue
            capturedBtn.onClick.AddListener(() => OnBirdSelected(capturedBtn));
            Debug.Log("Hooked up button: " + capturedBtn.name);
        }

        // Show initial message
        UpdateMessage();
    }
    
    void OnBirdSelected(Button button)
    {
        Debug.Log("Button clicked: " + button.name);

        // Increment selected count
        selectedBirds++;
        Debug.Log("Selected birds count is now: " + selectedBirds);

        // Disable the button so it can’t be clicked again
        button.interactable = false;
        Debug.Log(button.name + " is now disabled.");

        // Update the message
        UpdateMessage();
    }

    void UpdateMessage()
    {
        int remaining = totalBirds - selectedBirds;
        Debug.Log("Remaining birds to choose: " + remaining);

        if (remaining > 0)
        {
            messageText.text = $"Choose ({remaining}) more bird{(remaining > 1 ? "s" : "")}";
            messageText.gameObject.SetActive(true); // ensure it’s visible
            Debug.Log("Message updated: " + messageText.text);
        }
        else
        {
            // All birds chosen, hide the message
            messageText.text = "";
            messageText.gameObject.SetActive(false);
            Debug.Log("All birds selected. Message hidden.");
        }
    }
}
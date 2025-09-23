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

        // Hook each button's click event
        foreach (Button btn in birdButtons)
        {
            btn.onClick.AddListener(() => OnBirdSelected(btn));
        }

        // Show initial message
        UpdateMessage();
    }
    
    void OnBirdSelected(Button button)
    {
        // Increment selected count
        selectedBirds++;

        // Disable the button so it can’t be clicked again
        button.interactable = false;

        // Update the message
        UpdateMessage();
        
        Debug.Log("Clicked: " + button.name); // <- This line
        selectedBirds++;
        button.interactable = false;
        UpdateMessage();
    }

    void UpdateMessage()
    {
        int remaining = totalBirds - selectedBirds;

        if (remaining > 0)
        {
            messageText.text = $"Choose ({remaining}) more bird{(remaining > 1 ? "s" : "")}";
            messageText.gameObject.SetActive(true); // ensure it’s visible
        }
        else
        {
            // All birds chosen, hide the message
            messageText.text = "";
            messageText.gameObject.SetActive(false);
        }
    }
}
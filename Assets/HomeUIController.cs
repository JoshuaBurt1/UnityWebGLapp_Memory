using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import the TextMeshPro namespace

public class HomeUIController : MonoBehaviour
{
    // UI References for InputField, Button, and Text elements
    public TMP_InputField playerNameInput;  // TMP_InputField for the player name input
    public Button startButton;            // Button to start the game
    public TextMeshProUGUI gameTitle;  // TextMeshProUGUI for name display

    private void Start()
    {
        // Load saved name from PlayerPrefs, if available
        string savedName = PlayerPrefs.GetString("playerName", "");
        if (!string.IsNullOrEmpty(savedName))
        {
            playerNameInput.text = savedName;
        }

        // Add listener for the start button click
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    // Handle start button click
    private void OnStartButtonClicked()
    {
        if (playerNameInput != null && startButton != null)
        {
            string playerNameSet = playerNameInput.text.Trim();

            // Save the player's name to PlayerPrefs
            PlayerPrefs.SetString("playerName", playerNameSet);
            //Debug.Log("Saved Name: " + playerNameSet); // Debug line

            // Update the name in the "navigation" text element
            //nameInNavigation.text = newName;

            //string playerNameGet = PlayerPrefs.GetString("playerName", "Default Name");
            //Debug.Log("Retrieved Name, Home scene: " + playerNameGet); // Debug line

            // Load the Game Scene (ensure the scene is added in Build Settings)
            SceneManager.LoadScene("GameScene");

            // Optionally, hide the virtual keyboard on mobile devices
            TouchScreenKeyboard.hideInput = true;
        }
        else
        {
            Debug.LogError("One of the UI elements is not properly initialized!");
        }
    }
}

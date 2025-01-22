using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class HighscoreController : MonoBehaviour
{
    // UI References for displaying high scores
    public TextMeshProUGUI highscoreTitle;  // TextMeshProUGUI for title
    public TextMeshProUGUI highScore1;  // TextMeshProUGUI for highest score
    public TextMeshProUGUI highScore2;  // TextMeshProUGUI for second-highest score
    public TextMeshProUGUI highScore3;  // TextMeshProUGUI for third-highest score

    private void Start()
    {
        // Retrieve the player's name or score (using PlayerPrefs or a predefined method)
        string savedName = PlayerPrefs.GetString("playerName", "No Name");
        int savedScore = PlayerPrefs.GetInt("playerScore", 0);

        // Retrieve existing high scores from PlayerPrefs, or set default values if not set
        int highScore1Value = PlayerPrefs.GetInt("highScore1", 0);
        int highScore2Value = PlayerPrefs.GetInt("highScore2", 0);
        int highScore3Value = PlayerPrefs.GetInt("highScore3", 0);

        string highScore1Name = PlayerPrefs.GetString("highScore1Name", "Player1");
        string highScore2Name = PlayerPrefs.GetString("highScore2Name", "Player2");
        string highScore3Name = PlayerPrefs.GetString("highScore3Name", "Player3");

        // Check if the saved score is greater than the first high score
        if (savedScore > highScore1Value)
        {
            // Shift the scores down
            highScore3Value = highScore2Value;
            highScore2Value = highScore1Value;
            highScore1Value = savedScore;

            // Shift names down as well
            highScore3Name = highScore2Name;
            highScore2Name = highScore1Name;
            highScore1Name = savedName;

            // Save the new high scores and names
            PlayerPrefs.SetInt("highScore1", highScore1Value);
            PlayerPrefs.SetString("highScore1Name", highScore1Name);

            PlayerPrefs.SetInt("highScore2", highScore2Value);
            PlayerPrefs.SetString("highScore2Name", highScore2Name);

            PlayerPrefs.SetInt("highScore3", highScore3Value);
            PlayerPrefs.SetString("highScore3Name", highScore3Name);
        }
        // Check if the saved score is greater than the second high score
        else if (savedScore > highScore2Value)
        {
            // Shift the second and third scores down
            highScore3Value = highScore2Value;
            highScore2Value = savedScore;

            // Shift names down as well
            highScore3Name = highScore2Name;
            highScore2Name = savedName;

            // Save the new high scores and names
            PlayerPrefs.SetInt("highScore2", highScore2Value);
            PlayerPrefs.SetString("highScore2Name", highScore2Name);

            PlayerPrefs.SetInt("highScore3", highScore3Value);
            PlayerPrefs.SetString("highScore3Name", highScore3Name);
        }
        // Check if the saved score is greater than the third high score
        else if (savedScore > highScore3Value)
        {
            // Update the third high score and name
            highScore3Value = savedScore;
            highScore3Name = savedName;

            // Save the new third high score and name
            PlayerPrefs.SetInt("highScore3", highScore3Value);
            PlayerPrefs.SetString("highScore3Name", highScore3Name);
        }

        // Update UI to display the high scores
        highScore1.text = "1. " + highScore1Name + " : " + highScore1Value;
        highScore2.text = "2. " + highScore2Name + " : " + highScore2Value;
        highScore3.text = "3. " + highScore3Name + " : " + highScore3Value;

        // Save all changes
        PlayerPrefs.Save();

        // Delete the player score and name after use, so they won't be saved twice
        PlayerPrefs.DeleteKey("playerScore");
    }
}

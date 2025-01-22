using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour
{
    // Reference to the buttons
    public Button homeButton;
    public Button gameButton;
    public Button highscoreButton;

    private static NavigationController instance;

    void Start()
    {
        // Set up button listeners
        homeButton.onClick.AddListener(() => LoadScene("HomeScene"));
        gameButton.onClick.AddListener(() => LoadScene("GameScene"));
        highscoreButton.onClick.AddListener(() => LoadScene("HighscoreScene"));
    }

    // Method to load the scene by name
    void LoadScene(string sceneName)
    {
        // Load the scene using SceneManager
        SceneManager.LoadScene(sceneName);
    }

    void Awake()
    {
        // Check if an instance of NavigationController already exists
        if (instance == null)
        {
            // If no instance exists, set this as the instance and don't destroy it on scene load
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one to avoid duplicates
            Destroy(gameObject);
        }
    }
}

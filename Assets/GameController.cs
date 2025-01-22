using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class MemoryGame : MonoBehaviour
{
    public TextMeshProUGUI gameName;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI tilesText;
    public Button[] cells; // Array of tile buttons

    private int tilesCount = 4;
    private int points = 0;
    private int round = 0;
    private int multiplier = 1;
    private List<int> selectedTilesIndices = new List<int>();
    private List<int> playerClicks = new List<int>();
    private float memorizationTime = 3f;
    private float actionTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        string playerName = PlayerPrefs.GetString("playerName", "");
        gameName.text = $"Name: {playerName}";
        UpdateUI();
        StartRound();
    }

    // Start a new round
    void StartRound()
    {
        memorizationTime = 3f;
        actionTime = 5f;
        selectedTilesIndices.Clear();
        playerClicks.Clear();
        StartCoroutine(RunTimersSequentially());
    }

    // Coroutine to run timers sequentially
    IEnumerator RunTimersSequentially()
    {
        yield return StartCoroutine(MemoryTimer());
        yield return StartCoroutine(GridSelectionTimer());
    }

    // Common countdown logic for both MemoryTimer and GridSelectionTimer
    IEnumerator CountdownTimer(float countdownTime, System.Action onComplete)
    {
        while (countdownTime > 0)
        {
            timeText.text = $"Time: {countdownTime:0.0}";
            countdownTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        timeText.text = "Time: 0.0";
        onComplete.Invoke();
    }

    // Countdown 5 to 0 for grid selection
    IEnumerator GridSelectionTimer()
    {
        EnableTileInteractions(true);

        while (actionTime > 0)
        {
            timeText.text = $"Time: {actionTime:0.0}";
            actionTime -= 0.1f;
            CheckPlayerPattern();
            if (IsAnyIncorrectTileClicked())
            {
                GameOver();
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
        GameOver();
    }

    // Check if the player clicked the correct pattern
    void CheckPlayerPattern()
    {
        if (playerClicks.Count == selectedTilesIndices.Count && !selectedTilesIndices.Except(playerClicks).Any())
        {
            if(round % 3==0 && round != 0){
                multiplier+=1;
                tilesCount +=1;
            }
            points += 10*multiplier;
            round +=1;
            UpdateUI();
            ResetAllTilesToBlue();
            StopAllCoroutines();
            StartRound();
        }
    }

    // Countdown 3 to 0 for memory phase
    IEnumerator MemoryTimer()
    {
        ResetAllTilesToBlue();
        EnableTileInteractions(false);

        List<Button> selectedTiles = new List<Button>();
        for (int i = 0; i < tilesCount; i++)
        {
            Button randomTile;
            do
            {
                randomTile = cells[Random.Range(0, cells.Length)];
            } while (selectedTiles.Contains(randomTile));

            selectedTiles.Add(randomTile);
            SetTileColor(randomTile, Color.red);
            selectedTilesIndices.Add(System.Array.IndexOf(cells, randomTile));
        }

        yield return StartCoroutine(CountdownTimer(memorizationTime, () => ResetAllTilesToBlue()));
    }

    // Method to set tile color
    void SetTileColor(Button tile, Color color)
    {
        tile.GetComponent<Image>().color = color;
        var colors = tile.colors;
        colors.disabledColor = color;
        tile.colors = colors;
    }

    // Method to reset all tile colors to blue
    void ResetAllTilesToBlue() => SetAllTilesColor(Color.blue);

    // Method to set all tile colors at once
    void SetAllTilesColor(Color color)
    {
        foreach (Button tile in cells)
        {
            SetTileColor(tile, color);
            tile.interactable = true;
        }
    }

    // Check if any clicked tile is not part of the memorized pattern
    bool IsAnyIncorrectTileClicked() => playerClicks.Any(clickedIndex => !selectedTilesIndices.Contains(clickedIndex));

    // Method to handle tile clicks and change tile to red color
    void OnTileClicked(Button clickedButton)
    {
        SetTileColor(clickedButton, Color.red);
        clickedButton.interactable = false;
        playerClicks.Add(System.Array.IndexOf(cells, clickedButton));
    }

    // Enable or disable tile interactions
    void EnableTileInteractions(bool enable)
    {
        foreach (Button cell in cells)
        {
            if (enable)
            {
                cell.onClick.AddListener(() => OnTileClicked(cell));
                cell.interactable = true;
            }
            else
            {
                cell.onClick.RemoveAllListeners();
                cell.interactable = false;
            }
        }
    }

    // Game Over method
    void GameOver()
    {
        string playerName = PlayerPrefs.GetString("playerName", "").Trim(); 
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetInt("playerScore", points);
        }
        SceneManager.LoadScene("HighscoreScene");
    }


    // Update UI with initial values
    void UpdateUI()
    {
        timeText.text = $"Time: {memorizationTime}";
        tilesText.text = $"Tiles: {tilesCount}";
        scoreText.text = $"Score: {points}";
        roundText.text = $"Round: {round}";

    }
}

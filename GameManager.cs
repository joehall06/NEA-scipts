using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using System;


public class GameManager : MonoBehaviour
{
    // variable which sets the initial speed of the game
    private float initialGameSpeed = 5f;
    // increaes the game speed over time
    private float increaseGameSpeed = 0.0001f;
    // variable to hold the current game speed
    public float gameSpeed;
    // variable to hold starting amount of lives
    public float livesRemaining = 3f;

    // allows the sprite to be controlled
    public GameObject player;

    // control the GUI displayed texts
    public TextMeshProUGUI livesRemainingText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    private float currentScore;

    // declares the starting game number
    private int gameNumber = 0;

    // the displayed text for the current game numner
    public TextMeshProUGUI gameNumberText;
    // 'GameObject' allows the text boxes to be controlled from within the script
    public GameObject gameNumberTextDisplay;
    // determines the time which the text box will be displayed for
    private float gameNumberTimeRemaining = 1.5f;
    // boolean determining whether text boxes are currently active or inactive
    private bool gameNumberTimerActive = false;





    // Start is called before the first frame update
    void Start()
    {
        // calls a new game
        NewGame();

        DisplaySprite();


        highScoreText.text = Mathf.CeilToInt(StaticData.originalHighScore).ToString("D5");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentScore();
        UpdateHighScore();

        CalculateGameSpeed(currentScore);

        GameNumberDisplay();

        DisplaySprite();
    }

    private void UpdateCurrentScore()
    {
        // increases the current score
        currentScore += Time.deltaTime * 4;
        currentScoreText.text = Mathf.CeilToInt(currentScore).ToString("D5");
    }

    private void UpdateHighScore()
    {
        if (!StaticData.dailyChallengeSelected)
        {
            if (currentScore > StaticData.originalHighScore)
            {
                StaticData.originalHighScore = currentScore;
                highScoreText.text = currentScoreText.text;
            }
        }
        if (StaticData.dailyChallengeSelected) {
            highScoreText.text = currentScoreText.text;
        }
    }
    
    // called by 'Update' function
    private void GameNumberDisplay()
    {
        // displays the current game number at the start of each new game
        if (!StaticData.dailyChallengeSelected)
        {
            if (gameNumberTimeRemaining > 0)
            {
                // decreases the time remaining by the time passed
                // not affected by time scale
                gameNumberTimeRemaining -= Time.deltaTime;
                gameNumberText.text = $"Game {gameNumber}";
            }
            else
            {
                // stops displaying the text after the timer runs out
                gameNumberTimerActive = false;
                gameNumberTextDisplay.SetActive(false);
            }
        }
        if (StaticData.dailyChallengeSelected)
        {
            if (gameNumberTimeRemaining > 0)
            {
                // decreases the time remaining by the time passed
                // not affected by time scale
                gameNumberTimeRemaining -= Time.deltaTime;
                gameNumberText.text = "Good Luck!";
            }
            else
            {
                // stops displaying the text after the timer runs out
                gameNumberTimerActive = false;
                gameNumberTextDisplay.SetActive(false);
            }
        }
    }

    // ensures initial game is reset
    public void NewGame()
    {
        // reset variables to starting values
        currentScore = 0f;
        livesRemaining = 3f;
        gameSpeed = initialGameSpeed;
        livesRemainingText.text = livesRemaining.ToString();

        // resets all variables to do with display the current game
        gameNumberTimerActive = true;
        gameNumberTimeRemaining = 1.5f;
        gameNumberTextDisplay.SetActive(true);

        // creates an array of the current obstacles present on the canvas
        // loops through the array and destroys all objects on the canvas
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles) { 
            Destroy(obstacle);
        }
        GameObject[] obstaclesCollections = GameObject.FindGameObjectsWithTag("Obstacle Collection");
        foreach (GameObject collection in obstaclesCollections)
        {
            Destroy(collection);
        }
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("Power Up");
        foreach (GameObject powerUp in powerUps)
        {
            Destroy(powerUp);
        }

        // current game number increments
        gameNumber += 1;
    }

    public void DecreaseLives()
    {

        // decreases the lives remaining
        livesRemaining -= 1;

        // checks if the player has no lives remaining 
        // checks if it is not the 'Daily Challenge' game mode
        if (livesRemaining == 0 && !StaticData.dailyChallengeSelected)
        {
            // new game is loaded
            NewGame();
        }

        // checks if the player has no lives remaining 
        // checks if it is the 'Daily Challenge' game mode
        if (livesRemaining == 0 && StaticData.dailyChallengeSelected) 
        {
            // boolean values updated
            // game paused
            Time.timeScale = 0f;

            StaticData.dailyChallengeScore = (int)currentScore;

            // loads the menu scene
            SceneManager.LoadScene(0);

        }


        // the current amount of lives remaining is updated 
        livesRemainingText.text = livesRemaining.ToString();
    }

    public void DisplayLives()
    {
        // the current amount of lives remaining is updated 
        livesRemainingText.text = livesRemaining.ToString();
    }

    private float CalculateGameSpeed(float currentScore)
    {
        int[] scoreThresholds = { 100, 200, 300, 400, 500, 600, 700, 800, 900 };

        foreach (int threshold in scoreThresholds) 
        {
            if (currentScore > threshold && currentScore < threshold + 50)
            {
                gameSpeed += increaseGameSpeed;
            }
        }
        if (currentScore > 1000)
        {
            gameSpeed += increaseGameSpeed;
        }
        return gameSpeed;
    }

    // stores default sprite
    // stores default colour (dark blue)
    public Sprite defaultSpriteImage;
    public Color deafaultSpriteColor = new Color32(0, 0, 255, 255);

    public void DisplaySprite()
    {
        // gets access to the sprites sprite renderer
        // allows it to be edited
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();


        // checks if static data contains contains a value
        // if not default colour and image is displayed
        if (StaticData.selectedSprite != null)
        {
            spriteRenderer.sprite = StaticData.selectedSprite;
        }
        else
        {
            spriteRenderer.sprite = defaultSpriteImage;
        }

        if (StaticData.selectedColor != default) 
        {
            spriteRenderer.color = StaticData.selectedColor;
        }
        else
        {
            spriteRenderer.color = deafaultSpriteColor;
        }
    }
}
 



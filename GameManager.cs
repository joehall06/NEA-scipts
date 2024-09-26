using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // variable which sets the initial speed of the game
    private float initialGameSpeed = 5f;
    // increaes the game speed over time
    private float increaseGameSpeed = 0.0001f;
    // variable to hold the current game speed
    public float gameSpeed;
    // variable to hold starting amount of lives
    public float livesRemaining;


    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    private float currentScore;


    // Start is called before the first frame update
    void Start()
    {
        // calls a new game
        NewGame();
    }

    void Update()
    {
        currentScore += Time.deltaTime * 4;
        currentScoreText.text = Mathf.CeilToInt(currentScore).ToString("D5");

        CalculateGameSpeed(currentScore);
    }

    // ensures initital speed is stored as game speed when called
    private void NewGame()
    {
        currentScore = 0f;
        livesRemaining = 3f;
        gameSpeed = initialGameSpeed;
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
}




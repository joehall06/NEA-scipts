using UnityEngine;
using TMPro;
using System;
using UnityEngine.AI;

public class PauseResumePlay : MonoBehaviour
{
    // the countdown remaining before the game resumes
    private float resumeTimeRemaining = 3;

    // the duration to display the 'GO!' text after countdown finishes
    private float playResumeTimeRemaining = 1;

    // the displayed text for the number text box
    public TextMeshProUGUI countdownText;

    // 'GameObject' allows the text boxes to be controlled from within the script
    public GameObject countdownTextDisplay;
    public GameObject resumePlayTextDisplay;

    // booleans determining whether text boxes are currently active or inactive
    private bool resumeTimerActive = false;
    private bool resumePlayTimerActive = false;


    // Start is called before the first frame update
    void Start()
    {
        // initialses the countdown text to the starting time
        // Mathf.Ceil rounds the number up to the nearest integer
        // converted to string as it is displayed within a text box
        countdownText.text = Mathf.Ceil(resumeTimeRemaining).ToString();
    }

    // update the countdown timers every second
    void Update()
    {
        if (resumeTimerActive)
        {
            if (resumeTimeRemaining > 0)
            {
                // decreases the time remaining by the time passed
                // not affected by time scale
                resumeTimeRemaining -= Time.unscaledDeltaTime;
                // displays the updated time remaining
                countdownText.text = Mathf.Ceil(resumeTimeRemaining).ToString();
            }
            else
            {
                resumeTimerActive = false;
                ResumeGame();

                // displays the 'GO!' text box
                resumePlayTextDisplay.SetActive(true);
                // activates the timer of the 'Go!' text box
                resumePlayTimerActive = true;
            }
        }

        if (resumePlayTimerActive)
        {
            if (playResumeTimeRemaining > 0)
            {
                // decreases the time remaining by the time passed
                // not affected by time scale
                playResumeTimeRemaining -= Time.unscaledDeltaTime;

            }
            else
            {
                resumePlayTextDisplay.SetActive(false);
                resumePlayTimerActive = false;
            }
        }
    }

    // called externally from Unity
    // starts countdown when called
    public void StartCountdown()
    {
        resumeTimerActive = true;
    }

    // called externally from Unity
    // pauses the game
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    // resumes the game where it previously set off
    public void ResumeGame()
    {
        Time.timeScale = 1;
        countdownTextDisplay.SetActive(false);
    }

    // called externally from Unity
    // resets both timers for next use
    public void ResetTimers()
    {
        resumeTimeRemaining = 3;
        playResumeTimeRemaining = 1;
    }
    
    // called externally from Unity
    // ensures neither timers text is displayed when the menu is in d
    public void CancelTimers()
    {
        resumeTimerActive = false;
        resumePlayTimerActive = false;
    }

    // creating arrays which store game objects
    // can be used over multiple scripts
    public GameObject[] obstacles;
    public GameObject[] obstaclesCollections;
    public GameObject[] powerUps;

    public void DisableGameObjects()
    {
        // stores the different types of specific obstacles in their respective array
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstaclesCollections = GameObject.FindGameObjectsWithTag("Obstacle Collection");
        powerUps = GameObject.FindGameObjectsWithTag("Power Up");

        // loops through the array and sets all objects on the canvas inactive (stops displaying them)
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.SetActive(false);
        }
        foreach (GameObject collection in obstaclesCollections)
        {
            collection.SetActive(false);
        }
        foreach (GameObject powerUp in powerUps)
        {
            powerUp.SetActive(false);
        }
    }

    public void EnableGameObjects()
    {
        // loops through the array and sets all objects on the canvas active (displays them)
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.SetActive(true);
        }
        foreach (GameObject collection in obstaclesCollections)
        {
            collection.SetActive(true);
        }
        foreach (GameObject powerUp in powerUps)
        {
            powerUp.SetActive(true);
        }
    }
}



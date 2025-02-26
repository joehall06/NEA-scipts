using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [Header("Scenes")]
    public GameObject OpeningScreen;
    public GameObject MainMenuScreen;
    public GameObject LoginScreen;

    // game is loaded by changing scene when play button is hit
    public void OriginalPlayGame()
    {
        StaticData.dailyChallengeSelected = false;

        // the game scene is loaded
        // the 'orginal' game mode is located in index 1 of the build settings within Unity
        SceneManager.LoadScene(1);
        // ensures the game is running at all times when the game scene is loaded
        Time.timeScale = 1;
    }

    // allows game objects to be accessed and made active / inactive
    public GameObject dailyChallengeLeaderboard;
    public GameObject playMenu;

    public void DailyChallengePlayGame()
    {
        // calls if it is the first time that this function has been called
        if (!StaticData.dailyChallengePlayed)
        {
            // boolean values are changed
            // ensures that this game mode can only be called once per day
            StaticData.dailyChallengePlayed = true;
            StaticData.dailyChallengeSelected = true;

            // the game scene is loaded
            SceneManager.LoadScene(1);
            // ensures the game is running at all times when the game scene is loaded=
            Time.timeScale = 1;
        }
        else
        {
            dailyChallengeLeaderboard.SetActive(true);
            playMenu.SetActive(false);
        }
    }

    // original starting menu is loaded by changing scene when a button is hit
    public void PlayStartingMenu()
    {
        // the original starting menu screen is loaded
        // the starting screen is loacted in index 0 of the build setting within Unity
        SceneManager.LoadScene(0);
    }


    // forces application to close
    public void QuitGame()
    {
        // prints to console as it can't be closed with Unity
        // ensures that the function is being executed
        Debug.Log("APPLICATION CLOSED!");
        Application.Quit();
    }
}

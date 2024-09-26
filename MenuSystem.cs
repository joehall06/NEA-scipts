using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    // game is loaded by changing scene when play button is hit
    public void PlayGame()
    {
        // the game scene is loaded
        // the 'orginal' game mode is located in index 1 of the build settings within Unity
        SceneManager.LoadScene(1);
        // ensures the game is running at all times when the game scene is loaded
        Time.timeScale = 1;
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

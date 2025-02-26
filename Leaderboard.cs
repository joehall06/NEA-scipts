using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    // reference to the timer text box
    public TextMeshProUGUI timerText;

    public AuthenticationManager authenticationManager;

    // Update is called once per frame
    void Update() 
    {
        UpdateTimer();
    }

    // updates the time displayed using the time remaining until midnight
    private void UpdateTimer()
    {
        // uses the system
        // ensures completely accurate results
        // calculates time remaining until midnight
        System.DateTime now = System.DateTime.Now;
        System.DateTime midnight = now.Date.AddDays(1);
        System.TimeSpan timeUntilMidnight = midnight - now;

        // update the timer text
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}",timeUntilMidnight.Hours, timeUntilMidnight.Minutes, timeUntilMidnight.Seconds);

        // checks if timer has reached midnight
        if (timeUntilMidnight.TotalSeconds <= 4000)
        {
            // allow the 'Daily Challenge' game mode to be played again
            StartCoroutine(authenticationManager.ChangeDailyChallengeBooleanDatabase());
        }
    }

    public TMP_Text usernameText;
    public TMP_Text highscoreText;


    public void NewScoreElement(string username, int highscore)
    {
        usernameText.text = username;
        highscoreText.text = highscore.ToString();
    }
}









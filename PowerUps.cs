using TMPro;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // reference to other scripts
    // allows public variables to be altered
    public GameManager gameManager;
    private PlayerMovement playerMovement;

    // controls the power up text display
    public TextMeshProUGUI powerUpText;
    public GameObject powerUpTextDisplay;
    private float powerUpTextTimeRemaining;
    // holds the text which is displayed 
    private string powerUpTextMessage;

    // controls the power up text display
    public TextMeshProUGUI powerUpTimeText;
    public GameObject powerUpTimeTextDisplay;
    private float powerUpTimeRemaining;

    // boolean for power up 1 
    public bool decreaseLives = true;
    // times used for power up 1 and 3
    private float powerUpTimerOne;
    private float powerUpTimerTwo;

    // controls the sound effect applied
    public AudioSource source;
    // allows different sound effects to be generated
    public AudioClip s1, s2, s3, s4;



    // Start is called before the first frame update
    void Start()
    {
        // searches the scene for a GameObject that has the PlayerMovement script attached
        // allows it to be always accessed
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        // used for power up 1
        if (powerUpTimerOne > 0) 
        {
            powerUpTimerOne -= Time.deltaTime;

            if (powerUpTimerOne <= 0) 
            {
                // calls function which deactivates the power up
                DeactivatePowerUpOne();
            }
        }
        // used for power up 2
        if (powerUpTimerTwo > 0)
        {
            powerUpTimerTwo -= Time.deltaTime;

            if (powerUpTimerTwo <= 0)
            {
                // calls function which deactivates the power up
                DeactivatePowerUpThree();
            }
        }

        // repeatedly calls the display function
        // determines if the text should be displayed or not
        PowerUpTextDisplay();

        // repeatedly calls the display function
        // determines if the text should be displayed or not
        PowerUpTimeRemainingDisplay();
    }

    // unlimited lives
    // lasts 5 seconds
    public void PowerUpOne()
    {
        // determines time
        powerUpTimerOne = 5f;
        // disables players lives from decreasing
        decreaseLives = false;

        // controls the message displayed by the text
        // controls the length the text is displayed for
        powerUpTextMessage = "Unlimited Lives!";
        powerUpTextTimeRemaining = 1f;
        PowerUpTextDisplay();

        // determines time
        powerUpTimeRemaining = 5f;
        PowerUpTimeRemainingDisplay();

        // plays the chosen sound effect
        source.clip = s1;
        source.Play();
    }

    // deactivate power up 1
    private void DeactivatePowerUpOne()
    {
        // return value to its original state
        decreaseLives = true;
    }

    // decreases lives remaining to 1
    public void PowerUpTwo()
    {
        // immediately calls the new game function
        gameManager.livesRemaining = 1f;
        gameManager.DisplayLives();

        // controls the message displayed by the text
        // controls the length the text is displayed for
        powerUpTextMessage = "OOPS!";
        powerUpTextTimeRemaining = 1f;
        PowerUpTextDisplay();

        // plays the chosen sound effect
        source.clip = s2;
        source.Play();
    }

    // slow motion
    // lasts 10 seconds
    public void PowerUpThree()
    {
        // determines time
        powerUpTimerTwo = 10f;
        // slows the game down by 3/4
        Time.timeScale = 0.75f;

        // controls the message displayed by the text
        // controls the length the text is displayed for
        powerUpTextMessage = "Slowww motion!";
        powerUpTextTimeRemaining = 1f;
        PowerUpTextDisplay();

        // determines time
        powerUpTimeRemaining = 10f;
        PowerUpTimeRemainingDisplay();

        // plays the chosen sound effect
        source.clip = s3;
        source.Play();
    }

    // deactivate power up 3
    private void DeactivatePowerUpThree()
    {
        // return value to its original state
        Time.timeScale = 1f;
    }

    // increase lives remaining by 1
    public void PowerUpFour()
    {
        // calls the function which increases game lives
        gameManager.livesRemaining += 1f;
        gameManager.DisplayLives();

        // controls the message displayed by the text
        // controls the length the text is displayed for
        powerUpTextMessage = "Lucky you!";
        powerUpTextTimeRemaining = 1f;
        PowerUpTextDisplay();

        // plays the chosen sound effect
        source.clip = s4;
        source.Play();
    }

    // conrols the text which is displayed
    // called each time a power up is triggered
    private void PowerUpTextDisplay()
    {
        // checks if there is time remaining to display the text
        if (powerUpTextTimeRemaining > 0)
        {
            // turns on the text
            powerUpTextDisplay.SetActive(true);
            // decreases time remaining for text to display
            powerUpTextTimeRemaining -= Time.deltaTime;
            // displays the individual message
            // unique for each power up 
            powerUpText.text = powerUpTextMessage;
        }
        else
        {
            // turns off the text
            powerUpTextDisplay.SetActive(false);
        }
    }

    // controls the numbers which are displayed
    // called each time a power up with a timer is triggered
    private void PowerUpTimeRemainingDisplay()
    {
        if (powerUpTimeRemaining > 0) 
        {
            // turns on the text
            powerUpTimeTextDisplay.SetActive(true);
            // decreases time remaining for text to display
            powerUpTimeRemaining -= Time.deltaTime;
            // displays the individual message

            powerUpTimeText.text = Mathf.Ceil(powerUpTimeRemaining).ToString();
        }
        else
        {
            // turns off the text
            powerUpTimeTextDisplay.SetActive(false);
        }
    }
}





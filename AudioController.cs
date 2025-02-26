using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    // allows different audio sources to be accessed
    public AudioSource backgroundMusic1;
    public AudioSource soundEffects;

    // booleans determinig whether sound is muted or unmuted
    private bool backgroundMusicMuted = false;
    private bool soundEffectsMuted = false;


    // storing the different buttons as GameObjects allowing them to be controlled
    public GameObject backgroundMusicOnButton;
    public GameObject backgroundMusicOffButton;
    public GameObject soundEffectsOnButton;
    public GameObject soundEffectsOffButton;


    // Start is called before the first frame update
    void Start()
    {
        // loads mute data from PlayerPrefs
        backgroundMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        soundEffectsMuted = PlayerPrefs.GetInt("EffectsMuted", 0) == 1;

        // applies the mute settings
        backgroundMusic1.mute = backgroundMusicMuted;
        soundEffects.mute = soundEffectsMuted;


        UpdateButtonStates();
    }

    public void ToggleMusic()
    {
        // changes the state of boolean
        // causes the state of the sound to change
        backgroundMusicMuted = !backgroundMusicMuted;
        backgroundMusic1.mute = backgroundMusicMuted;

        // updates the PlayerPrefs
        if (backgroundMusicMuted)
        {
            PlayerPrefs.SetInt("MusicMuted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MusicMuted", 0);
        }
    }

    public void ToggleSoundEffects()
    {
        // changes the state of boolean
        // causes the state of the sound to change
        soundEffectsMuted = !soundEffectsMuted;
        soundEffects.mute = soundEffectsMuted;

        // updates the PlayerPrefs
        if (soundEffectsMuted)
        {
            PlayerPrefs.SetInt("EffectsMuted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("EffectsMuted", 0);
        }
    }

    public void UpdateButtonStates()
    {
        // changes states of the background music button according to the boolean
        backgroundMusicOnButton.SetActive(!backgroundMusicMuted);
        backgroundMusicOffButton.SetActive(backgroundMusicMuted);

        // changes states of the sound effect music button according to the boolean
        soundEffectsOnButton.SetActive(!soundEffectsMuted);
        soundEffectsOffButton.SetActive(soundEffectsMuted);
    }
}



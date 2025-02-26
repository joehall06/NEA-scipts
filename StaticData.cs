using UnityEngine;

// stores data that can be used across multiple scenes
public class StaticData : MonoBehaviour
{
    // stores the sprite image
    public static Sprite selectedSprite;
    // stores the sprite colour
    public static Color selectedColor;

    // boolean values refering to the 'Daily Challenge' game mode
    public static bool dailyChallengePlayed;
    public static bool dailyChallengeSelected;
    public static int dailyChallengeScore;

    // stores highscore value (original game mode)
    public static float originalHighScore;
}




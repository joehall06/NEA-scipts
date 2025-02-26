using UnityEngine;
using UnityEngine.UI;

public class SpriteMenu : MonoBehaviour
{
    // reference to the image where the sprite image will be changed
    public Image imageDisplayImage;
    // reference to the image where the sprite colour will be changed
    public GameObject colourDisplayImage;

    // stores default sprite
    // stores default colour (dark blue)
    public Sprite defaultSpriteImage;
    public Color deafaultSpriteColor = new Color32(0, 0, 255, 255);


    // Start is called before the first frame update
    // checks if static data contains contains a value
    // if not default colour and image is displayed
    void Start()
    {
        if (StaticData.selectedSprite != null)
        {
            imageDisplayImage.sprite = StaticData.selectedSprite;
        }
        else
        {
            imageDisplayImage.sprite = defaultSpriteImage;
        }


        if (StaticData.selectedColor != default)
        {
            imageDisplayImage.color = StaticData.selectedColor;
        }
        else
        {
            imageDisplayImage.color = deafaultSpriteColor;
        }
    }


    // called from Unity
    // changes the sprite image
    public void DisplaySprite(Sprite selectedSprite)
    {
        // set the new sprite to the display image
        imageDisplayImage.sprite = selectedSprite;
        StaticData.selectedSprite = selectedSprite;
    }

    // called from Unity
    // changed colour of sprite image
    // each function has a different colour
    // all in RGB format
    public void ChangeSpriteColour1()
    {
        // green
        colourDisplayImage.GetComponent<Image>().color = new Color32(126,255,0,255); 
        StaticData.selectedColor = new Color32(126, 255, 0, 255);
    }
    public void ChangeSpriteColour2()
    {
        // dark blue
        colourDisplayImage.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        StaticData.selectedColor = new Color32(0, 0, 255, 255);
    }
    public void ChangeSpriteColour3()
    {
        // pink
        colourDisplayImage.GetComponent<Image>().color = new Color32(126, 0, 255, 255);
        StaticData.selectedColor = new Color32(126, 0, 255, 255);
    }
    public void ChangeSpriteColour4()
    {
        // yellow
        colourDisplayImage.GetComponent<Image>().color = new Color32(255, 184, 0, 255);
        StaticData.selectedColor = new Color32(255, 184, 0, 255);
    }
    public void ChangeSpriteColour5()
    {
        // dark blue
        colourDisplayImage.GetComponent<Image>().color = new Color32(0, 201, 255, 255);
        StaticData.selectedColor = new Color32(0, 201, 255, 255);
    }
    public void ChangeSpriteColour6()
    {
        // red
        colourDisplayImage.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        StaticData.selectedColor = new Color32(255, 0, 0, 255);
    }
}



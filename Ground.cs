using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // reference to the MeshRenderer component added to the ground
    // makes it easier to access this component
    private MeshRenderer meshRenderer;

    // reference to the script 'GameManager' allowing public variable to be used
    public GameManager gameManager;
    private float gameSpeed;


    // Start is called before the first frame update
    void Start()
    {
        // sets the variable meshRenderer to the characters MeshRenderer in order to easily access variable
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // stores the public variable 'gameSpeed' found in the 'GameManager' script
        gameSpeed = gameManager.gameSpeed;

        // moves the texture of the material to the left
        // creates allusion that player is moving
        // speed us adjusted by the scale of the ground object in the x direction (25)
        // maintains conistent appearance
        meshRenderer.material.mainTextureOffset += Vector2.right * (gameSpeed / transform.localScale.x) * Time.deltaTime;
    }
}




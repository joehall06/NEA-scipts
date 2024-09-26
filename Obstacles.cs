using Unity.VisualScripting;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    // reference to the script 'GameManager' allowing public variable to be used
    public GameManager gameManager;
    private float gameSpeed;


    // Start is called before the first frame update
    private void Start()
    {
        // due to the obstacles being prefabs, they are not on the scene
        // this function is then called
        // searches the scene for a GameObject that has the GameManager script attached
        gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // stores the public variable 'gameSpeed' found in the 'GameManager' script
        gameSpeed = gameManager.gameSpeed;

        // moves the obstacles to the left
        // creates allusion that player is moving towards them
        transform.position += Vector3.left * gameSpeed * Time.deltaTime;

        // destroys the obstacles when they go out of sight
        // stops the build up on infinite obstacles
        if (transform.position.x < -11)
        {
            // removes the game object in which the script is attached to (obstacles)
            Destroy(gameObject);
        }
    }
}





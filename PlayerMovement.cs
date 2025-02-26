using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // reference to the RigidBody2D component added to the player game object
    // allows this game object to be accessed
    private Rigidbody2D rigidBody;
    // reference to the script 'GameManager' allowing public variable to be used
    public GameManager gameManager;

    public PowerUps powerUps;

    // creates a fixed jumping force
    public float jumpForce = 40f;

    // boolean determining if player is touching the ground game object
    private bool playerGrounded = false;
    // boolean determining if player can jump or not
    public bool allowJump = true;

    // creates a new 2D vector
    public Vector2 originalScale;

    
    // used for flashing sprites flashing effect
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    private bool spriteFlashing = false;

    // Start is called before the first frame update
    void Start()
    {
        // sets the variable rigidBody to the characters RigidBody2D in order to access the variable
        rigidBody = GetComponent<Rigidbody2D>();

        // searches the scene for a GameObject that has the GameManager script attached
        // allows it to be always accessed
        gameManager = FindObjectOfType<GameManager>();

        // searches the scene for a GameObject that has the PowerUps script attached
        // allows it to be always accessed
        //powerUps = FindObjectOfType<PowerUps>();

        // allows the sprite to be controlled
        // used for the sprite flashing effect
        spriteRenderer = player.GetComponent<SpriteRenderer>();

        // sets the variable orginalScale to the game objects scale
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the sprite is on the ground
        if (playerGrounded)
        {
            // allows sprite to crouch
            // when Left Control is held down
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                // used the multiplier 0.5 as this will half the scale of the object
                transform.localScale = new Vector2(originalScale.x, originalScale.y * 0.5f);

                // disables the jumping mechanic whilst crouched
                allowJump = false;
            }

            // when Left Control is released
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                // resets the scale to what was predifined when the script was first ran
                transform.localScale = originalScale;

                // enables the jumping mechanic when uncrouched
                allowJump = true;
            }
        }
    }

    // FixedUpdate() is used as it called at a constant 50 times a second
    // makes physics smoother
    private void FixedUpdate()
    {
        // checks if the sprite is on the ground
        if (playerGrounded)
        {
            // allows the sprite to jump
            // removes the ability to jump again whilst in the air
            if (Input.GetKey(KeyCode.Space) && allowJump)
            {
                // applies an impulse force in the y direction
                rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }


    // boolean holding value if sprite is colliding
    private bool isColliding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks if the other game object's tag is 'Ground'
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Floor")))
        {
            playerGrounded = true;
        }

        // checks if the other game object's tag is an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // if true, the function will not occur
            if (isColliding) return;
            isColliding = true;

            // causes collided obstacle to disappear
            collision.gameObject.SetActive(false);

            if (powerUps.decreaseLives)
            {
                // calls function in 'Game Manager' script
                gameManager.DecreaseLives();
            }

            // causes sprite to flash
            ControlSpriteFlash();
        }

        // checks if the other game object's tag is a floor
        if (collision.gameObject.name == "LeftEdge")
        {
            if (powerUps.decreaseLives)
            {
                // calls function in 'Game Manager' script
                gameManager.DecreaseLives();
            }

            // adds a position vector to each obstacle present
            // makes illusion as if sprite has moved back
            GameObject[] obstaclesCollections = GameObject.FindGameObjectsWithTag("Obstacle Collection");
            foreach (GameObject collection in obstaclesCollections)
            {
                collection.transform.position += new Vector3(8, 0, 0);
            }

            // causes sprite to flash
            ControlSpriteFlash();
        }

        // checks if the other game object's tag is a power up
        if (collision.gameObject.CompareTag("Power Up"))
        {
            // if true, the function will not occur
            if (isColliding) return;
            isColliding = true;

            // causes collided power up to disappear
            collision.gameObject.SetActive(false);

            // depending on the power up
            // specific function is called
            switch (collision.gameObject.name)
            {
                case "PU 1(Clone)":
                    powerUps.PowerUpOne();
                    break;

                case "PU 2(Clone)":
                    powerUps.PowerUpTwo();
                    break;

                case "PU 3(Clone)":
                    powerUps.PowerUpThree();
                    break;

                case "PU 4(Clone)":
                    powerUps.PowerUpFour();
                    break;
            }
        }

        // schedules the function be called 0.1s later
        Invoke("ResetCollisionFlag", 0.1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // only reset grounded if exiting ground or floor colliders
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Floor"))
        {
            playerGrounded = false;
        }
    }

    // allows object to collide again
    private void ResetCollisionFlag()
    {
        isColliding = false;
    }

    // controls the flash of the sprite
    private void ControlSpriteFlash()
    {
        if (!spriteFlashing)
        {
            spriteFlashing = true;
            // repeatedly calls every 0.1s
            InvokeRepeating("SpriteFlashStart", 0f, 0.1f);
            // schedules the function be called 1s later
            Invoke("SpriteFlashStop", 1f);
        }
    }

    private void SpriteFlashStart()
    {
        // disables / enabled the sprite renderer of the sprite
        // creates flashing illusion
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void SpriteFlashStop()
    {
        // stops a function from being called
        CancelInvoke("SpriteFlashStart");
        
        // reset variables
        spriteRenderer.enabled = true;
        spriteFlashing = false;
    }
}







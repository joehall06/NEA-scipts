using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // reference to the RigidBody2D component added to the player game object
    // allows this game object to be accessed
    private Rigidbody2D rigidBody;
    // reference to the script 'GameManager' allowing public variable to be used
    public GameManager gameManager;

    // creates a variable which can be controlled to the player lives remaining
    private float livesRemaining = 3f;

    // creates a fixed jumping force
    private float jumpForce = 20f;

    // boolean determining if player is touching the ground game object
    private bool playerGrounded = false;
    // boolean determining if player can jump or not
    private bool allowJump = true;

    // creates a new 2D vector
    private Vector2 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        // sets the variable rigidBody to the characters RigidBody2D in order to access the variable
        rigidBody = GetComponent<Rigidbody2D>();

        // searches the scene for a GameObject that has the GameManager script attached
        // allows it to be always accessed
        gameManager = FindObjectOfType<GameManager>();

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

    // checks if a player's collider has triggered with another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks if the other game object's tag is 'Ground'
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerGrounded = true;
        }
    }

    // checks if a player's collider has untriggered
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerGrounded = false;
    }

    // checks if a player's collider has collided with another collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // checks if the collider has the tag 'Obstacle'
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // decreases the lives remaining
            livesRemaining -= 1;
            gameManager.DecreaseLives(livesRemaining);

            // checks if the player has no lives remaining
            if (livesRemaining <= 0)
            {
                gameManager.GameOver();
            }
        }
    }
}




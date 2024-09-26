using UnityEngine;
public class _PlayerMovement : MonoBehaviour
{
    // reference to the CharacterComponent component added to the player
    // makes it easier to access this component
    private CharacterController characterController;
    // reference to the script 'GameManager' allowing public variable to be used
    public GameManager gameManager;

    // creates a fixed jumping force which can be edited within unity
    private float jumpForce = 15f;

    // creates a fixed gravity force which can be edited within unity
    private float gravity = 50f;

    // vector used to store the player's movement direction
    private Vector2 direction;


    private float originalYScale;
    private float originalRadius;

    private bool allowJump = true;
    private bool allowCrouch = true;

    // sets the crouch sprite scale to 0.5 in the y direction, halfing the original scale (1)
    private float crouchYScale = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        // sets the variable rigidBody to the characters RigidBody2D in order to easily access variable
        characterController = GetComponent<CharacterController>();

        // sets the initial direction to zero
        // ensures no movement at the star
        direction = Vector2.zero;

        // stores original values from unity
        originalYScale = transform.localScale.y;
        originalRadius = characterController.radius;

        characterController.detectCollisions = false;
    }

    // FixedUpdate() is used as it called at a constant 50 times a second
    // makes physics smoother
    void FixedUpdate()
    {
        // apply gravity to the direction of the vector (downwards)
        // Time.fixedDeltaTime is used to ensure that gravity is applied across all frames
        direction += Vector2.down * gravity * Time.fixedDeltaTime;

        // checks if the sprite is on the ground
        // allows the sprite to be able to jump or not jump
        // removes the ability to jump again whilst in the air
        if (characterController.isGrounded && allowJump)
        {
            // resets the direction to only apply a downwards force when grounded
            direction = Vector2.down;

            if (Input.GetKey(KeyCode.Space))
            {
                // applies an upward force to the vector
                // only occurs once all conditions are met
                direction = Vector2.up * jumpForce;
            }
        }
        else if (!characterController.isGrounded)
        {
            allowCrouch = false;
        }


        // move the character based on the vector calculated
        // Time.fixedDeltaTime is used to ensure that gravity is applied across all frames
        characterController.Move(direction * Time.fixedDeltaTime);
    }

    // using 'Update' is more suited to certain inputs
    void Update()
    {
        if (characterController.isGrounded)
        {
            // when key is pressed down
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                // changes the y scale of the sprite to 0.5
                transform.localScale = new Vector2(transform.localScale.x, crouchYScale);
                // changes the radius of the sprite
                // allows it touch the ground
                characterController.radius = 0.3f;

                allowJump = false;
            }
            // when key is released
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                // resets both scale and radius to original values
                transform.localScale = new Vector2(transform.localScale.x, originalYScale);
                characterController.radius = originalRadius;

                allowJump = true;
            }
        }
    }

    // checks if player collides with obstacles
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // called when player enters a trigger collider (obstacle)
        if (hit.gameObject.name == "D - Spike 1(Clone)")
        {
            //float livesRemaining = gameManager.livesRemaining;

            // decreases the amount of lives the player has left
            gameManager.livesRemaining -= 1;

            print("Collision Detected");

            Debug.Log("Collision detected");
            Debug.Log($"Lives remaining: {gameManager.livesRemaining}");

            // if player has 0 lives remaining
            // game over
            if (gameManager.livesRemaining <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }
}




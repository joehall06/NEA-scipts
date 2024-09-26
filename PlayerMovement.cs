using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // reference to the RigidBody2D component added to the player game object
    // allows this game object to be accessed
    private Rigidbody2D rigidBody;

    // creates a fixed jumping force
    private float jumpForce = 20f;

    // boolean determining if player is touching the ground game object
    private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        // sets the variable rigidBody to the characters RigidBody2D in order to access the variable
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate() is used as it called at a constant 50 times a second
    // makes physics smoother
    private void FixedUpdate()
    {
        // checks if the sprite is on the ground
        // allows the sprite to jump
        // removes the ability to jump again whilst in the air
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            // applies an impulse force in the y direction
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    // checks if a player's collider has triggered with another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks if the other game object's tag is 'Ground'
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    // checks if a player's collider has untriggered
    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }
}




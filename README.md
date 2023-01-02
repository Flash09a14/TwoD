# TwoD (WORK IN PROGRESS GAME)
## A 2D unity game I'm working on, updates will display here
### About
#### Concept:
It's an action 2D platformer. You start on a platform that has obstacles and enemies blocking the way from the exit portal to the next level. You unlock an arsenal while you play the game. Each weapon has a unique perk that shape game mechanics completely. As you progress the levels get harder and harder, unlocking different perks, mechanics, and enemies. The arsenal is huge and has many variations, each weapon has interactions with one another and the environment.
### Art style:
Vibrant poly and pixel art with a gray/dusty color pallette (May vary depending on level).

# Code (so far)
## Movement
```cs
// Modules
using UnityEngine;

// Public script class
public class Movement : MonoBehaviour
{
    // Declaring variables
    // Speed
    public float speed = 10.0f;
    // Jump force
    public float jumpForce = 10.0f;
    // Air drag
    public float airDrag = 0.1f;
    
    // Referencing Rigidbody
    private Rigidbody2D rb;
    // Boolean to check if we're grounded
    private bool isGrounded = false;
    // Radius of the ground check
    private float groundCheckRadius = 0.2f;
    // Layer mask for ground
    public LayerMask groundLayer;
    // Transform of a ground check game object
    public Transform groundCheck;
    
    // Start, updates only at the first frame
    void Start()
    {
        // Assigning rigidbody to a variable
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Update, Updates per frame
    void Update()
    {
        // Checks if grounded, it uses the game object that is directly under the player to check if it's in contact with the ground.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Movement vector, moves the player along the axes (x, y)
        Vector2 movement = new Vector2(moveHorizontal * Time.deltaTime, moveVertical * Time.deltaTime);

        // Rigidbody multiplies the movement vector with the speed variable
        rb.AddForce(movement * speed);
        // Setting rigidbody drag to aurDrag variable
        rb.drag = airDrag;

        // Checks for jump input. Only gets called if you are grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Adds an impulse force upwards
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
```

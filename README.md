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

## Animation
```cs
using UnityEngine;

public class AnimChange : MonoBehaviour
{
    // An array of sprites that we want to cycle through when the character is idle
    public Sprite[] idleSprites;

    // The sprite that we want to switch to when the "D" key is pressed
    public Sprite newSprite;

    // The sprite that we want to switch to when the "A" key is pressed
    public Sprite anotherSprite;

    // The delay between each sprite it cycles through (in seconds)
    public float delay = 1.5f;

    // A reference to the sprite renderer component
    private SpriteRenderer spriteRenderer;

    // The current index in the idleSprites array
    private int currentIdleSpriteIndex = 0;

    // The time that the current idle sprite was last changed
    private float lastIdleSpriteChangeTime;

    void Start()
    {
        // Get a reference to the sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // If the player is not pressing the "D" key or the "A" key
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            // If it has been the specified delay since the last time the idle sprite was changed
            if (Time.time - lastIdleSpriteChangeTime > delay)
            {
                // Increment the current idle sprite index
                currentIdleSpriteIndex++;

                // If the current idle sprite index is greater than or equal to the length of the idleSprites array
                if (currentIdleSpriteIndex >= idleSprites.Length)
                {
                    // Reset the current idle sprite index to 0
                    currentIdleSpriteIndex = 0;
                }

                // Change the sprite to the current idle sprite
                spriteRenderer.sprite = idleSprites[currentIdleSpriteIndex];

                // Update the last idle sprite change time
                lastIdleSpriteChangeTime = Time.time;
            }
        }
        // If the player is pressing the "D" key
        else if (Input.GetKey(KeyCode.D))
        {
            // Change the sprite to the new sprite
            spriteRenderer.sprite = newSprite;
        }
        // If the player is pressing the "A" key
        else if (Input.GetKey(KeyCode.A))
        {
            // Change the sprite to the another sprite
            spriteRenderer.sprite = anotherSprite;
        }
    }
}
```
## CameraFollow
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // the target that the camera should follow
    public float smoothing = 5f; // the smoothing factor for the camera movement

    Vector3 offset; // the initial offset between the camera and the target

    void Start()
    {
        // Calculate the initial offset
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        // Create a position that the camera should be in
        Vector3 targetCamPos = target.position + offset;

        // Smoothly move the camera towards that position
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
```
## Respawn
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform respawnPoint; // the position to respawn the player at

    void Update()
    {
        // Check if the player has fallen off the platform
        if (transform.position.y < -50f)
        {
            // Teleport the player back to the respawn point
            transform.position = respawnPoint.position;
        }
        
    }
}
```


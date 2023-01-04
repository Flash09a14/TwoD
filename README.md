# TwoD (WORK IN PROGRESS GAME)
## A 2D unity game I'm working on, updates will display here
### About
#### Concept:
It's an action 2D platformer. You start on a platform that has obstacles and enemies blocking the way from the exit portal to the next level. You unlock an arsenal while you play the game. Each weapon has a unique perk that shape game mechanics completely. As you progress the levels get harder and harder, unlocking different perks, mechanics, and enemies. The arsenal is huge and has many variations, each weapon has interactions with one another and the environment.
### Art style:
Vibrant poly and pixel art with a gray/dusty color pallette (May vary depending on level).

# Code (so far)
## Movement and Animation
```cs
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 10.0f;
    public float airDrag = 0.1f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public Sprite rightSprite;
    public Sprite leftSprite;
    public Sprite jumpSprite;
    public Sprite fallSprite;
    public Sprite fireSprite; // new variable for the sprite when fire1 is pressed
    public float animationMaxSpeed = 0.5f;

    public Sprite[] idleSprites; // list of idle sprites
    private int idleSpriteIndex = 0; // counter to iterate through the list of idle sprites
    public float idleSpriteInterval = 0.5f; // interval at which to cycle through the idle sprites (adjustable through a public variable)
    private float nextIdleSprite = 0f; // time at which to change the idle sprite

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprites[0]; // set the initial sprite to the first element in the list
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal * Time.deltaTime, moveVertical * Time.deltaTime);

        rb.AddForce(movement * speed);
        rb.drag = airDrag;

        if (moveHorizontal > 0)
        {
            spriteRenderer.sprite = rightSprite;
        }
        else if (moveHorizontal < 0)
        {
            spriteRenderer.sprite = leftSprite;
        }
        else
        {
            spriteRenderer.sprite = idleSprites[idleSpriteIndex]; // set the sprite to the current element in the list of idle sprites
            if (Time.time > nextIdleSprite) // check if it's time to change the idle sprite
            {
                idleSpriteIndex++; // increment the counter
                if (idleSpriteIndex >= idleSprites.Length) // if the counter exceeds the number of idle sprites, set it back to zero
                {
                    idleSpriteIndex = 0;
                }
                nextIdleSprite = Time.time + idleSpriteInterval; // set the time for the next sprite change
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            spriteRenderer.sprite = jumpSprite;
        }
        else if (!isGrounded)
        {
            spriteRenderer.sprite = fallSprite;
        }

        // Replace the sprite while fire1 is held
        if (Input.GetButton("Fire1"))
        {
            if (Mathf.Abs(rb.velocity.x) < animationMaxSpeed && isGrounded) // check if player is at a specific adjustable velocity and grounded
            {
                spriteRenderer.sprite = fireSprite;
            }
        }
    }
}
```
## CameraFollow
```cs
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target that the camera should follow
    public float smoothTime = 0.3f; // The time it takes for the camera to catch up with the target's position

    private Vector3 velocity = Vector3.zero; // The velocity at which the camera should move

    void LateUpdate()
    {
        // Calculate the new position for the camera
        Vector3 newPos = target.position;

        // Smoothly move the camera towards the new position
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
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
## Projectile Destroy
```cs
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        // Destroy the projectile when it collides with a surface
        Destroy(gameObject);
    }
}
```
## Gun Script (Shooting Script)
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject projectilePrefab; // Drag and drop the projectile prefab in the inspector
    public float fireRate = 1f;
    public float nextFire = 0f;
    public float velocity = 50f;

    void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Fire the projectile when the player left clicks the mouse
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            FireProjectile(mousePos);
        }
    }

    void FireProjectile(Vector3 targetPos)
    {
        // Create an instance of the projectile prefab at the position and rotation of the player
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Calculate the direction to the target position
        Vector3 direction = targetPos - transform.position;

        // Normalize the direction to get a unit vector
        direction = direction.normalized;

        // Add velocity to the projectile in the direction of the target position
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * velocity;
    }
}
```

# HITSTORM (WORK IN PROGRESS GAME)
## A 2D unity game I'm working on, updates will display here
### About
#### Concept:
It's an action 2D platformer. You start on a platform that has obstacles and enemies blocking the way from the exit portal to the next level. You unlock an arsenal while you play the game. Each weapon has a unique perk that shape game mechanics completely. As you progress the levels get harder and harder, unlocking different perks, mechanics, and enemies. The arsenal is huge and has many variations, each weapon has interactions with one another and the environment.
### Art style:
Vibrant poly and pixel art with a gray/dusty color pallette (May vary depending on level).
## Version:
#### 0.0.7 (Early Development):
##### Changes and Patches
###### Movement: Improved air control.
###### Mechanics: Added jump pads. Jump pads can be found throughout the map and give a large vertical impulse to objects, enemies, and the player.
###### Mechanics: Improved weapon handling.
###### Mechanics: Added double jump powerup. Now, you must find a powerup on the floor to temporarily double jump (powerups are infinitely stackable).
###### Mechanics: Added speed boost powerup. Just like the double jump powerup, it boosts your speed temporarily and is stackable.
###### Mechanics: Added enemy-side damage handling (player-side still in progress).
###### Mechanics: Added portals (usually will be found in levels for puzzles, level travel, or object interaction. (early beta).
###### Art: Completely changed player model and animation.
###### Art: Changed sentry model.
###### Art: Made setting more vibrant.
###### Code: Reworked and simplified a lot of code (more info on code updates)
###### Code: Added seperate scripts for different handles for simplicity and better memory.
###### Extra notes: Disabled Sentry enemy temporarily due to technical issues and works of new enemies.
## Last Updated (Github and Game)
#### 20/02/2023 (0.0.7)
# Code (so far)
## Movement
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
    private int jumpCounter = 0;
    public int maxJumps = 2; 
    public float powerUpDuration = 5.0f;
    private float powerUpTimer = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = 0;

        Vector2 movement = new Vector2(moveHorizontal * Time.deltaTime, moveVertical * Time.deltaTime);

        rb.AddForce(movement * speed);
        rb.drag = airDrag;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpCounter = 0;
        }

        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
        {
            jumpCounter++;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            // Play the sound effect
            GetComponent<AudioSource>().Play();
        }
        if (powerUpTimer > 0)
        {
            powerUpTimer -= Time.deltaTime;
        }
        else if (powerUpTimer <= 0)
        {
            maxJumps = 1;
            speed = 4500;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUpJump"))
        {
            maxJumps += 1;
            powerUpTimer = powerUpDuration;
        }
        else if (other.CompareTag("PowerUpSpeed"))
        {
            speed += 2000;
            powerUpTimer = powerUpDuration;
        }
    }

}
```
## CameraFollow
```cs
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
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
    public int damage = 10;
    private void Start()
    {
        // Ignore collisions between the projectile and objects with the "Player" layer
        int projectileLayer = gameObject.layer;
        int playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(projectileLayer, playerLayer, true);
    }

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

        // Play the sound effect
        GetComponent<AudioSource>().Play();
    }
}
```
## Audio Player for Powerups
```cs
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip soundEffect;
    public float destroyDelay = 1f;
    public GameObject player;

    private AudioSource audioSource;
    private bool hasTriggered;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.gameObject == player)
        {
            audioSource.PlayOneShot(soundEffect);
            Destroy(gameObject, destroyDelay);
            hasTriggered = true;
        }
    }
}
```
## Damage Handling (Enemy-side)
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public int health = 100;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var projectile = collision.gameObject.GetComponent<Projectile>();
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().damage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
```
## Portals (Early Beta)
```cs
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = teleportTarget.position;
    }
}
```
Animation Handling
```cs
using UnityEngine;
using System.Collections;

public class SpriteHandler : MonoBehaviour
{
    public Sprite[] idleSprites;
    private int idleIndex = 0;
    public float idleInterval = 0.7f;
    private float idleTimer = 0;
    public Sprite jumpSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite fallSprite;
    public Sprite fire1LeftSprite;
    public Sprite fire1RightSprite;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isFiring = false;
    private bool shouldReturnToIdle = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            sr.sprite = jumpSprite;
            isJumping = true;
            shouldReturnToIdle = true;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            sr.sprite = leftSprite;
            shouldReturnToIdle = true;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            sr.sprite = rightSprite;
            shouldReturnToIdle = true;
        }
        else if (rb.velocity.y < 0)
        {
            sr.sprite = fallSprite;
            isFalling = true;
            shouldReturnToIdle = true;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                sr.sprite = fire1LeftSprite;
            }
            else
            {
                sr.sprite = fire1RightSprite;
            }
            isFiring = true;
            shouldReturnToIdle = true;
        }
        else if (!isJumping && !isFalling && !isFiring)
        {
            if (shouldReturnToIdle)
            {
                sr.sprite = idleSprites[idleIndex];
                shouldReturnToIdle = false;
            }
            else
            {
                idleTimer += Time.deltaTime;

                if (idleTimer >= idleInterval)
                {
                    idleTimer = 0;
                    idleIndex++;
                    if (idleIndex >= idleSprites.Length)
                    {
                        idleIndex = 0;
                    }

                    sr.sprite = idleSprites[idleIndex];
                }
            }
        }

        if (rb.velocity.y == 0)
        {
            isJumping = false;
            isFalling = false;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }
    }
}
```

## Enemy Projectile Shooting Script
### Temporarily disabled due to technical issues
## Enemy Projectile Script
### Temporarily disabled due to technical issues

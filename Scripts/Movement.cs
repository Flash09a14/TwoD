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

        rb.drag = airDrag;

        rb.AddForce(movement * speed);

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

        // Handle power-up timer
        if (powerUpTimer > 0)
        {
            powerUpTimer -= Time.deltaTime;
        }
        else
        {
            // Reset power-up effects
            speed = 4500f;
            maxJumps = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle power-up collision
        if (other.gameObject.tag == "PowerUpSpeed")
        {
            speed += 1500;
            powerUpTimer = powerUpDuration;
            // Play the power-up sound effect
            GetComponent<AudioSource>().Play();
        }
        else if (other.gameObject.tag == "PowerUpJump")
        {
            maxJumps += 1;
            powerUpTimer = powerUpDuration;
            // Play the power-up sound effect
            GetComponent<AudioSource>().Play();
        }
    }
}

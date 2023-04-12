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
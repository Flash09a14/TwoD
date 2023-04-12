using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f; // movement speed of the enemy
    public Transform target; // the target game object to follow

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Calculate the direction to move towards the target
        Vector2 direction = (target.position - transform.position).normalized;

        // Move the enemy towards the target only in the horizontal direction
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }
}

using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f; // The force with which the object will be bounced

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>(); // Get the rigidbody component of the object colliding with the pad
        if (rb != null)
        {
            Vector2 bounceDirection = Vector2.up * bounceForce; // Calculate the direction of the bounce
            rb.AddForce(bounceDirection, ForceMode2D.Impulse); // Apply the bounce force to the object's rigidbody
        }
    }
}

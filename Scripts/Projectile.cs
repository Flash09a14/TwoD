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

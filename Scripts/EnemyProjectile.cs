using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject ignoreCollisionObject;

    private void Start()
    {
        if (ignoreCollisionObject != null)
        {
            // Get the collider component of the projectile
            Collider2D projectileCollider = GetComponent<Collider2D>();
            // Get the collider component of the object to ignore collisions with
            Collider2D ignoreCollider = ignoreCollisionObject.GetComponent<Collider2D>();
            // Ignore collisions between the two colliders
            Physics2D.IgnoreCollision(projectileCollider, ignoreCollider);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != ignoreCollisionObject)
        {
            // Destroy the projectile game object
            Destroy(gameObject);
        }
    }
}

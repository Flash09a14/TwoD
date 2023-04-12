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

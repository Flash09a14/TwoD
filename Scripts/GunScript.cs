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

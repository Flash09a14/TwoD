using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform target;
    public AudioClip soundEffect;
    public float shootingInterval = 1f;
    public float projectileSpeed = 10f;
    public List<GameObject> objectsToIgnore;
    private float timeSinceLastShot = 0f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootingInterval)
        {
            timeSinceLastShot = 0f;

            // check if target is behind cover
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, Vector2.Distance(transform.position, target.position));
            bool isTargetBehindCover = false;
            if (hit.collider != null)
            {
                // check if the hit object is not in the ignore list
                if (!objectsToIgnore.Contains(hit.collider.gameObject))
                {
                    isTargetBehindCover = true;
                }
            }
            Debug.Log(isTargetBehindCover);
            if (!isTargetBehindCover)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
                Vector2 direction = (target.position - transform.position).normalized;
                projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
                audioSource.PlayOneShot(soundEffect);
            }
        }
    }
}
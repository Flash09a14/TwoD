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

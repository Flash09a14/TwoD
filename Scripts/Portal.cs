using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = teleportTarget.position;
    }
}

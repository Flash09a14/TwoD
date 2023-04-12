using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip soundEffect;
    public float destroyDelay = 1f;
    public GameObject player;

    private AudioSource audioSource;
    private bool hasTriggered;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.gameObject == player)
        {
            audioSource.PlayOneShot(soundEffect);
            Destroy(gameObject, destroyDelay);
            hasTriggered = true;
        }
    }
}

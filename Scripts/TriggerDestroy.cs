using UnityEngine;

public class TriggerDestroy : MonoBehaviour
{
    public GameObject playerObject;
    public Sprite[] sprites;
    public float interval = 1.0f;
    private int currentSprite = 0;
    private float timeSinceLastSprite = 0.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerObject)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timeSinceLastSprite += Time.deltaTime;
        if(timeSinceLastSprite >= interval)
        {
            timeSinceLastSprite = 0.0f;
            GetComponent<SpriteRenderer>().sprite = sprites[currentSprite];
            currentSprite = (currentSprite + 1) % sprites.Length;
        }
    }
}

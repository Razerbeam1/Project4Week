using UnityEngine;

public class Button : MonoBehaviour
{
    private Rigidbody2D batteryRb;
    private bool nPress = true;
    private SpriteRenderer spriteRenderer; // Reference to this object's SpriteRenderer

    [Header("Sprite Change")]
    [SerializeField] private Sprite newSprite; // The new sprite to change to

    private void Start()
    {
        // Get the SpriteRenderer from the same object this script is attached to
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this object!");
        }

        // Find the object named "Battery" and get its Rigidbody2D component
        GameObject battery = GameObject.Find("Battery");
        if (battery != null)
        {
            batteryRb = battery.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Battery object not found. Make sure an object named 'Battery' exists in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger enter");

        // Check if the object entering the trigger has the tag "Player"
        if (collision.CompareTag("Player") && batteryRb != null && nPress)
        {
            batteryRb.gravityScale = 1;
            batteryRb.linearVelocity = new Vector2(-20, batteryRb.linearVelocity.y);
            Debug.Log("Added sideways velocity to Battery.");

            // Change sprite if a new sprite is assigned
            if (spriteRenderer != null && newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
                Debug.Log("Sprite changed!");
            }
            else
            {
                Debug.LogError("No newSprite assigned in the Inspector!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        nPress = false;
    }
}
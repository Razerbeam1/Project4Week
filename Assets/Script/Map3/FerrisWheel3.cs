using UnityEngine;

public class FerrisWheel3 : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D playerRb;
    private bool playerOnPlatform = false;

    void FixedUpdate()
    {
        if (playerOnPlatform && player != null)
        {
            // Move the player with the platform
            player.position += (Vector3)(GetComponent<Rigidbody2D>().linearVelocity * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
            playerOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
            player = null;
            playerRb = null;
        }
    }
}

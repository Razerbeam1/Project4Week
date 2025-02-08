using UnityEngine;

public class BounceForce : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.rigidbody;
        if (rb != null)
        {
            float bounceForce = 50f * (rb.mass/10); // Increase force based on mass
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        }
    }
}

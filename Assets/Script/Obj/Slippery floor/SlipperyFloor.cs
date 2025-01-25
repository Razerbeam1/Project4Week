using UnityEngine;

public class SlipperyFloor2D : MonoBehaviour
{
    [SerializeField] float speedBoostMultiplier = 2f;
    [SerializeField] float maxSpeed = 10f;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTriggerStay2D triggered!");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is on slippery floor!");

            Rigidbody2D playerRigidbody = other.attachedRigidbody;

            if (playerRigidbody != null)
            {
                Debug.Log($"Player Rigidbody detected: {playerRigidbody.name}");

                Vector2 boostedVelocity = playerRigidbody.linearVelocity.sqrMagnitude > 0.1f
                    ? playerRigidbody.linearVelocity * speedBoostMultiplier
                    : Vector2.right * maxSpeed;

                if (boostedVelocity.magnitude > maxSpeed)
                {
                    boostedVelocity = boostedVelocity.normalized * maxSpeed;
                }

                playerRigidbody.linearVelocity = boostedVelocity;

                Debug.Log($"Player is speeding up! New Velocity: {boostedVelocity}");
            }
            else
            {
                Debug.Log("Player does not have a Rigidbody2D attached.");
            }
        }
    }
}
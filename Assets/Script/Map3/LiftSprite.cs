using UnityEngine;

public class LiftSprite : MonoBehaviour
{
    [SerializeField] private Transform floatingPlatform; // Reference to the moving platform
    [SerializeField] private Transform fixedPoint; // The fixed position (like the ground)
    [SerializeField] private bool stretchOnlyY = true; // If true, it stretches only in the Y direction

    private Vector3 initialScale; // Store the original scale

    private void Start()
    {
        initialScale = transform.localScale; // Save the original scale
    }

    private void Update()
    {
        // Calculate the distance between the platform and the fixed point
        float distance = Vector2.Distance(floatingPlatform.position, fixedPoint.position);

        // Update the scale of the stretching sprite
        if (stretchOnlyY)
        {
            transform.localScale = new Vector3(initialScale.x, distance, initialScale.z);
        }
        else
        {
            transform.localScale = new Vector3(distance, initialScale.y, initialScale.z);
        }

        // OPTIONAL: Position the stretching object in the middle
        transform.position = (floatingPlatform.position + fixedPoint.position) / 2;
    }
}

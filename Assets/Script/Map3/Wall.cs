using UnityEngine;
using System.Collections.Generic; // Needed for List
public class Wall : MonoBehaviour
{
    public float massThreshold = 1f; // Minimum mass required to break the wall

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the Rigidbody2D of the colliding object
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        // Check if the colliding object has a Rigidbody2D and if its mass exceeds the threshold
        if (rb != null && rb.mass > massThreshold)
        {
            BreakWall(); // Call the function to break the wall
        }
    }

    private void BreakWall()
    {
        // Store child objects in a list before modifying the hierarchy
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        // Now loop through the stored list instead of modifying transform's children directly
        foreach (Transform child in children)
        {
            child.SetParent(null); // Detach the child from the parent

            // Add Rigidbody2D if not already present
            if (!child.gameObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = child.gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1; // Adjust gravity as needed
            }

            // Add BoxCollider2D if not already present
            if (!child.gameObject.GetComponent<BoxCollider2D>())
            {
                child.gameObject.AddComponent<BoxCollider2D>();
            }
            Destroy(child.gameObject,2);
        }

        // Destroy the parent object (optional, if you don't want an empty GameObject left)
        Destroy(gameObject);
    }
}

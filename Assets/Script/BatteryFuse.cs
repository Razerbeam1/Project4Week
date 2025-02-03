using UnityEngine;

public class BatteryFuse : MonoBehaviour
{
    public bool charged = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Loop through all child objects of the colliding object
        foreach (Transform child in other.transform)
        {
            // Check if the child object has the name "Battery"
            if (child.name == "Battery")
            {
                //turn on elevator
                charged = true;
                
                // Detach the battery from its parent
                child.SetParent(null);

                // Position the battery at the trigger's location
                child.position = transform.position;

                // Reset rotation to (0,0)
                child.rotation = Quaternion.identity;

                // Ensure the battery has a Rigidbody2D
                Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
                if (rb == null)
                {
                    rb = child.gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D if missing
                }

                // Freeze X and Y position, allow only rotation
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                // Disable collider to prevent further interactions
                Collider2D batteryCollider = child.GetComponent<Collider2D>();
                if (batteryCollider != null)
                {
                    batteryCollider.enabled = false;
                }

                // Exit the loop after processing the first "Battery" found (optional)
                break;
            }
        }
    }
}

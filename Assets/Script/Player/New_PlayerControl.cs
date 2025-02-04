using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class New_PlayerControl : MonoBehaviour
{
    // ----- Player VAR ------
    private Rigidbody2D rb;
    private float weight;
    [SerializeField] private float speed;
    
    // ----- Absorb ------
    public List<GameObject> pickedUpObjects = new List<GameObject>(); // List to store picked-up objects
    private bool canPickup = true; // Cooldown flag to control picking up objects
    public float pickupCooldown = 2f; // Cooldown duration in seconds
    
    // ----- Movement ------
    private Vector2 groundNormal;
    
    /* To do next
     - Add move wall
     - Add absorb limit
     */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weight = rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        if (Input.GetButtonDown("Fire1") && pickedUpObjects.Count > 0)
        {
            Debug.Log("Clicked");
            ReleaseAllTrashObjects();
        }
    }
    
    #region Movement
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // ปรับความเร็วตามน้ำหนัก
        float adjustedSpeed = speed / (1 + weight / 100);
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // ปรับการหันหน้าตัวละคร
        if (moveInput != 0)
        {
            // ใส่ค่า Torque เพื่อให้ตัวละครกลิ้งไปตามทิศทาง
            float torqueAmount = -moveInput * adjustedSpeed;
            rb.AddTorque(torqueAmount);
        }
    }
    #endregion
    
    #region Release_OBJ
    private void ReleaseAllTrashObjects()
    {
        foreach (GameObject trashObject in pickedUpObjects)
        {
            // Detach the trash object from the player
            trashObject.transform.SetParent(null);

            // Check if the trash object already has a Rigidbody2D
            Rigidbody2D rb = trashObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                // Add Rigidbody2D only if it doesn't exist
                rb = trashObject.AddComponent<Rigidbody2D>();
            }

            // Set gravity scale (optional)
            rb.gravityScale = 1;

            Debug.Log($"{trashObject.name} has been released.");
        }

        // Clear the list of picked-up objects
        pickedUpObjects.Clear();

        // Start the cooldown before picking up objects again
        StartCoroutine(PickupCooldown());
    }
    #endregion
    
    #region Magnet_Cooldown
    private IEnumerator PickupCooldown()
    {
        canPickup = false; // Disable picking up objects
        Debug.Log("Pickup disabled for 2 seconds.");
        yield return new WaitForSeconds(pickupCooldown); // Wait for the cooldown duration
        canPickup = true; // Re-enable picking up objects
        Debug.Log("Pickup re-enabled.");
    }
    #endregion
    
    Vector2 AdjustMovementToGround(Vector2 inputDirection)
    {
        // โยนทิศทางการเคลื่อนที่ไปยังระนาบที่ขนานกับพื้น
        Vector2 tangent = Vector2.Perpendicular(groundNormal);
        return Vector2.Dot(inputDirection, tangent) * tangent.normalized;
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player_Control_Base : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private float weight;
    [SerializeField] private float baseWeight;
    public Transform holdPoint;
    
    public List<GameObject> pickedUpObjects = new List<GameObject>(); // List to store picked-up objects
    private bool canPickup = true; // Cooldown flag to control picking up objects
    public float pickupCooldown = 2f; // Cooldown duration in seconds
    
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
            Debug.Log("torque = " +torqueAmount);
        }
    }
    #endregion

    #region Magnet
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player can pick up objects and the collided object has the tag "TrashObject"
        if (canPickup && collision.gameObject.CompareTag("TrashObject") && !pickedUpObjects.Contains(collision.gameObject))
        {
            // Pick up the object
            GameObject trashObject = collision.gameObject;

            // Remove the Rigidbody2D component from the trash object
            Rigidbody2D rb = trashObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Destroy(rb);
            }

            // Set the trash object as a child of the player and move it to the hold point
            trashObject.transform.SetParent(transform);
            if (holdPoint != null)
            {
                trashObject.transform.position = holdPoint.position + Vector3.up * (pickedUpObjects.Count * 0.5f); // Stack objects slightly
            }

            // Add the trash object to the list
            pickedUpObjects.Add(trashObject);

            Debug.Log($"{trashObject.name} has been picked up.");
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
    
}

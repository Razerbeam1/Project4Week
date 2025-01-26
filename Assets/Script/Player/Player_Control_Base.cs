using UnityEngine;

public class Player_Control_Base : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private float weight;
    [SerializeField] private float baseWeight;
    public Transform holdPoint;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weight = rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        
    }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the tag "TrashObject"
        if (collision.gameObject.CompareTag("TrashObject"))
        {
            Debug.Log("touch");
            // Get the Rigidbody2D component of the collided object
            Rigidbody2D objectRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (objectRb != null)
            {
                // Remove the Rigidbody2D component
                Destroy(objectRb);

                // Set the object as a child of the player
                collision.transform.SetParent(transform);

                // Move the object to the holdPoint if specified
                if (holdPoint != null)
                {
                    collision.transform.position = holdPoint.position;
                }

                Debug.Log($"{collision.gameObject.name} has been picked up and Rigidbody2D removed.");
            }
        }
        
    }
    public void DropTrash(Transform trashObject)
    {
        Rigidbody2D objectRb = trashObject.GetComponent<Rigidbody2D>();

        if (objectRb != null)
        {
            // Re-enable the object's Rigidbody2D
            objectRb.simulated = true;

            // Detach the object from the player
            trashObject.SetParent(null);
        }
    }
}

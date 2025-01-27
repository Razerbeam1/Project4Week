using UnityEngine;

public class BouncingObject : MonoBehaviour
{
    [SerializeField] private float bounceForce; // แรงในการเด้ง

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ตรวจสอบว่า Player ชนกับแพลตฟอร์ม
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // เพิ่มแรงกระโดดในแนวตั้ง
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            }
        }
    }
}
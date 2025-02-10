using UnityEngine;

public class BounceForce : MonoBehaviour
{
    private int jumpCount = 0; // ตัวนับการกระโดด
    public Transform checkpoint; // เช็คพอย์ที่ต้องการย้ายไป
    public float jumpThreshold = 4; // จำนวนครั้งที่กระโดดก่อนย้าย
    private bool canMoveToCheckpoint = false; // ตรวจสอบว่าเราสามารถย้ายไปยังเช็คพอย์ได้หรือไม่
    private Transform player; // ตัวแปรสำหรับเก็บอ้างอิงถึง Player

    void Start()
    {
        // หาตัว Transform ของ Player (สมมติว่า Player มี tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.rigidbody;
        if (rb != null)
        {
            // เพิ่มแรงเด้งมากขึ้น โดยการเพิ่มค่าคูณ
            float bounceForce = 20f * (rb.mass / 1); // เพิ่มค่าแรงเด้งให้มากขึ้น

            // ใช้สูตรเพื่อเพิ่มแรงเด้งในทิศทาง Y (เพื่อให้เด้งสูงขึ้น)
            rb.velocity = new Vector2(rb.velocity.x, bounceForce); // เปลี่ยนค่า velocity ให้มีแรงเด้งในทิศทาง Y

            // เพิ่มจำนวนครั้งที่กระโดด
            jumpCount++;

            // ถ้ากระโดดครบ 4 ครั้ง (หรือค่าที่กำหนดใน jumpThreshold)
            if (jumpCount >= jumpThreshold)
            {
                MovePlayerToCheckpoint(); // ย้าย Player ไปยังเช็คพอย์
            }
        }
    }

    void MovePlayerToCheckpoint()
    {
        if (checkpoint != null && player != null)
        {
            // ย้าย Player ไปยังตำแหน่งของเช็คพอย์
            player.position = checkpoint.position;

            // ตั้งค่าความเร็วให้เป็น 0 เพื่อให้ Player หยุดนิ่ง
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero; // หยุดความเร็วทั้งหมด
            }

            jumpCount = 0; // รีเซ็ตจำนวนการกระโดดหลังจากย้าย
        }
    }
}
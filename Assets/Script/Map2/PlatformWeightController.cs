using UnityEngine;

public class PlatformWeightController : MonoBehaviour
{
    public float targetWeight = 5f; // น้ำหนักที่ต้องการเพื่อให้ Platform เลื่อนลงมา
    public float moveSpeed = 2f; // ความเร็วในการเลื่อนลง
    public Transform platform; // อ้างอิง Platform
    public Vector3 targetPosition; // ตำแหน่งสุดท้ายที่ Platform จะเลื่อนลงไป

    private float currentWeight = 0f; // น้ำหนักปัจจุบันของ Player บน Platform
    private bool isMoving = false; // เช็คว่า Platform กำลังเลื่อนลงอยู่หรือไม่

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // เพิ่มน้ำหนักเมื่อ Player หรือวัตถุเข้ามาอยู่บน Platform
            currentWeight += rb.mass;

            // ตรวจสอบว่าน้ำหนักเท่ากับ targetWeight หรือไม่
            if (Mathf.Approximately(currentWeight, targetWeight))
            {
                isMoving = true; // เริ่มเลื่อน Platform ลงมา
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ลดน้ำหนักเมื่อ Player หรือวัตถุออกจาก Platform
            currentWeight -= rb.mass;
            currentWeight = Mathf.Max(currentWeight, 0); // ป้องกันน้ำหนักติดลบ
        }
    }

    private void Update()
    {
        if (isMoving && platform != null)
        {
            // ค่อยๆ เลื่อน Platform ไปยังตำแหน่งที่กำหนด
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);

            // หยุดการเลื่อนเมื่อถึงตำแหน่งเป้าหมาย
            if (platform.position == targetPosition)
            {
                isMoving = false; // หยุดเลื่อน
            }
        }
    }
}


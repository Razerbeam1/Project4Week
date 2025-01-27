using UnityEngine;

public class BreakableBridge : MonoBehaviour
{
    public float maxWeight = 6f; // น้ำหนักสูงสุดที่สะพานสามารถรับได้

    private float currentWeight = 0f; // น้ำหนักปัจจุบันบนสะพาน
    private bool isBroken = false; // เช็คว่าสะพานพังแล้วหรือยัง

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBroken) return; // ถ้าสะพานพังแล้ว ไม่ต้องตรวจสอบอีก

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            currentWeight += rb.mass; // เพิ่มน้ำหนักเมื่อวัตถุเข้ามาบนสะพาน
            if (currentWeight > maxWeight)
            {
                BreakBridge(); // พังสะพานเมื่อเกินน้ำหนัก
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBroken) return; // ถ้าสะพานพังแล้ว ไม่ต้องตรวจสอบอีก

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            currentWeight -= rb.mass; // ลดน้ำหนักเมื่อวัตถุออกจากสะพาน
            currentWeight = Mathf.Max(currentWeight, 0); // ป้องกันน้ำหนักติดลบ
        }
    }

    private void BreakBridge()
    {
        isBroken = true; // ตั้งค่าสถานะว่าสะพานพังแล้ว
        Debug.Log("สะพานพัง!");

        // ทำลายสะพานทันที
        Destroy(gameObject);
    }
}
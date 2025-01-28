using UnityEngine;

public class WeightBox : MonoBehaviour
{
    [SerializeField] public float maxWeight; // น้ำหนักสูงสุดที่ต้องการเพื่อเปิดประตู
    public Transform door; // อ้างอิงประตูที่ต้องเปิด
    [SerializeField] public float doorOpenYPosition; // ตำแหน่ง Y เมื่อประตูเปิดสุด
    [SerializeField] public float openSpeed; // ความเร็วในการเปิดประตู

    private float currentWeight = 0f; // น้ำหนักปัจจุบันในกล่อง
    private float doorClosedYPosition; // ตำแหน่งเริ่มต้นของประตูในแกน Y

    void Start()
    {
        if (door != null)
            doorClosedYPosition = door.position.y; // เก็บตำแหน่ง Y เริ่มต้นของประตู
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            currentWeight += rb.mass; // เพิ่มน้ำหนักเมื่อมีวัตถุเข้ามา
            currentWeight = Mathf.Clamp(currentWeight, 0, maxWeight); // จำกัดน้ำหนักไม่ให้เกิน maxWeight
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            currentWeight -= rb.mass; // ลดน้ำหนักเมื่อวัตถุออกไป
            currentWeight = Mathf.Clamp(currentWeight, 0, maxWeight); // จำกัดน้ำหนักไม่ให้ต่ำกว่า 0
        }
    }

    void Update()
    {
        if (door != null)
        {
            // คำนวณตำแหน่ง Y ของประตูตามน้ำหนักปัจจุบัน
            float weightPercentage = currentWeight / maxWeight; // สัดส่วนของน้ำหนัก
            float targetYPosition = Mathf.Lerp(doorClosedYPosition, doorOpenYPosition, weightPercentage);

            // เคลื่อนที่ประตูในแกน Y อย่างราบรื่น
            door.position = new Vector2(door.position.x, Mathf.MoveTowards(door.position.y, targetYPosition, openSpeed * Time.deltaTime));
        }
    }
}


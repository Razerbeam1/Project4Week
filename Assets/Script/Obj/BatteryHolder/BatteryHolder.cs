using UnityEngine;

public class BatteryHolder : MonoBehaviour
{
    [SerializeField] private Transform platform; // พื้นที่ที่ Player ยืนอยู่
    [SerializeField] private float moveUpDistance; // ระยะทางที่เลื่อนขึ้น
    [SerializeField] private float moveSpeed; // ความเร็วในการเลื่อน
    private bool isPlatformMoving = false; // สถานะการเลื่อนของพื้นที่ยืน
    private bool hasTrashObject = false; // สถานะว่า TrashObject ถูกใส่แล้วหรือยัง

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่ามี Obj ที่มี Tag เป็น TrashObject มาชนหรือไม่
        if (collision.CompareTag("TrashObject"))
        {
            // ลบ Obj ออกจากฉาก
            Destroy(collision.gameObject);

            hasTrashObject = true; // ตั้งค่าว่า TrashObject ถูกใส่แล้ว
            Debug.Log("Trash Object added to the holder!");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // ตรวจสอบว่า Player กำลังยืนอยู่บน platform และ TrashObject ถูกใส่แล้ว
        if (collision.CompareTag("Player") && hasTrashObject && !isPlatformMoving)
        {
            Debug.Log("Player is on the platform trigger. Platform will move up.");
            isPlatformMoving = true; // เริ่มการเลื่อน
            StartCoroutine(MovePlatformUp());
        }
    }

    private System.Collections.IEnumerator MovePlatformUp()
    {
        // คำนวณตำแหน่งปลายทางที่ Platform จะเลื่อนไป
        Vector3 targetPosition = platform.position + new Vector3(0, moveUpDistance, 0);

        // ทำการเลื่อน Platform ขึ้นไปจนถึงตำแหน่งที่กำหนด
        while (Vector3.Distance(platform.position, targetPosition) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // รอเฟรมถัดไป
        }

        Debug.Log("Platform moved up!");
    }
}
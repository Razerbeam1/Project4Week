using UnityEngine;

public class DoorWarp : MonoBehaviour
{
    [SerializeField] private Transform checkpoint; // จุดเช็คพอยต์ที่ต้องการวาร์ปผู้เล่นไป
    [SerializeField] private string playerTag = "Player"; // Tag ของผู้เล่นที่สามารถชนประตูได้

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าผู้เล่นชนประตูหรือไม่
        if (collision.CompareTag(playerTag))
        {
            // ย้ายตำแหน่งของ Player ไปยังตำแหน่ง Checkpoint
            collision.transform.position = checkpoint.position;
        }
    }
}
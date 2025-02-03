using UnityEngine;

public class DoorWarp : MonoBehaviour
{
    [SerializeField] private Transform checkpoint; // จุดเช็คพอยต์ที่ต้องการวาร์ปผู้เล่นไป
    [SerializeField] private string playerTag = "Player"; // Tag ของผู้เล่นที่สามารถชนประตูได้
    [SerializeField] private CameraController cameraController; // ตัวควบคุมกล้องที่อ้างถึง CameraController
    [SerializeField] private int mapIndex = 0; // แผนที่ที่ต้องการเปลี่ยน
    [SerializeField] private SpriteRenderer backgroundRenderer; // ตัวจัดการพื้นหลัง
    [SerializeField] private Sprite newBackgroundSprite; // BG ใหม่ที่ต้องการเปลี่ยนไป

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าผู้เล่นชนประตูหรือไม่
        if (collision.CompareTag(playerTag))
        {
            // ย้ายตำแหน่งของ Player ไปยังตำแหน่ง Checkpoint
            collision.transform.position = checkpoint.position;

            // เปลี่ยนพื้นหลังในกล้อง
            if (cameraController != null)
            {
                cameraController.ChangeMap(mapIndex); // เปลี่ยนแผนที่ในกล้อง
            }

            // เปลี่ยนภาพ BG เป็นของ Map ตามที่กำหนด
            if (backgroundRenderer != null && newBackgroundSprite != null)
            {
                backgroundRenderer.sprite = newBackgroundSprite;
            }
        }
    }
}
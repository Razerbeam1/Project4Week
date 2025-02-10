/*using UnityEngine;

public class DoorWarp : MonoBehaviour
{
    [SerializeField] private Transform checkpoint; // จุดเช็คพอยต์ที่ต้องการวาร์ปผู้เล่นไป
    [SerializeField] private string playerTag = "Player"; // Tag ของผู้เล่นที่สามารถชนประตูได้
    [SerializeField] private CameraController cameraController; // ตัวควบคุมกล้องที่อ้างถึง CameraController
    [SerializeField] private int mapIndex = 0; // เลข ID ของแผนที่ใหม่
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
}*/

using UnityEngine;

public class DoorWarp : MonoBehaviour
{
    [SerializeField] private Transform checkpoint; // จุด Checkpoint ของด่านใหม่
    [SerializeField] private string playerTag = "Player"; // Tag ของ Player
    [SerializeField] private CameraController cameraController; // ตัวควบคุมกล้อง
    [SerializeField] private int mapIndex = 0; // เลข ID ของแผนที่ใหม่
    [SerializeField] private SpriteRenderer backgroundRenderer; // พื้นหลังของด่าน
    [SerializeField] private Sprite newBackgroundSprite; // พื้นหลังของด่านใหม่

    [SerializeField] private GameObject currentLevelPanel; // Panel ของด่านปัจจุบัน
    [SerializeField] private GameObject nextLevelPanel; // Panel ของด่านถัดไป

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            // เปลี่ยนด่าน
            ChangeLevel(collision.transform);
            
            // รีเซ็ตค่าของ Player
            Player_controller2 playerController = collision.GetComponent<Player_controller2>();
            if (playerController != null)
            {
                playerController.ResetPlayerStats(); // รีเซ็ต HP และค่าอื่น ๆ
                playerController.ResetPlayerMass(); // รีเซ็ตมวลของ Player
                playerController.ResetPlayerCollider(); // รีเซ็ตขนาด Collider
                playerController.ClearAbsorbedObjects(); // เคลียร์ Obj ที่ถูกดูด
            }
        }
    }

    private void ChangeLevel(Transform player)
    {
        // ปิด Panel ของด่านปัจจุบัน และเปิด Panel ของด่านถัดไป
        if (currentLevelPanel != null) currentLevelPanel.SetActive(false);
        if (nextLevelPanel != null) nextLevelPanel.SetActive(true);

        // วาร์ป Player ไปยัง Checkpoint ของด่านใหม่
        player.position = checkpoint.position;

        // อัปเดตกล้องให้ตรงกับแผนที่ใหม่
        if (cameraController != null)
        {
            cameraController.ChangeMap(mapIndex);
        }

        // เปลี่ยนพื้นหลัง
        if (backgroundRenderer != null && newBackgroundSprite != null)
        {
            backgroundRenderer.sprite = newBackgroundSprite;
        }

        Debug.Log("Player warped to new level!");
    }
}

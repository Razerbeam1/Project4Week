using UnityEngine;
using UnityEngine.UI; 

public class BoxObject : MonoBehaviour
{
    // กำหนดตัวแปรสำหรับการแสดง Canvas Win
    [SerializeField] private GameObject winCanvas; // UI Canvas สำหรับการแสดงข้อความชนะ
    [SerializeField] private GameObject gamePlayCanvas; // UI Canvas สำหรับ Game Play
    [SerializeField] private GameObject Game_system;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่า Player ชนกับ Box
        if (other.CompareTag("Player"))
        {
            // หยุดการเคลื่อนที่ของ Player
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false; // ปิดการควบคุมของ Player
            }

            // ซ่อน Canvas Game Play
            if (gamePlayCanvas != null)
            {
                gamePlayCanvas.SetActive(false); // ปิด Canvas Game Play
            }

            if (Game_system != null) 
            {
                Game_system.SetActive(false);
            }

            
            // แสดง Canvas Win
            if (winCanvas != null)
            {
                winCanvas.SetActive(true); // เปิด Canvas Win
            }

            // หยุดเกมโดยการตั้งค่า Time.timeScale = 0
            //Time.timeScale = 0f;

            // บันทึกข้อความใน Console
            Debug.Log("Player reached the box! You Win!");
        }
    }
}